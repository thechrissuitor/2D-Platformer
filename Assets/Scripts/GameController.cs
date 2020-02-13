using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] float bufferTime = 0.5f;

    [SerializeField] Text score;
    [SerializeField] Text livesText;
    [SerializeField] Image coinLogo;

    Button restartButton;
    int coins = 000;
    int coinsPerLastLevel = 0;

    private void Awake()
    {
        // establish singleton
        if(FindObjectsOfType<GameController>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        score.text = coins.ToString("000");
        livesText.text = "x " + playerLives.ToString();
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game Over"))
        {
            restartButton = FindObjectOfType<Button>();
        }
        score.text = coins.ToString("000");
    }

    public void CoinCollection()
    {
        coins++;
        score.text = coins.ToString("000");
    }

    public void SaveScoreOnLoad()
    {
        coinsPerLastLevel = coins;
    }

    // call this public method when handling the player's death
    public void playerDeath()
    {
        if(playerLives > 1)
        {
            StartCoroutine("Killed");
        }
        else
        {
            StartCoroutine("restartGame");
        }
    }

    // This method processes the player's death while they still have more lives
    IEnumerator Killed()
    {
        playerLives = playerLives - 1;
        livesText.text = "x " + playerLives.ToString();
        coins = coinsPerLastLevel;

        var currScene = SceneManager.GetActiveScene();

        yield return new WaitForSecondsRealtime(bufferTime);

        SceneManager.LoadScene(currScene.buildIndex);
    }

    // This method processes the player's death while they do not have any more lives
    IEnumerator restartGame()
    {
        playerLives = playerLives - 1;
        livesText.text = "x " + playerLives.ToString();

        yield return new WaitForSecondsRealtime(bufferTime);

        SceneManager.LoadScene("Game Over");
    }
}

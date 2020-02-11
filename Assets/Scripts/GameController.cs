using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] float bufferTime = 0.5f;

    [SerializeField] AudioClip backgroundMusic;

    Button restartButton;
    Text canvasText;
    int coins = 000;

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
        canvasText = FindObjectOfType<Text>();
        canvasText.text = coins.ToString("000");
        DontDestroyOnLoad(FindObjectOfType<Canvas>());

    }

    private void Update()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game Over"))
        {
            restartButton = FindObjectOfType<Button>();
        }

        // this block causes an error
        if (FindObjectsOfType<Canvas>().Length > 1)
        {
            Destroy(FindObjectOfType<Canvas>());
        }
        else
        {
            DontDestroyOnLoad(FindObjectOfType<Canvas>());
        }
        // ~~~

        canvasText.text = coins.ToString("000");
    }

    private void PlayBackgroundMusic()
    {
        AudioSource.PlayClipAtPoint(backgroundMusic, transform.position);
    }

    public void CoinCollection()
    {
        coins++;
        canvasText.text = coins.ToString("000");
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
        coins = 0;
        var currScene = SceneManager.GetActiveScene();

        yield return new WaitForSecondsRealtime(bufferTime);

        SceneManager.LoadScene(currScene.buildIndex);
    }

    // This method processes the player's death while they do not have any more lives
    IEnumerator restartGame()
    {
        yield return new WaitForSecondsRealtime(bufferTime);

        SceneManager.LoadScene("Game Over");
        Destroy(gameObject);
    }
}

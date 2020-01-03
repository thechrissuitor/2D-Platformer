using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] float bufferTime = 0.5f;

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
        var currScene = SceneManager.GetActiveScene();

        yield return new WaitForSecondsRealtime(bufferTime);

        SceneManager.LoadScene(currScene.buildIndex);
    }

    // This method processes the player's death while they do not have any more lives
    IEnumerator restartGame()
    {
        yield return new WaitForSecondsRealtime(bufferTime);

        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}

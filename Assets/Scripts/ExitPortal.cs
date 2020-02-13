using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitPortal : MonoBehaviour
{
    [SerializeField] float loadTime = 3f;
    [SerializeField] float slowMoFactor = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine("loadNextScene");
    }

    IEnumerator loadNextScene()
    {
        Time.timeScale = slowMoFactor;

        yield return new WaitForSecondsRealtime(loadTime);

        var currScene = SceneManager.GetActiveScene();
        int currBuildIndex = currScene.buildIndex;
        FindObjectOfType<GameController>().SaveScoreOnLoad();
        SceneManager.LoadScene(currBuildIndex + 1);

        Time.timeScale = 1f;
    }
}

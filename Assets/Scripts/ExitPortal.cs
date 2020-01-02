using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{
    [SerializeField] float loadTime = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine("loadNextScene");
    }

    IEnumerator loadNextScene()
    {
        yield return new WaitForSecondsRealtime(loadTime);

        var currScene = SceneManager.GetActiveScene();
        int currBuildIndex = currScene.buildIndex;
        SceneManager.LoadScene(currBuildIndex + 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    Button restartButton;

    private void Start()
    {
        restartButton = FindObjectOfType<Button>();
        restartButton.onClick.AddListener(OnTaskClick);
    }

    private void OnTaskClick()
    {
        Destroy(FindObjectOfType<GameController>().gameObject);
        SceneManager.LoadScene(0);
    }
}

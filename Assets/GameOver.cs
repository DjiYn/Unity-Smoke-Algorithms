using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GlobalRenderData globalRenderData;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(globalRenderData.currentSceneName);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

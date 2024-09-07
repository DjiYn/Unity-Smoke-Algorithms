using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GlobalRenderData globalRenderData;

    public void ExitButton()
    {
        Application.Quit();

        Debug.Log("Quit Game!");
    }

    public void StartGame()
    {
        globalRenderData.currentSceneName = "Level0";
        SceneManager.LoadScene("Level0");
        Cursor.lockState = CursorLockMode.Locked;
    }
}

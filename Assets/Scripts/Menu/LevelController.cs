using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GlobalRenderData globalRenderData;

    public string levelName;

    public void NextLevel()
    {
        globalRenderData.currentSceneName = levelName;
        SceneManager.LoadScene(levelName);
    }

}

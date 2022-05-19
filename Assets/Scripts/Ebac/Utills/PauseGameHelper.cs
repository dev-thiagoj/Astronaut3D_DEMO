using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;

public class PauseGameHelper : Singleton<PauseGameHelper>
{
    public GameObject pauseScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        AudioListener.pause = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }
}

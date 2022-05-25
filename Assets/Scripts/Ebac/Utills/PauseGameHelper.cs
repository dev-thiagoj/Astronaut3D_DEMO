using UnityEngine;
using Ebac.Core.Singleton;

public class PauseGameHelper : Singleton<PauseGameHelper>
{
    public GameObject pauseScreen;
    public GameObject confirmScreen;

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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ConfirmQuit()
    {
        confirmScreen.SetActive(true);
    }

    public void DontQuit()
    {
        confirmScreen.SetActive(false);
    }
}

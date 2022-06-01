using UnityEngine.SceneManagement;
using Ebac.Core.Singleton;

public class LoadSceneHelper : Singleton<LoadSceneHelper>
{
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
        SaveManager.Instance.CreateNewSave();
        SaveManager.Instance.LoadFile();
    }


}

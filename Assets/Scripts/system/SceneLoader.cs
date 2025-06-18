using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadStartScene()
    {
        SceneManager.LoadScene("01Start");
    }
    public void LoadEditorScene()
    {
        SceneManager.LoadScene("02Editor");
    }

    public void LoadShootingScene()
    {
        SceneManager.LoadScene("03Shooting");
    }


}

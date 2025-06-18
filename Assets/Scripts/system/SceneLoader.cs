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
        FindAnyObjectByType<UIManager>().ExportSlotsToPatternManager();
        SceneManager.LoadScene("03Shooting");
    }


}

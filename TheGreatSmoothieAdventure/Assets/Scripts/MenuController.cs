using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public string sceneName = "SampleScene";
    public void LoadGame() {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadGame(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame() {
        Application.Quit();
    }
}

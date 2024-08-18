using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame(int sceneIndex) => SceneManager.LoadScene(sceneIndex);

    public void Quit() => Application.Quit();
}

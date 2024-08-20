using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CrossFade fade;

    private int sceneIndex;

    public void StartGame(int sceneIndex) {
        fade.StartFadeOut();
        
        this.sceneIndex = sceneIndex;
    }

    public void OnFadeFinished()
    {
        
        SceneManager.LoadScene(sceneIndex);
    }
        

    public void Quit() => Application.Quit();
}

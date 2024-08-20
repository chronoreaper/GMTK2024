using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CrossFade fade;
    public AudioClip MainGameTheme;

    private int sceneIndex;

    public void StartGame(int sceneIndex) {
        fade.StartFadeOut();
        //AudioManager.Inst.PlayMusic(MainGameTheme);
        this.sceneIndex = sceneIndex;
    }

    public void OnFadeFinished()
    {
        
        SceneManager.LoadScene(sceneIndex);
    }
        

    public void Quit() => Application.Quit();
}

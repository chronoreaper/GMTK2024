using UnityEngine;
using UnityEngine.Events;

public class CrossFade : MonoBehaviour
{
    private Animator myAnimator;

    [Space]
    [Header("On Animation Finished Events")]
    [SerializeField] private UnityEvent OnFadeInFinished;
    [SerializeField] private UnityEvent OnFadeOutFinished;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         StartFadeIn();
    //     }

    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         StartFadeOut();
    //     }
    // }

    public void StartFadeIn()
    {
        myAnimator.SetTrigger("FadeIn");
    }

    public void StartFadeOut()
    {
        myAnimator.SetTrigger("FadeOut");
    }

    protected void FadeInFinished()
    {
        OnFadeInFinished?.Invoke();
    }

    protected void FadeOutFinished()
    {
        OnFadeOutFinished?.Invoke();
    }
}

using System.Collections;
using UnityEngine;

public class CrossFade : MonoBehaviour
{
    [SerializeField] private float transitionTime = 1f;

    private Animator myAnimator;


    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void RunFadeAnimation()
    {
        StartCoroutine(FadeInTransition());
    }

    private IEnumerator FadeInTransition()
    {
        myAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        myAnimator.SetTrigger("End");
    }
}

using System.Collections;
using UnityEngine;

public class CrossFade : MonoBehaviour
{
    private Animator myAnimator;

    private float transitionTime = 1.5f;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Planet.onSwitchPlanetView += RunFadeInAnimation;
        Planet.onSwitchGalaxyView += RunFadeOutAnimation;
    }

    private void OnDisable()
    {
        Planet.onSwitchPlanetView -= RunFadeInAnimation;
        Planet.onSwitchGalaxyView -= RunFadeOutAnimation;
    }

    private void RunFadeInAnimation()
    {
        StartCoroutine(FadeInTransition());
    }

    private IEnumerator FadeInTransition()
    {
        myAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        myAnimator.SetTrigger("End");
    }

    public void RunFadeOutAnimation()
    {
        StartCoroutine(FadeInTransition());
    }
}

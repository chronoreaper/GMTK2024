using System.Collections;
using UnityEngine;

public class CrossFade : MonoBehaviour
{
    private Animator myAnimator;

    private float transitionTime = 1f;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SwitchMapView.onSwitchPlanetView += RunFadeAnimation;
        SwitchMapView.onSwitchGalaxyView += RunFadeAnimation;
    }

    private void OnDisable()
    {
        SwitchMapView.onSwitchPlanetView -= RunFadeAnimation;
        SwitchMapView.onSwitchGalaxyView -= RunFadeAnimation;
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

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    [Header("Slider")]
    [Range(0, 1f)] [SerializeField] private float silderValue;

    public bool CurrentValue { get; private set; }

    private Slider _slider;

    [Header("Animation")]
    [Range(0f, 1f)][SerializeField] private float animationDuration = 0.125f;

    private Coroutine _animationSliderCoroutine;

    [Header("Events")]
    [SerializeField] private UnityEvent onToggleOn;
    [SerializeField] private UnityEvent onToggleOff;

    protected void OnValidate()
    {
        SetupToggleComponent();


        _slider.value = silderValue;
    }

    private void SetupToggleComponent()
    {
        if (_slider != null)
        {
            return;
        }

        SetupSliderComponent();
    }

    private void SetupSliderComponent()
    {
        _slider = GetComponent<Slider>();

        if (_slider == null)
        {
            Debug.LogWarning("No Slider Found");
            return;
        }

        _slider.interactable = false;
        ColorBlock sliderColor = _slider.colors;
        sliderColor.disabledColor = Color.white;
        _slider.colors = sliderColor;
        _slider.transition = Selectable.Transition.None;
    }

    private void Awake()
    {
        SetupToggleComponent();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Toggle();
    }

    private void Toggle()
    {
        SetStateAndAnimation(!CurrentValue);
    }

    private void SetStateAndAnimation(bool state)
    {
        CurrentValue = state;

        if (CurrentValue)
            onToggleOn?.Invoke();
        else
            onToggleOff?.Invoke();

        if (_animationSliderCoroutine != null)
            StopCoroutine(_animationSliderCoroutine);

        _animationSliderCoroutine = StartCoroutine(SliderAnimation());
    }

    private IEnumerator SliderAnimation()
    {
        float startValue = _slider.value;
        float endValue = CurrentValue ? 1 : 0;

        float time = 0;
        if (animationDuration > 0)
        {
            while (time < animationDuration)
            {
                time += Time.deltaTime;

                float lerpFactor = time / animationDuration;
                _slider.value = Mathf.Lerp(startValue, endValue, lerpFactor);

                yield return null;
            }
        }

        _slider.value = endValue;
    }
    
}

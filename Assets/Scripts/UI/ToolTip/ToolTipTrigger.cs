using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string toolTipHeader;
    [TextArea]
    [SerializeField] private string toolTipContent;

    [SerializeField] private float delay;

    public void OnPointerEnter(PointerEventData eventData)
    {
        var customData = eventData.pointerEnter.GetComponent<IGetCustomTip>();

        if (eventData.pointerEnter.GetComponent<TextMeshProUGUI>())
        {
            return;
        }
        
        if (customData != null)
        {
            StartCoroutine(DelayToolTip(customData.GetCustomData(), toolTipHeader));
            return;
        }
        
        StartCoroutine(DelayToolTip(toolTipContent, toolTipHeader));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        ToolTipSystem.Instance.Hide();
    }

    private void OnMouseEnter()
    {
        var customData = transform.GetComponent<IGetCustomTip>();
        
        if (customData != null)
        {
            StartCoroutine(DelayToolTip(customData.GetCustomData(), toolTipHeader));
            return;
        }
        
        StartCoroutine(DelayToolTip(toolTipContent, toolTipHeader));
    }

    private void OnMouseExit()
    {
        StopAllCoroutines();
        ToolTipSystem.Instance.Hide();
    }

    private IEnumerator DelayToolTip(string content, string header)
    {
        yield return new WaitForSeconds(delay);
        ToolTipSystem.Instance.Show(content, header);
    }
}

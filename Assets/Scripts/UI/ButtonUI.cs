using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CostToBuild;

public class ButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 _initialPos;
    RectTransform _transform;

    [SerializeField] private CostToBuild.BuildTypes type;
    [SerializeField] private TextMeshProUGUI _wood;
    [SerializeField] private TextMeshProUGUI _stone;
    [SerializeField] private TextMeshProUGUI _water;
    [SerializeField] private TextMeshProUGUI _lava;


    public void SetCost(ResourceTypes type, int cost)
    {
        switch (type)
        {
            case ResourceTypes.Wood:
                _wood.text = cost.ToString();
                break;
            case ResourceTypes.Stone:
                _stone.text = cost.ToString();
                break;
            case ResourceTypes.Water:
                _water.text = cost.ToString();
                break;
            case ResourceTypes.Lava:
                _lava.text = cost.ToString();
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(Move(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(Move(false));
    }

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        ResourceCost[] cost = null;
        foreach (var i in PlayerMouse.Inst.BuildCosts)
        {
            if (i.Building == type)
            {
                cost = i.Cost;
                break;
            }
        }

        if (cost != null)
        {
            foreach (var resource in cost)
            {
                SetCost(resource.Type, resource.Amount);
            }
        }
    }

    IEnumerator Move(bool up)
    {
        if (_initialPos == Vector3.zero)
            _initialPos = _transform.anchoredPosition;
        Vector2 targetPos = _initialPos;
        if (up)
            targetPos = _initialPos + Vector3.up * 150;
        while ((_transform.anchoredPosition - targetPos).sqrMagnitude > Mathf.Epsilon)
        {
            _transform.anchoredPosition = Vector3.Lerp(_transform.anchoredPosition, targetPos, Time.deltaTime * 3);
            yield return null;
        }
    }
}

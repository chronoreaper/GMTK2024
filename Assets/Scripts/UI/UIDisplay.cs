using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{
    public TextMeshProUGUI WoodQuant;
    public TextMeshProUGUI StoneQuant;
    public TextMeshProUGUI WaterQuant;
    public TextMeshProUGUI LavaQuant;
    public Image HpSlider;

    public void ChangeResourceValue(ResourceTypes type, int value)
    {
        switch (type)
        {
            case ResourceTypes.Wood:
                UpdateText(WoodQuant, value);
                break;
            case ResourceTypes.Stone:
                UpdateText(StoneQuant, value);
                break;
            case ResourceTypes.Water:
                UpdateText(WaterQuant, value);
                break;
            case ResourceTypes.Lava:
                UpdateText(LavaQuant, value);
                break;
        }
    }

    private void UpdateText(TextMeshProUGUI text, int newValue)
    {
        int difference = 0;
        if (int.TryParse(text.text, out int oldValue))
        {
            difference = newValue - oldValue;
        }
        text.text = newValue.ToString();
    }
}

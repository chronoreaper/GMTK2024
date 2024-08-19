using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayValue : MonoBehaviour
{
    public void SetText(float amount) => GetComponent<TextMeshProUGUI>().text = amount.ToString();
}

using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayValue : MonoBehaviour
{
    public void SetText(int amount) => GetComponent<TextMeshProUGUI>().text = amount.ToString();
}

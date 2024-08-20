using TMPro;
using UnityEngine;

public class ErrorMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private Color errorColor;

    public void Init(string text, Color color)
    {
        errorText.text = text;
        errorColor = color;
    }
}

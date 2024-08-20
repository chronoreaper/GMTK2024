using UnityEngine;

public class ErrorMessageCreator : MonoBehaviour
{
    private static ErrorMessageCreator _instance;

    public static ErrorMessageCreator Instance
    {
        get => _instance;
        private set
        {
            if (_instance == null)
            {
                _instance = value;
            }

            if (_instance != value)
            {
                Destroy(value);
            }
        }
    }

    public void Create(string text, Color color)
    {
        
    }
}

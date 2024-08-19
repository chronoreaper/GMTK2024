using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreditsRoller : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Image fadeOut;
    [SerializeField] private float fadeOutSpeed;
    
    private void Update()
    {
        if (transform.localPosition.y >= 1375f)
        {
            FadeOut();
            return;
        }
        
        transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime,
            transform.position.z);
    }

    private void FadeOut()
    {
        var fadeOutObjColor = fadeOut.color;
        fadeOut.color = new Color(fadeOutObjColor.r, fadeOutObjColor.g, fadeOutObjColor.b,
            Mathf.Lerp(fadeOutObjColor.a, 255f, fadeOutSpeed * Time.deltaTime));


        if (fadeOut.color.a >= 255f)
        {
            StartCoroutine(nameof(Quit));
        }
    }

    private IEnumerator Quit()
    {
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }
}

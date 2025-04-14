using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Flash : MonoBehaviour
{
    public Image imageToFlash;
    public float flashSpeed = 2f;

    private void Start()
    {
        StartCoroutine(FlashLoop());
    }

    IEnumerator FlashLoop()
    {
        Color originalColor = imageToFlash.color;

        while (true)
        {
            float alpha = Mathf.PingPong(Time.time * flashSpeed, 1f);
            imageToFlash.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }


    // IEnumerator Flash()
    // {
    //     float timer = 0f;
    //     Color originalColor = imageToFlash.color;

    //     while (timer < flashDuration)
    //     {
    //         // PingPong creates a value that goes up and down between 0 and 1
    //         float alpha = Mathf.PingPong(Time.time * flashSpeed, 1f);
    //         imageToFlash.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    //         timer += Time.deltaTime;
    //         yield return null;
    //     }

    //     // Reset to original
    //     imageToFlash.color = originalColor;
    // }
}

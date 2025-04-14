using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CommsMinigameArrow : MonoBehaviour
{
    [SerializeField] private float interval = 0.5f;
    private Image image;
    
    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(Coro());
    }

    IEnumerator Coro()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            image.enabled = !image.enabled;
        }
    }
}

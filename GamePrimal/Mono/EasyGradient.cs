using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EasyGradient : MonoBehaviour
{
    public Color StartColor = Color.white;
    public Color EndColor = Color.black;
    public Color DecommissionColor = Color.cyan;
    public int FadeTime = 2;
    public bool Decommission = false;

    private void OnEnable()
    {
        gameObject.GetComponent<Image>().color = StartColor;

        StartCoroutine(FadeColor(gameObject, EndColor, FadeTime));
    }

    private void LateUpdate()
    {
        if (Decommission == true)
        {
            gameObject.GetComponent<Image>().color = EndColor;

            StartCoroutine(FadeColor(gameObject, DecommissionColor, FadeTime));

            Decommission = false;
        }
    }

    IEnumerator FadeColor(GameObject objectToFade, Color newColor, float fadeTime = 3)
    {
        Image tempImage = objectToFade.GetComponent<Image>();
        Color currentColor = tempImage.color;
        float counter = 0;

        while (counter < fadeTime)
        {
            counter += Time.deltaTime;
            tempImage.color = Color.Lerp(currentColor, newColor, counter / fadeTime);

            yield return null;
        }
    }
}

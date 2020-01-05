using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasyGradient : MonoBehaviour
{
    void Start() => StartCoroutine(fadeColor(gameObject, Color.black, 2));

    IEnumerator fadeColor(GameObject objectToFade, Color newColor, float fadeTime = 3)
    {
        Image tempImage = objectToFade.GetComponent<Image>();
        Color currentColor = tempImage.color;
        newColor.a = 0.8f;
        float counter = 0;

        while (counter < fadeTime)
        {
            counter += Time.deltaTime;
            tempImage.color = Color.Lerp(currentColor, newColor, counter / fadeTime);

            yield return null;
        }
    }
}

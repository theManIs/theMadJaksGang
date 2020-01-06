using System.Collections;
using System.Collections.Generic;
using Assets.TeamProjects.GamePrimal.Proxies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EasyGradient : MonoBehaviour
{
    public Color StartColor = Color.white;
    public Color EndColor = Color.black;
    public Color DecommissionColor = Color.cyan;
    public int FadeTime = 2;

    private void OnEnable()
    {
        gameObject.GetComponent<Image>().color = StartColor;

        foreach (var img in GetComponentsInChildren<Image>())
            StartCoroutine(FadeColor(img.gameObject, EndColor, FadeTime));
    }

    private void OnDisable()
    {
        gameObject.GetComponent<Image>().color = EndColor;

            foreach (Image img in GetComponentsInChildren<Image>())
                if (gameObject.activeInHierarchy)
                    StartCoroutine(FadeColor(img.gameObject, DecommissionColor, FadeTime));

            foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
                if (gameObject.activeInHierarchy)
                    StartCoroutine(FadeColorText(text.gameObject, DecommissionColor, FadeTime));

    }

    IEnumerator FadeColorText(GameObject objectToFade, Color newColor, float fadeTime = 3)
    {
        TextMeshProUGUI tempImage = objectToFade.GetComponent<TextMeshProUGUI>();
        Color currentColor = tempImage.color;
        float counter = 0;

        while (counter < fadeTime)
        {
            counter += Time.deltaTime;
            tempImage.color = Color.Lerp(currentColor, newColor, counter / fadeTime);

            yield return null;
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

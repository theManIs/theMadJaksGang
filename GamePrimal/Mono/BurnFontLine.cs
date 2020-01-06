using System;
using System.Collections;
using System.Collections.Generic;
using Assets.TeamProjects.GamePrimal.Proxies;
using TMPro;
using UnityEngine;

public class BurnFontLine : MonoBehaviour
{
    public Color TopGradient = new Color(1, 0.5419199f, 0.4757921f);
    public Color BottomGradient = new Color(0.9339623f, 8392934f, 0.4757921f);
    public float LetterDelayTime = .02f;
    public Color FallBackColor = Color.white;
    public float CoolTime = 3f;
    public bool MotherScript = true;
    public Queue<Transform> NextLine = new Queue<Transform>();
    public Transform TheFirstElement = null;

    private string _textToShow;
    private TextMeshProUGUI _textMesh;
    private bool _lineIsDone = false;

    private void Start()
    {
        if (MotherScript)
            if (NextLine.Count == 0)
            {
                foreach (Transform t in transform.parent.transform)
                {
                    NextLine.Enqueue(t);

                    t.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
                }

                TheFirstElement = NextLine.Dequeue();
            }

        _textMesh = GetComponent<TextMeshProUGUI>();
        _textToShow = _textMesh.text;
        _textMesh.text = "";
        _textMesh.color = FallBackColor;
        _textMesh.enableVertexGradient = true;
        _textMesh.colorGradient = new VertexGradient(TopGradient, TopGradient, BottomGradient, BottomGradient);

        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        while (_textToShow.Length > 0)
        {
            yield return new WaitForSeconds(LetterDelayTime);

            _textMesh.text += _textToShow.Substring(0, 1);
            _textToShow = _textToShow.Substring(1);
            
        }

        StartCoroutine(LerpTextColor(_textMesh, FallBackColor, CoolTime));
    }

    private IEnumerator LerpTextColor(TextMeshProUGUI textMeshPro, Color toColor, float reductionTime)
    {
        float deltaBetween = 0;

        while (deltaBetween < (reductionTime / 3))
        {
            deltaBetween += Time.deltaTime;
            VertexGradient vg = new VertexGradient();
            float parabolaGraph = deltaBetween / reductionTime;
            vg.bottomLeft = Color.Lerp(textMeshPro.colorGradient.bottomLeft, toColor, parabolaGraph);
            vg.bottomRight = Color.Lerp(textMeshPro.colorGradient.bottomRight, toColor, parabolaGraph);
            vg.topLeft = Color.Lerp(textMeshPro.colorGradient.topLeft, toColor, parabolaGraph);
            vg.topRight = Color.Lerp(textMeshPro.colorGradient.topRight, toColor, parabolaGraph);
            textMeshPro.colorGradient = vg;

            yield return null;
        }

        textMeshPro.enableVertexGradient = false;
        textMeshPro.color = FallBackColor;
        _lineIsDone = true;
    }

    private void Update()
    {
        if (_lineIsDone && NextLine.Count > 0)
        {
            BurnFontLine actualBfl = gameObject.GetComponent<BurnFontLine>();
            BurnFontLine bfl = NextLine.Dequeue().gameObject.AddComponent<BurnFontLine>();
            bfl.CoolTime = actualBfl.CoolTime;
            bfl.LetterDelayTime = actualBfl.LetterDelayTime;
            bfl.TopGradient = actualBfl.TopGradient;
            bfl.BottomGradient = actualBfl.BottomGradient;
            bfl.FallBackColor = actualBfl.FallBackColor;
            bfl.NextLine = actualBfl.NextLine;
            bfl.MotherScript = false;
            bfl.TheFirstElement = actualBfl.TheFirstElement;

            Destroy(actualBfl);
        } else if (_lineIsDone && NextLine.Count == 0) {
            if (StaticProxyInput.LeftMouse || StaticProxyInput.Space)
            {
                FindObjectOfType<EasyGradient>().enabled = false;

                Destroy(gameObject.GetComponent<BurnFontLine>());

//                BurnFontLine actualBfl = gameObject.GetComponent<BurnFontLine>();
//                BurnFontLine bfl = TheFirstElement.gameObject.AddComponent<BurnFontLine>();
//                bfl.CoolTime = actualBfl.CoolTime;
//                bfl.LetterDelayTime = actualBfl.LetterDelayTime;
//                bfl.TopGradient = actualBfl.TopGradient;
//                bfl.BottomGradient = actualBfl.BottomGradient;
//                bfl.FallBackColor = actualBfl.FallBackColor;
//                bfl.NextLine = actualBfl.NextLine;
//                bfl.MotherScript = false;
//                bfl.TheFirstElement = actualBfl.TheFirstElement;
            }
        }
    }
}

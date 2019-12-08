using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.GamePrimal.Mono
{
    public class TextDamagePro : MonoBehaviour
    {
        private GameObject _canvasFrame;

        // Start is called before the first frame update    
        void Start()
        {
            _canvasFrame = new GameObject("TextEmitter");
            _canvasFrame.AddComponent<Canvas>();
            
            RectTransform canvasRectTransform = _canvasFrame.GetComponent<RectTransform>();
//            canvasRectTransform.sizeDelta = new Vector2(0.35f, 0.35f);
            _canvasFrame.transform.parent = transform;
            _canvasFrame.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            _canvasFrame.transform.localPosition = Vector3.zero;
            //            canvasRectTransform.rect.Set(0,0, 0.35f, 0.35f);

            EmitText();
        }

        private void EmitText()
        {
            GameObject textMesh = new GameObject();
            textMesh.transform.parent = _canvasFrame.transform;
            TextMeshProUGUI textComponent = textMesh.AddComponent<TextMeshProUGUI>();
            textComponent.text = "Fuck it all";
//            textComponent.fontSize = 10;

            RectTransform rt = textMesh.GetComponent<RectTransform>();

            rt.transform.localPosition = new Vector3(0, 5f,0);
            rt.transform.localScale = _canvasFrame.transform.localScale;
//            rt.sizeDelta = new Vector2(1f, 0.35f);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
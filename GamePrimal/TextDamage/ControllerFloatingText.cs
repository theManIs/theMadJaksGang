using System;
using Assets.GamePrimal.TextDamage;
using Assets.TeamProjects.GamePrimal.MainScene;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.GamePrimal.TextDamage
{
    public class ControllerFloatingText
    {
        #region Fields

        private static FloatingText popupTextPrefab;
        private static GameObject _canvasOne;
        private static bool _canvasHasCreated = false; 

        #endregion


        public static void Initialize()
        {
            if (_canvasHasCreated) return;

            popupTextPrefab = Resources.Load<FloatingText>("PopupTextParent");
//            _canvasOne = GameObject.Find("FlatElementsCanvas");
            _canvasOne = CreateCanvas();
            StaticProxyEvent.EMatchHasComeToAnEnd.Event += EventDisposer;
        }

        private static GameObject CreateCanvas()
        {
            MainScene ms = Object.FindObjectOfType<MainScene>();
            GameObject canvasGm = new GameObject("FlatElementsCanvas");
            Canvas can = canvasGm.AddComponent<Canvas>();
            can.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvasHasCreated = true;
            canvasGm.transform.SetParent(ms.transform, false);

            return canvasGm;
        }

        public static void CreateFloatingText(string text, Transform location)
        {
            FloatingText instance = GameObject.Instantiate(popupTextPrefab);
            MeshRenderer mr = location.GetComponent<MeshRenderer>();
            

            Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
            Vector2 halfScreen = new Vector2(screenPosition.x - Screen.width / 2, screenPosition.y - Screen.height / 2 + mr.bounds.max.y);
//            Debug.Log(screenPosition + " " + halfScreen);
//            Debug.Log(mr.bounds.max.y);

//            instance.transform.position = screenPosition;
            instance.transform.position = halfScreen;
            instance.transform.SetParent(_canvasOne.transform, false);
            instance.SetText(text);
        }

        private static void EventDisposer(EventMatchHasComeToAnEndParams acp) => _canvasHasCreated = false;
    }
}

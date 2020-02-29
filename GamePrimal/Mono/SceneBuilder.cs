using Assets.GamePrimal.Helpers;
using Assets.TeamProjects.GamePrimal.SeparateComponents.SceneShifter.Monobeh;
using Assets.TeamProjects.GamePrimal.SeparateComponents.UI.BriefingDisplay;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Mono
{
    public class SceneBuilder : MonoBehaviour
    {
        public Camera CameraRig;
        public SceneCanvasStates CanvasState;
        public ControllerSceneShift SceneShift;
        public MainScene.MainScene MainScene;

        

        private void Start()
        {
            if (CanvasState != SceneCanvasStates.None && !FindObjectOfType<ControllerStartDisplay>())
            {
                ControllerStartDisplay display = gameObject.AddComponent<ControllerStartDisplay>();

                display.SetState(CanvasState);
            }

            if (!FindObjectOfType<ControllerSceneShift>())
                SceneShift = gameObject.AddComponent<ControllerSceneShift>();

            if (!FindObjectOfType<MainScene.MainScene>())
                MainScene = gameObject.AddComponent<MainScene.MainScene>();
        }

    }
}

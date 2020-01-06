using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Mono
{
    public class SceneBuilder : MonoBehaviour
    {
        public Camera CameraRig;
        public bool AllowStartScreen = true;

        private void Start()
        {
            if (AllowStartScreen && !FindObjectOfType<ControllerStartDisplay>())
                gameObject.AddComponent<ControllerStartDisplay>();
        }

    }
}

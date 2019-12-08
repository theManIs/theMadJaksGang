using UnityEngine;

namespace Assets.GamePrimal.CharacterOrtJoyPrafabs.Enemies.Skeleton
{
    public class AccelerationIconScript : MonoBehaviour
    {
        private Camera _mainCamera;
        private Quaternion _originalRot;

        // Start is called before the first frame update
        void Start()
        {
            _mainCamera = Camera.main;
            _originalRot = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = _mainCamera.transform.rotation * _originalRot;
        }
    }
}

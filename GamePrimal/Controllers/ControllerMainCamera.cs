
using Assets.GamePrimal.Controllers;
using UnityEngine;
using UnityStandardAssets.Cameras;

namespace Assets.TeamProjects.GamePrimal.Controllers
{
    public class ControllerMainCamera
    {
        public bool Engaged = true;

        private FreeLookCamWithUserInput _hardCodeCameraRig;
        private Transform _hardCodeCameraPivot;
        private Transform _hardCodeCameraObject;
        private Camera _hardCodeCamera;

        public void UserStart() 
        {
            _hardCodeCameraRig = Object.FindObjectOfType<FreeLookCamWithUserInput>();

            if (!_hardCodeCameraRig)
            {
                Engaged = false;
            }
            else
            {
                _hardCodeCameraPivot = _hardCodeCameraRig.transform.GetChild(0).transform;
                _hardCodeCameraObject = _hardCodeCameraPivot.transform.GetChild(0).transform;
                _hardCodeCamera = _hardCodeCameraObject.GetComponent<Camera>();
            }
        }

        public void UserEnable()
        {
            ControllerRouter.GetControllerDrumSpinner().RoundHandlerEvent += MoveCameraTo;
        }

        public void UserDisable()
        {
            ControllerRouter.GetControllerDrumSpinner().RoundHandlerEvent -= MoveCameraTo;
        }

        public void UserUpdate()
        {
//            Debug.Log(_hardCodeCameraRig.Target);
            if (_hardCodeCameraRig.Target)
                if (_hardCodeCameraRig.HasReachedTarget())
                    _hardCodeCameraRig.SetTarget(null);
        }

        private void MoveCameraTo(Transform activeCharacter)
        {
//            Debug.Log(activeCharacter);
            _hardCodeCameraRig.SetTarget(activeCharacter);
        }
    }
}

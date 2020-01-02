
using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.CameraRigs.CamerasScripts;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
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
            StaticProxyEvent.ETurnWasFound.Event += MoveCameraTo;
        }

        public void UserDisable()
        {
            StaticProxyEvent.ETurnWasFound.Event -= MoveCameraTo;
        }

        public void UserUpdate()
        {
            if (_hardCodeCameraRig  && _hardCodeCameraRig.Target)
                if (_hardCodeCameraRig.HasReachedTarget())
                    _hardCodeCameraRig.SetTarget(null);
        }

        private void MoveCameraTo(EventTurnWasFoundParams args)
        {
            if (_hardCodeCameraRig)
                _hardCodeCameraRig.SetTarget(args.TurnApplicant);
        }
    }
}

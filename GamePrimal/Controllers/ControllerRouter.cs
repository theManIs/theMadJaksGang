using Assets.TeamProjects.GamePrimal.Controllers;
using UnityEngine;

namespace Assets.GamePrimal.Controllers
{
    public class ControllerRouter
    {
        private static ControllerInput _cInput;
        private static ControllerDrumSpinner _cDrum;
        private static ControllerMainCamera _cMCam;
        private static ControllerAttackCapture _cACap;

        public static ControllerEvent GetControllerEvent()
        {
            return new ControllerEvent();
        }

        public static ControllerInput GetControllerInput() => _cInput ?? (_cInput = new ControllerInput());
        public static ControllerDrumSpinner GetControllerDrumSpinner() => _cDrum ?? (_cDrum = new ControllerDrumSpinner());
        public static ControllerMainCamera GetControllerMainCamera() => _cMCam ?? (_cMCam = new ControllerMainCamera());
        public static ControllerAttackCapture GetControllerAttackCapture() => _cACap ?? (_cACap = new ControllerAttackCapture());
    }
}

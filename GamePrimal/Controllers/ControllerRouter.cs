using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using UnityEngine;

namespace Assets.GamePrimal.Controllers
{
    public class ControllerRouter
    {
        private static ControllerInput _cInput;
        private static ControllerDrumSpinner _cDrum;
        private static ControllerMainCamera _cMCam;
        private static ControllerAttackCapture _cACap;
        private static ControllerCharacterMovement _cCMov;
        private static ControllerFocusSubject _cFs;
        private static ControllerPathFinding _cPF;

        public static ControllerEvent GetControllerEvent() => new ControllerEvent();
        public static ControllerInput GetControllerInput() => _cInput ?? (_cInput = new ControllerInput());
        public static ControllerDrumSpinner GetControllerDrumSpinner() => _cDrum ?? (_cDrum = new ControllerDrumSpinner());
        public static ControllerMainCamera GetControllerMainCamera() => _cMCam ?? (_cMCam = new ControllerMainCamera());
        public static ControllerAttackCapture GetControllerAttackCapture() => _cACap ?? (_cACap = new ControllerAttackCapture());
        public static ControllerCharacterMovement GetControllerCharacterMovement() => _cCMov ?? (_cCMov = new ControllerCharacterMovement().UserAwake());
        public static ControllerFocusSubject GetControllerFocusSubject() => _cFs ?? (_cFs = new ControllerFocusSubject());
        public static ControllerPathFinding GetControllerPathFinding() => _cPF ?? ((_cPF = new ControllerPathFinding()));
    }
}

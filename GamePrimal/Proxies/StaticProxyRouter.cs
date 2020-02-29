using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Proxies
{
    public class StaticProxyRouter
    {
        private static ControllerInput _cInput;
        private static ControllerDrumSpinner _cDrum;
        private static ControllerMainCamera _cMCam;
        private static ControllerAttackCapture _cACap;
        private static ControllerCharacterMovement _cCMov;
        private static ControllerFocusSubject _cFs;
        private static ControllerPathFinding _cPF;
        private static bool _debugFlag = false;
        private static bool _theFirstCall = true;

        #region TheFirstCall

        private static void TheFirstCall()
        {
            if (!_theFirstCall) return;

            _theFirstCall = false;

            StaticProxyEvent.EMatchHasComeToAnEnd.Event += TheFirstCallCameToEnd;
//            StaticProxyEvent.EEndOfRound.Event += TheFirstCallCameToEnd;
        }

        private static void TheFirstCallCameToEnd(EventParamsBase epb)
        {
            StaticProxyEvent.EMatchHasComeToAnEnd.Event -= TheFirstCallCameToEnd;
//            StaticProxyEvent.EEndOfRound.Event -= TheFirstCallCameToEnd;
            _theFirstCall = true;

            _cDrum = default;
            _cFs = default;
            _cPF = default;
            _cInput = default;
            _cMCam = default;
            _cACap = default;
            _cCMov = default;

            if (_debugFlag) Debug.LogWarning("ControllerDrumSpinner is dead " + _cDrum);
            if (_debugFlag) Debug.LogWarning("ControllerFocusSubject is dead " + _cFs);
            if (_debugFlag) Debug.LogWarning("ControllerFocusSubject is dead " + _cPF);
        }

        #endregion

        public static ControllerEvent GetControllerEvent() => new ControllerEvent();
        public static ControllerInput GetControllerInput()
        {
            TheFirstCall();

            return _cInput ?? (_cInput = new ControllerInput());
        }

        public static ControllerDrumSpinner GetControllerDrumSpinner()
        {
            TheFirstCall();

            return _cDrum ?? (_cDrum = new ControllerDrumSpinner());
        }

        public static ControllerMainCamera GetControllerMainCamera()
        {
            TheFirstCall();

            return _cMCam ?? (_cMCam = new ControllerMainCamera());
        }

        public static ControllerAttackCapture GetControllerAttackCapture()
        {
            TheFirstCall();
            
            return _cACap ?? (_cACap = new ControllerAttackCapture());
        }

        public static ControllerCharacterMovement GetControllerCharacterMovement()
        {
            TheFirstCall();

            return _cCMov ?? (_cCMov = new ControllerCharacterMovement().UserAwake());
        }

        public static ControllerFocusSubject GetControllerFocusSubject()
        {
            TheFirstCall();
            
            return (_cFs ?? (_cFs = new ControllerFocusSubject()));
        }

        public static ControllerPathFinding GetControllerPathFinding()
        {
            TheFirstCall();
            
            return _cPF ?? ((_cPF = new ControllerPathFinding()));
        }
    }
}

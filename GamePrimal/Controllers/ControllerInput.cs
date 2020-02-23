using Assets.GamePrimal.Navigation.HighlightFrame;
using UnityEngine;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using static Assets.TeamProjects.GamePrimal.Proxies.StaticProxyRouter;

namespace Assets.GamePrimal.Controllers
{
    public struct PressedButtons
    {
        public bool EscapeButton;
        public bool N;
        public bool P;
        public bool O;
        public bool L;
    }

    public class ControllerInput
    {
        private PressedButtons _pressedButtons;
        private ControllerMainCamera _controllerMainCamera;
        private ControllerCharacterMovement _cMovementCharacter;
        private ControllerFocusSubject _subjectFocus;
        private CursorChanger _cursorChanger;
        private ControllerDrumSpinner _cDrumSpinner;

        public ControllerInput UserAwake()
        {
            _controllerMainCamera = GetControllerMainCamera();
            _cMovementCharacter = GetControllerCharacterMovement();
            _subjectFocus = GetControllerFocusSubject();
            _cursorChanger = new CursorChanger();
            _cDrumSpinner = GetControllerDrumSpinner();

            return this;
        }

        public void UserEnable()
        {
            _controllerMainCamera.UserEnable();
            _cMovementCharacter.UserEnable();
        }

        public void UserDisable()
        {
            _controllerMainCamera.UserDisable();
            _cMovementCharacter.UserDisable();
        }

        public void Start()
        {
            _controllerMainCamera.UserStart();
            _cursorChanger.UserStart(new StartParams());
        }

        public void Update()
        {
            _pressedButtons = new PressedButtons();

            if (Input.GetKeyDown(KeyCode.Escape))
                _pressedButtons.EscapeButton = true;
            else if (Input.GetKeyDown(KeyCode.O))
                _pressedButtons.O = true;
            else if (Input.GetKeyDown(KeyCode.P))
                _pressedButtons.P = true;
            else if (Input.GetKeyDown(KeyCode.L))
                _pressedButtons.L = true;
            
            _subjectFocus.UpdateOnce();
            _controllerMainCamera.UserUpdate();
            _cMovementCharacter.FixedUpdate(_subjectFocus.GetFocus(), _subjectFocus.HasFocused());
//            _cursorChanger.SetCursor(_subjectFocus.GetSoftFocus(), _subjectFocus.GetHardFocus());
            _cursorChanger.SetCursorIfOnUi(_subjectFocus.GetSoftFocus(), _subjectFocus.GetHardFocus(), StaticProxyStateHolder.GetStatesList());

            if (StaticProxyInput.Space)
                _cDrumSpinner.ReleaseRound();
        }

    }
}

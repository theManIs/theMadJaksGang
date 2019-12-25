using Assets.GamePrimal.Navigation.HighlightFrame;
using UnityEngine;
using Assets.GamePrimal.SeparateComponents.PauseMenu;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using static Assets.GamePrimal.Controllers.ControllerRouter;

namespace Assets.GamePrimal.Controllers
{
    public struct PressedButtons
    {
        public bool EscapeButton;
    }

    public class ControllerInput
    {
        private PressedButtons _pressedButtons;
        private PauseMenuBlock _pauseMenu;
        private ControllerMainCamera _controllerMainCamera;
        private ControllerCharacterMovement _cMovementCharacter;
        private ControllerFocusSubject _subjectFocus;
        private CursorChanger _cursorChanger;

        public ControllerInput UserAwake()
        {
            _controllerMainCamera = GetControllerMainCamera();
            _pauseMenu = new PauseMenuBlock();
            _cMovementCharacter = GetControllerCharacterMovement();
            _subjectFocus = GetControllerFocusSubject();
            _cursorChanger = new CursorChanger();

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
            _pauseMenu.Start();
            _controllerMainCamera.UserStart();
            _cursorChanger.UserStart(new StartParams());
        }

        public void Update()
        {
            _pressedButtons = new PressedButtons();

            if (Input.GetKeyDown(KeyCode.Escape))
                _pressedButtons.EscapeButton = true;

            _subjectFocus.UpdateOnce();
            _pauseMenu.GetToMainMenu(_pressedButtons);
            _controllerMainCamera.UserUpdate();
            _cMovementCharacter.FixedUpdate(_subjectFocus.GetFocus(), _subjectFocus.HasFocused());
            _cursorChanger.SetCursor(_subjectFocus.GetSoftFocus(), _subjectFocus.GetHardFocus());
        }

    }
}

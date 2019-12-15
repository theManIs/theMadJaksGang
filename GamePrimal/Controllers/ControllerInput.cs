using Assets.GamePrimal.Navigation.HighlightFrame;
using Assets.GamePrimal.Navigation.Pathfinder;
using UnityEngine;
using Assets.GamePrimal.SeparateComponents.PauseMenu;
using Assets.TeamProjects.GamePrimal.Controllers;

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
        private FetchMovablePoint _cMovementCharacter;
        private SubjectFocus _subjectFocus;

        public ControllerInput UserAwake()
        {
            _controllerMainCamera = ControllerRouter.GetControllerMainCamera();
            _pauseMenu = new PauseMenuBlock();
            _cMovementCharacter = new FetchMovablePoint();
            _subjectFocus = new SubjectFocus();

            return this;
        }

        public void UserEnable()
        {
            _controllerMainCamera.UserEnable();
        }

        public void UserDisable()
        {
            _controllerMainCamera.UserDisable();
        }

        public void Start()
        {
            _subjectFocus.Start();
            _pauseMenu.Start();
            _controllerMainCamera.UserStart();
        }

        public void Update()
        {
            _pressedButtons = new PressedButtons();

            if (Input.GetKeyDown(KeyCode.Escape))
                _pressedButtons.EscapeButton = true;

            _subjectFocus.FixedUpdate();
            _pauseMenu.GetToMainMenu(_pressedButtons);
            _controllerMainCamera.UserUpdate();
            _cMovementCharacter.FixedUpdate(_subjectFocus.GetFocus(), _subjectFocus.HasFocused());
        }

    }
}

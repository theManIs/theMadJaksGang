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

        public ControllerInput UserAwake()
        {
            _controllerMainCamera = ControllerRouter.GetControllerMainCamera();
            _pauseMenu = new PauseMenuBlock();

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
            _pauseMenu.Start();
            _controllerMainCamera.UserStart();
        }

        public void Update()
        {
            _pressedButtons = new PressedButtons();

            if (Input.GetKeyDown(KeyCode.Escape))
                _pressedButtons.EscapeButton = true;

            _pauseMenu.GetToMainMenu(_pressedButtons);
            _controllerMainCamera.UserUpdate();
        }

    }
}

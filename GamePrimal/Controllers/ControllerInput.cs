using UnityEngine;
using Assets.GamePrimal.SeparateComponents.PauseMenu;

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

        public void Start()
        {
            _pauseMenu = new PauseMenuBlock();

            _pauseMenu.Start();
        }

        public void Update()
        {
            _pressedButtons = new PressedButtons();

            if (Input.GetKeyDown(KeyCode.Escape))
                _pressedButtons.EscapeButton = true;

            _pauseMenu.GetToMainMenu(_pressedButtons);
        }
    }
}

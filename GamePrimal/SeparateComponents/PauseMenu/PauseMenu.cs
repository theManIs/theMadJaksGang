using Assets.GamePrimal.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GamePrimal.SeparateComponents.PauseMenu
{
    public class PauseMenuBlock
    {
        private SceneShift _sceneShift;

        public void Start()
        {
            ControllerEvent.UserInputBroadcaster += GetToMainMenu;
            _sceneShift = Object.FindObjectOfType<SceneShift>();
        }

        public void Destroy()
        {
            ControllerEvent.UserInputBroadcaster -= GetToMainMenu;
        }

        public void GetToMainMenu(PressedButtons _pressedButtons)
        {

            if (_pressedButtons.EscapeButton)
            {
                Debug.Log("ReloadScene");
                SceneManager.LoadScene(_sceneShift.SceneManagerStruct.MainMenuScene);
            }
        }
    }
}
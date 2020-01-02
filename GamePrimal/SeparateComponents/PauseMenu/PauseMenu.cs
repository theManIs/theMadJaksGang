using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.SeparateComponents.SceneShifter.Monobeh;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.PauseMenu
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

        public void GetToMainMenu(PressedButtons pressedButtons)
        {
            if (!_sceneShift) return;

            if (pressedButtons.P)
                _sceneShift.LoadPureWeaponScene();
            else if (pressedButtons.L)
                _sceneShift.LoadMapScene();
            else if (pressedButtons.O)
                _sceneShift.LoadChurchFirstFloorScene();
        }
    }
}
using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.SeparateComponents.HudPack.Mono;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.HudPack.Scripts
{
    public class HudViewer : IUserAwake, IUpdate
    {
        private ActionPointsHolder _actionPointsHolder;
        private ControllerFocusSubject _cFocusSubject;

        public void UserAwake(AwakeParams ap)
        {
            _cFocusSubject = ControllerRouter.GetControllerFocusSubject();
            _actionPointsHolder = Object.FindObjectOfType<ActionPointsHolder>();
        }

        public void UserStart(StartParams sp)
        {
            throw new System.NotImplementedException();
        }


        public void ShowTurnPoints(int points, Transform actualInvoker)
        {
            if (actualInvoker == _cFocusSubject.GetHardFocus())
                foreach (Transform t in _actionPointsHolder.transform)
                {
                    t.GetComponent<Image>().enabled = points > 0;
                    points -= 1;
                }
        }

        public void UserUpdate()
        {
            _cFocusSubject.UpdateOnce();
        }
    }
}
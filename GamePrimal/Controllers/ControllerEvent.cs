using Assets.TeamProjects.GamePrimal.Controllers;
using UnityEngine;

namespace Assets.GamePrimal.Controllers
{
    public class ControllerEvent
    {
        public delegate void HitDetected(AttackCaptureParams acp);
        public static event HitDetected HitDetectedHandler;

        public delegate void HitApplied(Transform captured, Transform broadcaster);
        public static event HitApplied HitAppliedHandler;

        public delegate void UserInput(PressedButtons pressedButtons);
        public static event UserInput UserInputBroadcaster;

        public void UserInputInvoke(PressedButtons pressedButtons)
        {
            UserInputBroadcaster?.Invoke(pressedButtons);
        }

        public void HitDetectedInvoke(AttackCaptureParams acp)
        {
            HitDetectedHandler?.Invoke(acp);
        }

        public void HitAppliedHandlerInvoke(Transform ally, Transform enemy)
        {
            HitAppliedHandler?.Invoke(ally, enemy);
        }
    }
}

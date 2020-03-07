using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Proxies
{
    public class StaticProxyEvent
    {
        public static EventEndOfRound EEndOfRound = new EventEndOfRound();
        public static EventTurnWasFound ETurnWasFound = new EventTurnWasFound();
        public static EventActiveAbilityChanged EActiveAbilityChanged = new EventActiveAbilityChanged();
        public static EventMatchHasComeToAnEnd  EMatchHasComeToAnEnd = new EventMatchHasComeToAnEnd();
    }

    public class ControllerEvent  //todo dissolve this class
    {
        public delegate void HitDetected(AttackCaptureParams acp);
        public static event HitDetected HitDetectedHandler;

        public delegate void HitApplied(Transform captured, Transform broadcaster);
        public static event HitApplied HitAppliedHandler;

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

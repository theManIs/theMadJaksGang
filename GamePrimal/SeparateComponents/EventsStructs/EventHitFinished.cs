using Assets.TeamProjects.GamePrimal.Controllers;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public delegate void EventHitFinishedDelegate(EventHitFinishedParams acp);

    public struct EventHitFinishedParams
    {
        public Transform TurnApplicant;
    }

    public struct EventHitFinished
    {
        public event EventHitFinishedDelegate Event;
        public void Invoke(EventHitFinishedParams args) => Event?.Invoke(args);
    }
}
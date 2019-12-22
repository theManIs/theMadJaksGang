using Assets.TeamProjects.GamePrimal.Controllers;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public delegate void EventTurnWasFoundDelegate(EventTurnWasFoundParams acp);

    public struct EventTurnWasFoundParams
    {
        public Transform TurnApplicant;
    }

    public struct EventTurnWasFound
    {
        public event EventTurnWasFoundDelegate Event;
        public void Invoke(EventTurnWasFoundParams args) => Event?.Invoke(args);
    }
}
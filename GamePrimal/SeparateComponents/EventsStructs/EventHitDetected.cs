using Assets.TeamProjects.GamePrimal.Controllers;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public class HitDetectedParams : EventParamsBase
    {
        public Transform Source;
        public Transform Target;
        public bool HasHit;
        public bool HasDied;
    }

    public struct EventHitDetected
    {
        public event EventParamsBaseDelegate HitDetectedEvent;
        public void Invoke(HitDetectedParams acp) => HitDetectedEvent?.Invoke(acp);
    }
}
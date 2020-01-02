using Assets.TeamProjects.GamePrimal.Controllers;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public struct EventHitFinished
    {
        public event EventParamsBaseDelegate Event;
        public void Invoke(EventParamsBase args) => Event?.Invoke(args);
    }
}
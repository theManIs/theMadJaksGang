using Assets.TeamProjects.GamePrimal.Controllers;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public delegate void EventMoveCostAppliedDelegate(EventMoveCostAppliedParams acp);

    public struct EventMoveCostAppliedParams
    {
        public Transform ActualInvoker;
    }
    
    public struct EventMoveCostApplied
    {
        public event EventMoveCostAppliedDelegate Event;
        public void Invoke(EventMoveCostAppliedParams args) => Event?.Invoke(args);
    }
}
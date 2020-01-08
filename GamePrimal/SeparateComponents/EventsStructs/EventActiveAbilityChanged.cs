namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public delegate void EventActiveAbilityChangedDelegate(EventActiveAbilityChangedParams acp);

    public class EventActiveAbilityChangedParams : EventParamsBase
    {
        public bool AbilityInLockMode;
        public string AbilityName;
    }

    public struct EventActiveAbilityChanged
    {
        public event EventActiveAbilityChangedDelegate Event;
        public void Invoke(EventActiveAbilityChangedParams args) => Event?.Invoke(args);
    }
}
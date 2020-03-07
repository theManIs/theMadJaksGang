using Assets.GamePrimal.Mono;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public delegate void EventEndOfRoundDelegate(EventEndOfRoundParams acp);
    public class EventEndOfRoundParams : EventParamsBase
    {
        public MonoMechanicus Monomech;
    }

    public struct EventEndOfRound
    {
        public event EventEndOfRoundDelegate Event;

        public void Invoke(EventEndOfRoundParams args) => Event?.Invoke(args);
    }
}
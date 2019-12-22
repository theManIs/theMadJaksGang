using Assets.TeamProjects.GamePrimal.Controllers;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public delegate void EventEndOfRoundDelegate(EventEndOfRoundParams acp);

    public struct EventEndOfRoundParams
    {

    }

    public struct EventEndOfRound
    {
        public event EventEndOfRoundDelegate Event;
        public void Invoke(EventEndOfRoundParams args) => Event?.Invoke(args);
    }
}
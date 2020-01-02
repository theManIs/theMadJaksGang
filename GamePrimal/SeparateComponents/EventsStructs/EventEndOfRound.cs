using Assets.TeamProjects.GamePrimal.Controllers;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public struct EventEndOfRound
    {
        public event EventParamsBaseDelegate Event;
        public void Invoke(EventParamsBase args) => Event?.Invoke(args);
    }
}
using Assets.GamePrimal.Helpers;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public delegate void EventMatchHasComeToAnEndDelegate(EventMatchHasComeToAnEndParams acp);

    public class EventMatchHasComeToAnEndParams : EventParamsBase
    {
        public SceneField NewScene;
    }

    public struct EventMatchHasComeToAnEnd
    {
        public event EventMatchHasComeToAnEndDelegate Event;
        public void Invoke(EventMatchHasComeToAnEndParams args) => Event?.Invoke(args);
    }
}
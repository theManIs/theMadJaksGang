using Assets.GamePrimal.Helpers;
using Assets.TeamProjects.GamePrimal.SeparateComponents.SceneShifter;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public delegate void EventMatchHasComeToAnEndDelegate(EventMatchHasComeToAnEndParams acp);

    public class EventMatchHasComeToAnEndParams : EventParamsBase
    {
        public SceneField NewScene;
        public SceneIndexerEnum BuildIndex;
    }

    public struct EventMatchHasComeToAnEnd
    {
        public event EventMatchHasComeToAnEndDelegate Event;
        public void Invoke(EventMatchHasComeToAnEndParams args) => Event?.Invoke(args);
    }
}
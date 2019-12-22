using Assets.TeamProjects.GamePrimal.Controllers;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public delegate void HitDetectedDelegate(AttackCaptureParams acp);

    public struct HitDetectedParams
    {

    }

    public struct EventHitDetected
    {
        public event HitDetectedDelegate HitDetectedEvent;
        public void Invoke(AttackCaptureParams acp) => HitDetectedEvent?.Invoke(acp);
    }
}
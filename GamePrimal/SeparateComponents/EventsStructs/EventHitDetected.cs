namespace Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs
{
    public delegate void HitDetectedDelegate(HitDetectedParams hdp);

    public struct HitDetectedParams
    {

    }

    public struct EventHitDetected
    {
        public event HitDetectedDelegate HitDetectedEvent;
        public void Invoke(HitDetectedParams hdp) => HitDetectedEvent?.Invoke(hdp);
    }
}
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.InterfaceHold
{
    public interface IHitDetectedHandler
    {
        void HitDetectedHandler(AnimationEvent ae);
    }
    public interface IHitEndedHandler
    {
        void HitEndedHandler(AnimationEvent ae);
    }
}
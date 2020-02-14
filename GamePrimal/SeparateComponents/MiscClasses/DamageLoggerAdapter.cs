using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Proxies;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses
{
    public class DamageLoggerAdapter
    {
        public static void AttackCapture(MonoMechanicus attackSource, MonoMechanicus attackTarget)
        {
            AttackCaptureParams acp = new AttackCaptureParams() { Source = attackTarget.transform, Target = attackSource.transform, HasHit = false};

            StaticProxyRouter.GetControllerEvent().HitDetectedInvoke(acp);
            StaticProxyStateHolder.LockModeOn = false;
        }
    }
}
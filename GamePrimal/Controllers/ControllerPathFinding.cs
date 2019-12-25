using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Navigation.Pathfinder;
using Assets.TeamProjects.GamePrimal.SeparateComponents.InterfaceHold;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Controllers
{
    public class ControllerPathFinding : IUserStart, IUpdate
    {
        private readonly TracerProjectileScript _tracerProjectile = new TracerProjectileScript();
        private ControllerFocusSubject _cSubjectFocus = ControllerRouter.GetControllerFocusSubject();

        public void UserStart(StartParams sp)
        {
            _tracerProjectile.UserStart();
        }

        public void UserUpdate(UpdateParams up)
        {
            Transform focusedObject = _cSubjectFocus.GetFocus();
            MonoAmplifierRpg monoAmp = focusedObject?.GetComponent<MonoAmplifierRpg>();

            if (focusedObject && monoAmp)
                _tracerProjectile.UserUpdate(new UpdateParams()
                    {ActualInvoker = focusedObject, AmplifierRpg = monoAmp});

        }
    }
}
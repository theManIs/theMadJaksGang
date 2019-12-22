using Assets.GamePrimal.Mono;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.InterfaceHold
{
    public class UpdateParams
    {
        public MonoAmplifierRpg AmplifierRpg;
        public Transform ActualInvoker;
    }

    public interface IUpdate
    {
        void UserUpdate(UpdateParams up);
    }
}
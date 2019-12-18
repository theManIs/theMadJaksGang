using Assets.GamePrimal.TextDamage;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold
{
    public interface IUserAwake
    {
        void UserAwake(AwakeParams ap);
    }

    public struct AwakeParams
    {
        public float MeshSpeed;
        public Animator AnimatorComponent;
        public DamageLogger DamageLoggerComponent;
        public NavMeshAgent NavMeshAgentComponent;
    }
}
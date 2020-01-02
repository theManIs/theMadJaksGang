using Assets.GamePrimal.TextDamage;
using Assets.TeamProjects.DemoAnimationScene.MiscellaneousWeapons.CommonScripts;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold
{
    public interface IUserAwake
    {
        void UserAwake(AwakeParams ap);
    }

    public interface IUserAwakeInstantiator<T>
    {
        T UserAwakeInstantiator(AwakeParams ap);
    }


    public interface IUserStart
    {
        void UserStart(StartParams sp);
    }

    public struct StartParams
    {
        public WeaponTypes WeaponType;
        public float NavMeshSpeed;
    }

    public struct AwakeParams
    {
        public float MeshSpeed;
        public Animator AnimatorComponent;
        public DamageLogger DamageLoggerComponent;
        public NavMeshAgent NavMeshAgentComponent;
        public WeaponOperator WieldingWeapon;
    }
}
using Assets.GamePrimal.TextDamage;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using Assets.TeamProjects.GamePrimal.SeparateComponents.WeaponOrigins;
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
        T UserAwakeInstantiator(ref AwakeParams ap);
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
        public ControllerDrumSpinner CDrumSpinner;
        public EventEndOfRound EEndOfRound;
    }
}
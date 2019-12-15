using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses
{
    public class CharacterAnimator : IUserAwake, ISwitchable, IUpdate
    {
        private Animator _animator;
        public bool Engaged = true;
        private DamageLogger _dmLogger;
        private NavMeshAgent _navMeshAgent;
        private bool _isBlip = false;

        public void UserAwake()
        {

        }

        public CharacterAnimator SetAnimatorComponent(Animator anim)
        {
            _animator = anim;

            return this;
        }

        public CharacterAnimator SetDamageLogger(DamageLogger dm)
        {
            _dmLogger = dm;

            return this;
        }

        public CharacterAnimator SetNavAgent(NavMeshAgent nvm)
        {
            _navMeshAgent = nvm;

            return this;
        }


        private void ReactOnHit(Transform source, Transform target, string animName)
        {
            if (!Engaged) return;

            _animator.SetTrigger(animName);
        }

        public void UserEnable()
        {
            if (Engaged)
                Engaged = _animator && _navMeshAgent && _dmLogger;

            if (Engaged)
                _dmLogger.ReactOnHit += ReactOnHit;
        }

        public void UserDisable()
        {
            if (Engaged)
                _dmLogger.ReactOnHit -= ReactOnHit;
        }

        public void UserUpdate()
        {
            if (Engaged)
                if (!_isBlip && _navMeshAgent.hasPath)
                {
                    _isBlip = true;

//                    _animator.SetBool("IsStopped", false);
                    _animator.StopPlayback();
                    _animator.SetFloat("Blend", 1 );
                    _navMeshAgent.speed = 4;
                }
                else if (_isBlip && !_navMeshAgent.hasPath)
                {
                    _isBlip = false;

//                    _animator.SetBool("IsStopped", true);
                    _animator.SetFloat("Blend", 0);
                }
        }
    }
}

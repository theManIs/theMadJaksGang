using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Controllers;
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
        private Transform _transform;

        public void UserAwake(AwakeParams ap)
        {
            _animator = ap.AnimatorComponent;
            _transform = _animator.transform;
            _dmLogger = ap.DamageLoggerComponent;
            _navMeshAgent = ap.NavMeshAgentComponent;
            _navMeshAgent.speed = ap.MeshSpeed;
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


        private void ReactOnHit(AttackCaptureParams acp)
        {
            if (!Engaged) return;

            _animator.SetTrigger(acp.HasDied ? "Died" : "Hit");
        }

        public void UserEnable()
        {
            if (Engaged)
                Engaged = _animator && _navMeshAgent && _dmLogger;

            if (Engaged && _dmLogger)
            {
                _dmLogger.ReactOnHit += ReactOnHit;
                _dmLogger.AttackStarted += AttackStarted;
            }
                
        }

        private void AttackStarted(AttackCaptureParams acp)
        {
            _animator.SetTrigger("Attacking");
            _transform.LookAt(acp.Source);
        }

        public void UserDisable()
        {
            if (_dmLogger)
                _dmLogger.ReactOnHit -= ReactOnHit;
        }

        public void UserUpdate()
        {
            if (Engaged)
                if (!_isBlip && _navMeshAgent.hasPath)
                {
                    _isBlip = true;

                    _animator.SetBool("IsStopped", false);
//                    _animator.StopPlayback();
//                    _animator.SetFloat("Blend", 1 );
//                    _navMeshAgent.speed = 1.5f;
                }
                else if (_isBlip && !_navMeshAgent.hasPath)
                {
                    _isBlip = false;

                    _animator.SetBool("IsStopped", true);
//                    _animator.SetFloat("Blend", 0);
                }
        }
    }
}

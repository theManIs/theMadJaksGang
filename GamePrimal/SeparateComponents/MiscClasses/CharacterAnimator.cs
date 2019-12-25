using System;
using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.DemoAnimationScene.MiscellaneousWeapons.CommonScripts;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using Assets.TeamProjects.GamePrimal.SeparateComponents.InterfaceHold;
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
        private WeaponOperator _wieldingWeapon;
        private AttackCaptureParams _lastAttackCapture;
        private float _baseMeshSpeed;
        private readonly float _minNavMeshSpeed = 0.04f;
        private readonly float _minNormalizedValue = 0.01f;
        private bool _attacking = false;
        private Transform _lastEnemy;
        private MonoAmplifierRpg _monoAmplifierRpg;

        public void UserAwake(AwakeParams ap)
        {
            _animator = ap.AnimatorComponent;
            _transform = _animator.transform;
            _dmLogger = ap.DamageLoggerComponent;
            _navMeshAgent = ap.NavMeshAgentComponent;
            _wieldingWeapon = ap.WieldingWeapon;
            _monoAmplifierRpg = _transform.GetComponent<MonoAmplifierRpg>();
        }

        public void  UserStart(StartParams sp)
        {
            Engaged = _animator && _navMeshAgent && _dmLogger;
            _navMeshAgent.speed = sp.NavMeshSpeed;
//            Debug.Log(sp.NavMeshSpeed);
            _baseMeshSpeed = sp.NavMeshSpeed;
            _animator.SetInteger("WeaponType", (int)sp.WeaponType);
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
            if (Engaged && _dmLogger)
            {
                _dmLogger.ReactOnHit += ReactOnHit;
                _dmLogger.AttackStarted += AttackStarted;
                _dmLogger.EHitFinished.Event += HitFinished;
            }

        }

        public void UserDisable()
        {
            if (_dmLogger)
            {
                _dmLogger.ReactOnHit -= ReactOnHit;
                _dmLogger.AttackStarted -= AttackStarted;
                _dmLogger.EHitFinished.Event -= HitFinished;
            }
        }

        private void HitFinished(EventHitFinishedParams acp)
        {
            _attacking = false;
        }

        private void AttackStarted(AttackCaptureParams acp)
        {
            _animator.SetTrigger("Attacking");
            _transform.LookAt(acp.Source);
            _lastAttackCapture = acp;
            _attacking = true;
            _lastEnemy = acp.Source;

//            if (_monoAmplifierRpg.WieldingWeapon.isRanged)
//                _monoAmplifierRpg.WieldingWeapon.ShootAnyProjectile(_monoAmplifierRpg.WeaponProjectile.transform, acp.Source);

//            AnimatorClipInfo[] animatorClipInfo = _animator.GetCurrentAnimatorClipInfo(0);
//
//            if (animatorClipInfo.Length > 0 && animatorClipInfo[0].clip.name == "RifleFiring")
//            {
//                animatorClipInfo[0].clip.AddEvent(new AnimationEvent()
//                {
//                    
//                });
//            }

//            Debug.Log(_animator.GetCurrentAnimatorClipInfoCount(0));
//            Debug.Log(_animator.GetCurrentAnimatorClipInfo(0));
        }


        public void UserUpdate(UpdateParams up)
        {
            if (!Engaged) return;

            if (Engaged)
                if (!_isBlip && _navMeshAgent.hasPath)
                {
                    _isBlip = true;

                    _animator.SetBool("IsStopped", false);
                    _animator.SetFloat("MovementBlend", _minNormalizedValue);
                }
                else if (_isBlip && !_navMeshAgent.hasPath)
                {
                    _isBlip = false;

                    _animator.SetBool("IsStopped", true);
                    _animator.SetFloat("MovementBlend", 0);
                }

            if (Engaged)
                if (_isBlip)
                {
                    double towardAngle = GetYAngle(_navMeshAgent.steeringTarget, _transform.position);

                    float turningBlend = (float) Math.Sin(towardAngle * Mathf.Deg2Rad);
                    float movementBlend = (float) Math.Cos(towardAngle * Mathf.Deg2Rad);

                    _animator.SetFloat("TurningBlend", GetClampedNormal(turningBlend));
                    _animator.SetFloat("MovementBlend", towardAngle > 90 ? _minNormalizedValue : GetClampedNormal(movementBlend));
                    
                    _navMeshAgent.speed = towardAngle > 90 ? _minNavMeshSpeed : GetClampedNormal(movementBlend) * _baseMeshSpeed;
//
//                    DebugInfo.Log(GetClampedNormal(turningBlend) + "  " + GetClampedNormal(movementBlend));
//                    DebugInfo.Log(_baseMeshSpeed);
                }

            if (_attacking)
                _transform.LookAt(_lastEnemy);
        }

        private float GetClampedNormal(float value)
        {
            if (value > 0.99f)
                return 1f;
            else if (value < _minNormalizedValue)
                return _minNormalizedValue;
            else 
                return value;
        }

        private float GetYAngle(Vector3 from, Vector3 to)
        {
            Vector3 differenceBetweenPositions = from - to;

            return Vector3.Angle(differenceBetweenPositions, _transform.forward);
        }
    }
}

using System.Collections;
using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Mono;
using Assets.GamePrimal.TextDamage;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using Assets.TeamProjects.GamePrimal.SeparateComponents.InterfaceHold;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses
{
    public delegate void HitReaction(AttackCaptureParams acp);
    public delegate void HitReactionFinished(AttackCaptureParams acp);

    public class DamageLogger : MonoBehaviour, IHitDetectedHandler, IHitEndedHandler
    {
        public event HitReaction ReactOnHit;
        public event HitReaction AttackStarted;
        public event HitReactionFinished HitEndReached;
        public EventHitDetected EHitDetected = new EventHitDetected();
        public EventHitFinished EHitFinished = new EventHitFinished();

        //        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
//        private bool _isBlip = false;
//        private MainScene.MainScene _manMainScene;
        private ControllerEvent _ce;
        private Transform lastEnemy;
        private Transform lastAlly;
        private MonoAmplifierRpg _amplifier;
        private ControllerAttackCapture _cAttackCapture;
        private CapsuleCollider _capsuleCollider;
        private NavMeshAgent _navMeshAgent;
        private bool _attacking = false;
        private readonly int _autoAttackCost = 2;

        public void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
//            _manMainScene = Object.FindObjectOfType<MainScene.MainScene>();
            _ce = ControllerRouter.GetControllerEvent();
            _amplifier = GetComponent<MonoAmplifierRpg>();
            _cAttackCapture = ControllerRouter.GetControllerAttackCapture();

            ControllerFloatingText.Initialize();
        }

        private void ApplyDamage(Transform ally, Transform enemy)
        {
            if (ally.GetInstanceID() != transform.GetInstanceID()) return;

            int damageAmount = enemy.GetComponent<MonoAmplifierRpg>().CalcDamage();

            _amplifier.SubtractHealth(damageAmount);
//            _animator.SetTrigger(_amplifier.HasDied() ? "Died" : "Hit");
            ReactOnHit?.Invoke(new AttackCaptureParams() { Source = enemy, Target = ally, HasDied = _amplifier.HasDied() });
            ControllerFloatingText.CreateFloatingText(damageAmount.ToString(), transform);

            if (_amplifier.HasDied())
                _capsuleCollider.enabled = false;
        }

//        public void ImpactStage()
//        {
//            HitApply();
//        }

        public void OnEnable()
        {
            ControllerEvent.HitDetectedHandler += AttackTheEnemy;
            ControllerEvent.HitAppliedHandler += ApplyDamage;
        }

        public void OnDisable()
        {
            ControllerEvent.HitDetectedHandler -= AttackTheEnemy;
            ControllerEvent.HitAppliedHandler -= ApplyDamage;
        }

        public void HitDetectedHandler(AnimationEvent ae) => _ce.HitAppliedHandlerInvoke(lastEnemy, lastAlly);

        public void AttackTheEnemy(AttackCaptureParams acp)
        {
            if (acp.Target != transform || acp.Target == acp.Source || acp.HasHit) return;
            
            Transform enemy = acp.Source;
            Transform ally = acp.Target;

            if (Vector3.Distance(enemy.position, transform.position) < _amplifier.GetMeleeRange() && _amplifier.CanAct(_autoAttackCost))
            {
                _cAttackCapture.LockTarget();
//                _animator.StopPlayback();
//                _animator.SetTrigger("Attacking");
//                transform.LookAt(enemy);

//                _attacking = true;
                lastAlly = ally;
                lastEnemy = enemy;

                AttackStarted?.Invoke(acp);
//                Invoke(nameof(HitApply), 1f);
//                Invoke(nameof(HitEndedHandler), 2);

                EHitDetected.Invoke(new HitDetectedParams() {HasDied = acp.HasDied, HasHit = acp.HasHit, Source = acp.Source, Target = acp.Target});
            }
        }

        public void HitEndedHandler(AnimationEvent ae)
        {
//            DebugInfo.Log("Release lock");
            _cAttackCapture.ReleaseFixated();
            EHitFinished.Invoke(new EventParamsBase());

//            _attacking = false;
        }

//        public void HitApply()
//        {
//            _ce.HitAppliedHandlerInvoke(lastEnemy, lastAlly);
//        }

        // Update is called once per frame
        public void Update()
        {
//            if (_attacking)
//                gameObject.transform.LookAt(lastEnemy);

//            if (!_isBlip && _navMeshAgent.hasPath)
//            {
//                _isBlip = true;
//                _animator.SetBool("IsStoped", false);
//            }
//            else if (_isBlip && !_navMeshAgent.hasPath)
//            {
//                _isBlip = false;
//                _animator.SetBool("IsStoped", true);
//            }

//            if (Input.GetKeyDown(KeyCode.H))
//                if (_manMainScene.GetFocus().gameObject.GetInstanceID() == gameObject.GetInstanceID())
//                    _animator.SetTrigger("Attacking");
        }
    }
}

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
        private MonoMechanicus _monomech;

        public void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _ce = StaticProxyRouter.GetControllerEvent();
            _amplifier = GetComponent<MonoAmplifierRpg>();
            _cAttackCapture = StaticProxyRouter.GetControllerAttackCapture();
            _monomech = GetComponent<MonoMechanicus>();

            ControllerFloatingText.Initialize();
        }

        private void ApplyDamage(Transform ally, Transform enemy)
        {
            if (ally.GetInstanceID() != transform.GetInstanceID()) return;

            int damageAmount = enemy.GetComponent<MonoAmplifierRpg>().CalcDamage();

            _amplifier.SubtractHealth(damageAmount);
            ReactOnHit?.Invoke(new AttackCaptureParams() { Source = enemy, Target = ally, HasDied = _amplifier.HasDied() });
            ControllerFloatingText.CreateFloatingText(damageAmount.ToString(), transform);

            if (_amplifier.HasDied())
                _capsuleCollider.enabled = false;
        }

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

            Debug.Log("AttackTheEnemy " + acp.Source.gameObject.name + " " + acp.Target.gameObject.name + " " + acp.HasHit);
            Debug.Log("_autoAttackCost " + _amplifier.CanAct(_autoAttackCost));
            Debug.Log("GetMeleeRange " + Vector3.Distance(enemy.position, transform.position) + " " + _amplifier.GetMeleeRange() + " " + (Vector3.Distance(enemy.position, transform.position) < _amplifier.GetMeleeRange()));
            if (Vector3.Distance(enemy.position, transform.position) < _amplifier.GetMeleeRange() && _amplifier.CanAct(_autoAttackCost))
            {
                _cAttackCapture.LockTarget();

                lastAlly = ally;
                lastEnemy = enemy;

                AttackStarted?.Invoke(acp);

                _monomech.EHitDetected.Invoke(new HitDetectedParams() {HasDied = acp.HasDied, HasHit = acp.HasHit, Source = acp.Source, Target = acp.Target});
            }
        }

        public void HitEndedHandler(AnimationEvent ae)
        {
            _cAttackCapture.ReleaseFixated();
            EHitFinished.Invoke(new EventParamsBase());

        }
    }
}

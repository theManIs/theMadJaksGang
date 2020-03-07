using System.Collections;
using System.IO;
using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Navigation.HighlightFrame;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using Assets.TeamProjects.GamePrimal.SeparateComponents.HudPack.Scripts;
using Assets.TeamProjects.GamePrimal.SeparateComponents.InterfaceHold;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GamePrimal.Mono
{
    [RequireComponent(typeof(MonoAmplifierRpg))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ClickToMove))]
    public class MonoMechanicus : MonoBehaviourBaseClass, IHitEndedHandler
    {
        public bool _iAmMoving = false;
        public bool InfiniteMoving = false;
        public bool InfiniteAction = false;
        public bool AiImproved = false;
        public bool IsBlueTeam = false;

        private ControllerDrumSpinner _cDrumSpinner;
        private CharacterAnimator _characterAnimator;
        private DamageLogger _damageLogger;
        private Rigidbody _rb;
        private MeshRenderer _mr;
        private NavMeshAgent _navMeshAgent;

        private readonly int _autoAttackCost = 2;
        private float _meshWidth;

        public MonoAmplifierRpg _monoAmplifierRpg;
        public EventHitDetected EHitDetected = new EventHitDetected();
        public IArtificial Ai = new AiFrameBuilderNullObject();
        private AbstractHighlight _teamHighlight;

        public void HitDetectedHandler(AnimationEvent ae) => _characterAnimator.HitDetectedHandler();

        public void SetActiveAbility(string abilityName) => _monoAmplifierRpg.SetActiveAbility(abilityName);

        public void ResetCurrentAbility() => _monoAmplifierRpg.ResetActiveAbility();

        private void Awake()
        {
            _cDrumSpinner = StaticProxyRouter.GetControllerDrumSpinner();
            _characterAnimator = new CharacterAnimator();
            _damageLogger = gameObject.AddComponent<DamageLogger>();
            _mr = GetComponent<MeshRenderer>();
            _navMeshAgent = GetComponent<NavMeshAgent>();

            _rb = AddAndGetRigidbody(transform);
            _rb.isKinematic = true;
            _rb.constraints = RigidbodyConstraints.FreezePositionX;

            _monoAmplifierRpg = GetComponent<MonoAmplifierRpg>();
//            _hudViwer = new HudViewer();
//            Debug.Log(_monoAmplifierRpg.MeshSpeed);
            _characterAnimator.UserAwake(new AwakeParams()
            {
                AnimatorComponent = GetComponent<Animator>(), 
                DamageLoggerComponent = GetComponent<DamageLogger>(), 
                NavMeshAgentComponent = GetComponent<NavMeshAgent>(),
//                MeshSpeed = _monoAmplifierRpg.MeshSpeed,
//                WieldingWeapon = _monoAmplifierRpg.WieldingWeapon

            });

//            _hudViwer.UserAwake(new AwakeParams());

        }

        // Start is called before the first frame update
        void Start()
        {
            _meshWidth = _navMeshAgent.radius;

            if (_monoAmplifierRpg.WieldingWeapon)
                _characterAnimator.UserStart(new StartParams()
                {
                    WeaponType = _monoAmplifierRpg.WieldingWeapon.WeaponType,
                    NavMeshSpeed = _monoAmplifierRpg.MeshSpeed
                });

            _teamHighlight = HighlightBuilder.CreateWithParent(transform).GetRedBlueHighlight(IsBlueTeam);
        }

        void Update()
        {
            if (!enabled) return;

            _characterAnimator.UserUpdate(new UpdateParams());

            Ai.DoAny();

            if (Ai.CanNotDoAnyAction())
            {
                StaticProxyEvent.EEndOfRound.Invoke(new EventEndOfRoundParams() {Monomech = this});
                Ai.ClearControlAndTurnEnded();
            }
            
            _teamHighlight.FixedUpdate(transform);
        }

        #region Subscribers
        private void OnEnable()
        {
            _characterAnimator.UserEnable();

            EHitDetected.HitDetectedEvent += HitCapturedHandler;
            StaticProxyEvent.EActiveAbilityChanged.Event += ChangeActiveAbility;
            StaticProxyEvent.ETurnWasFound.Event += TurnWasFoundHandler;
        }

        private void OnDisable()
        {
            _characterAnimator.UserDisable();

            EHitDetected.HitDetectedEvent -= HitCapturedHandler;
            StaticProxyEvent.EActiveAbilityChanged.Event -= ChangeActiveAbility;
            StaticProxyEvent.ETurnWasFound.Event -= TurnWasFoundHandler;
        }

        #endregion

        private void TurnWasFoundHandler(EventTurnWasFoundParams acp)
        {
            if (acp.TurnApplicant != transform)
                return;

            Ai = new AiFrameBuilder(new AiFrameParams()
            {
                Enabled = AiImproved,
                CurrentTransform = transform,
                Monomech = this,
                ActionPoints = _monoAmplifierRpg.GetTurnPoints(),
                MovementSpeed = _monoAmplifierRpg.MoveSpeed,
                AutoAttackCost = _autoAttackCost,
                FightDistance = _monoAmplifierRpg.WieldingWeapon ? _monoAmplifierRpg.WieldingWeapon.WeaponRange : 0,
                MeshError = _meshWidth,
                Nma = _navMeshAgent,
                GetTurnPointsDelegate = _monoAmplifierRpg.GetTurnPoints,
                StartCoroutine = StartCoroutine
            });
            
            Ai.StartAssault();
        }

        private void ChangeActiveAbility(EventActiveAbilityChangedParams acp)
        {
            Transform hardFoucus = StaticProxyRouter.GetControllerFocusSubject().GetHardFocus();

            if (!hardFoucus) return;
            if (hardFoucus && hardFoucus.GetInstanceID() != transform.GetInstanceID()) return;

            if (!acp.AbilityInLockMode)
            {
                _monoAmplifierRpg.ResetActiveAbility();
            }
            else
            {
                _monoAmplifierRpg.SetActiveAbility(acp.AbilityName);
                _characterAnimator.SpawnSpecialProjectile(_monoAmplifierRpg.GetActualAbility());
            }
        }

        private void HitCapturedHandler(EventParamsBase epb)
        {
            if (!InfiniteAction)
                _monoAmplifierRpg.TurnPoints -= _autoAttackCost;
        }

        public void HitEndedHandler(AnimationEvent ae)
        {
            _monoAmplifierRpg.ResetActiveAbility();
        }

        public void OnDrawGizmos()
        {
            if (_navMeshAgent != null && _navMeshAgent.hasPath)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_navMeshAgent.destination, 1);
            }
        }

        private void OnDestroy() => _teamHighlight?.FixedUpdate(null);
    }
}

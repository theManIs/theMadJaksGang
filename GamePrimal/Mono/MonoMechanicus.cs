using System.IO;
using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using Assets.TeamProjects.GamePrimal.SeparateComponents.HudPack.Scripts;
using Assets.TeamProjects.GamePrimal.SeparateComponents.InterfaceHold;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
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

        private ControllerDrumSpinner _cDrumSpinner;
        private CharacterAnimator _characterAnimator;
        private DamageLogger _damageLogger;
        private Rigidbody _rb;
        public MonoAmplifierRpg _monoAmplifierRpg;
        public EventHitDetected EHitDetected = new EventHitDetected();

        public void HitDetectedHandler(AnimationEvent ae) => _characterAnimator.HitDetectedHandler();

        public void SetActiveAbility(string abilityName) => _monoAmplifierRpg.SetActiveAbility(abilityName);

        public void ResetCurrentAbility() => _monoAmplifierRpg.ResetActiveAbility();

        private void Awake()
        {
            _cDrumSpinner = StaticProxyRouter.GetControllerDrumSpinner();
            _characterAnimator = new CharacterAnimator();
            _damageLogger = gameObject.AddComponent<DamageLogger>();

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
            if (_monoAmplifierRpg.WieldingWeapon)
                _characterAnimator.UserStart(new StartParams()
                {
                    WeaponType = _monoAmplifierRpg.WieldingWeapon.WeaponType,
                    NavMeshSpeed = _monoAmplifierRpg.MeshSpeed
                });
        }

        // Update is called once per frame
        void Update()
        {
            if (!enabled) return;

            bool doIReallyMove = _cDrumSpinner.DoIMove(transform);
//            Debug.Log(doIReallyMove + " " + nameof(_cDrumSpinner));
            if (doIReallyMove)
            {
//                Debug.Log(Time.time + " " + gameObject.name + " " + GetComponent<MonoAmplifierRpg>().GetInitiative());

                _iAmMoving = true;
            }
            
            if (_iAmMoving)
                if (Input.GetKeyDown(KeyCode.Space))
                    if (_cDrumSpinner.ReleaseRound())
                        _iAmMoving = false;


            _characterAnimator.UserUpdate(new UpdateParams());

//            _hudViwer.UserUpdate(new UpdateParams() {ActualInvoker = transform, AmplifierRpg = _monoAmplifierRpg});
        }

        private void OnEnable()
        {
            _characterAnimator.UserEnable();

            EHitDetected.HitDetectedEvent += HitCapturedHandler;
            StaticProxyEvent.EActiveAbilityChanged.Event += ChangeActiveAbility;
        }

        private void OnDisable()
        {
            _characterAnimator.UserDisable();

            EHitDetected.HitDetectedEvent -= HitCapturedHandler;
            StaticProxyEvent.EActiveAbilityChanged.Event -= ChangeActiveAbility;
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
                _monoAmplifierRpg.TurnPoints -= 2;
        }

        public void HitEndedHandler(AnimationEvent ae)
        {
            _monoAmplifierRpg.ResetActiveAbility();
        }
    }
}

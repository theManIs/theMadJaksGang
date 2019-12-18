using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GamePrimal.Mono
{
    [RequireComponent(typeof(MonoAmplifierRpg))]
    public class MonoMechanicus : MonoBehaviour
    {
        public bool _iAmMoving = false;

        private ControllerDrumSpinner _cDrumSpinner;
        private CharacterAnimator _dl;
        private DamageLogger _damageLogger;
        private Rigidbody _rb;

        private void Awake()
        {
            _cDrumSpinner = ControllerRouter.GetControllerDrumSpinner();
            _dl = new CharacterAnimator();
            _damageLogger = gameObject.AddComponent<DamageLogger>();
            _rb = gameObject.AddComponent<Rigidbody>();
            _rb.isKinematic = true;
            _rb.constraints = RigidbodyConstraints.FreezePositionX;

            _dl.UserAwake(new AwakeParams()
            {
                AnimatorComponent = GetComponent<Animator>(), 
                DamageLoggerComponent = GetComponent<DamageLogger>(), 
                NavMeshAgentComponent = GetComponent<NavMeshAgent>(),
                MeshSpeed = GetComponent<MonoAmplifierRpg>().MeshSpeed
            });

        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (!enabled) return;

            bool doIReallyMove = _cDrumSpinner.DoIMove(transform);

            if (doIReallyMove)
            {
                Debug.Log(Time.time + " " + gameObject.name + " " + GetComponent<MonoAmplifierRpg>().GetInitiative());

                _iAmMoving = true;
            }
            
            if (_iAmMoving)
                if (Input.GetKeyDown(KeyCode.Space))
                    if (_cDrumSpinner.ReleaseRound())
                        _iAmMoving = false;


            _dl.UserUpdate();
        }

        private void OnEnable()
        {
            _dl.UserEnable();
        }

        private void OnDisable()
        {
            _dl.UserDisable();
        }
    }
}

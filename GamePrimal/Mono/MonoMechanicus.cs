using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GamePrimal.Mono
{
    [RequireComponent(typeof(MonoAmplifierRpg))]
    public class MonoMechanicus : MonoBehaviour
    {
        private ControllerDrumSpinner _cDrumSpinner;
        public bool _iAmMoving = false;
        private CharacterAnimator _dl;
        private DamageLogger _damageLogger;

        private void Awake()
        {
            _cDrumSpinner = ControllerRouter.GetControllerDrumSpinner();
            _dl = new CharacterAnimator();
            _damageLogger = gameObject.AddComponent<DamageLogger>();

            _dl.SetAnimatorComponent(GetComponent<Animator>());
            _dl.SetDamageLogger(GetComponent<DamageLogger>());
            _dl.SetNavAgent(GetComponent<NavMeshAgent>());

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

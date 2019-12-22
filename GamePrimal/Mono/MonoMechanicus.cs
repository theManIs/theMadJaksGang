﻿using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.HudPack.Scripts;
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
        private CharacterAnimator _characterAnimator;
        private DamageLogger _damageLogger;
        private Rigidbody _rb;
        private MonoAmplifierRpg _monoAmplifierRpg;
        private HudViewer _hudViwer;

        private void Awake()
        {
            _cDrumSpinner = ControllerRouter.GetControllerDrumSpinner();
            _characterAnimator = new CharacterAnimator();
            _damageLogger = gameObject.AddComponent<DamageLogger>();
            _rb = gameObject.AddComponent<Rigidbody>();
            _rb.isKinematic = true;
            _rb.constraints = RigidbodyConstraints.FreezePositionX;
            _monoAmplifierRpg = GetComponent<MonoAmplifierRpg>();
            _hudViwer = new HudViewer();
//            Debug.Log(_monoAmplifierRpg.MeshSpeed);
            _characterAnimator.UserAwake(new AwakeParams()
            {
                AnimatorComponent = GetComponent<Animator>(), 
                DamageLoggerComponent = GetComponent<DamageLogger>(), 
                NavMeshAgentComponent = GetComponent<NavMeshAgent>(),
//                MeshSpeed = _monoAmplifierRpg.MeshSpeed,
                WieldingWeapon = _monoAmplifierRpg.WieldingWeapon

            });

            _hudViwer.UserAwake(new AwakeParams());
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

            if (doIReallyMove)
            {
//                Debug.Log(Time.time + " " + gameObject.name + " " + GetComponent<MonoAmplifierRpg>().GetInitiative());

                _iAmMoving = true;
            }
            
            if (_iAmMoving)
                if (Input.GetKeyDown(KeyCode.Space))
                    if (_cDrumSpinner.ReleaseRound())
                        _iAmMoving = false;


            _characterAnimator.UserUpdate();

            _hudViwer.UserUpdate();
            _hudViwer.ShowTurnPoints(_monoAmplifierRpg.GetTurnPoints(), transform);
        }

        private void OnEnable()
        {
            _characterAnimator.UserEnable();
        }

        private void OnDisable()
        {
            _characterAnimator.UserDisable();
        }
    }
}

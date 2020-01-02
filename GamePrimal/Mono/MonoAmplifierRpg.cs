using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.DemoAnimationScene.MiscellaneousWeapons.CommonScripts;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using UnityEngine;
using UnityScript.Steps;
using static Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff.ResourcesList;

namespace Assets.GamePrimal.Mono
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class MonoAmplifierRpg : MonoBehaviourBaseClass
    {
        [SerializeField] private int Health;
        [SerializeField] private int Damage;
        [SerializeField] private int Initiative;
        [SerializeField] private float WeaponRange;
        [SerializeField] private CharacterFeatures CharacterFeatures;
        [SerializeField] private int MaxTurnPoints;
        [SerializeField] private int TurnPoints;
        [SerializeField] private int InitialTurnPoints;
        [SerializeField] private Sprite CharacterPortrait;
        public int MaxHealth;
        public int ExperienceActual;
        public int ExperienceMax;
        public Transform WeaponProjectile;
        public int MoveSpeed;

        private DamageLogger _damageLogger;

        public float MeshSpeed { get; private set; } = 4f;
        public WeaponOperator WieldingWeapon { get; private set; }
        public WeaponPosition _weaponPoint { get; private set; }

        public void SubtractHealth(int amount) => Health -= amount;
        public int CalcDamage() => Damage;
        public int ViewHealth() => Health;
        public bool HasDied() => Health <= 0;
        public int GetInitiative() => Initiative;
        public float GetMeleeRange() => WeaponRange;
        public int GetTurnPoints() => TurnPoints;
        public Sprite GetCharacterPortrait() => CharacterPortrait;

        private void Awake()
        {
            _damageLogger = GetComponent<DamageLogger>();

            Unpack(CharacterFeatures);
        }

        private void OnEnable()
        {
            _damageLogger.EHitDetected.HitDetectedEvent += HitCapturedHandler;
            ControllerRouter.GetControllerDrumSpinner().ETurnWasFound.Event += TurnWasFoundHandler;
        }

        private void OnDisable()
        {
            _damageLogger.EHitDetected.HitDetectedEvent += HitCapturedHandler;
            ControllerRouter.GetControllerDrumSpinner().ETurnWasFound.Event -= TurnWasFoundHandler;
        }

        private void TurnWasFoundHandler(EventTurnWasFoundParams acp)
        {
            TurnPoints = InitialTurnPoints;
        }

        private void HitCapturedHandler(EventParamsBase epb)
        {
            TurnPoints -= 2;
        }

        public bool CanAct(int actionCost)
        {
            return TurnPoints - actionCost >= 0;
        }

        public void SubtractActionCost(int actionCost) => TurnPoints = TurnPoints - actionCost;

        private void Unpack(CharacterFeatures af)
        {
            if (CharacterFeatures)
            {
                WieldingWeapon = af.StartWeapon;
                Health = af.StartHealth;
                MaxHealth = af.MaxHealth;
                Initiative = af.Initiative;
                Damage = af.Damage;
                MoveSpeed = af.MoveSpeed;
                TurnPoints = af.TurnPoints;
                InitialTurnPoints = af.TurnPoints;
                MaxTurnPoints = af.MaxTurnPoints;
                CharacterPortrait = af.CharacterPortrait;
                ExperienceActual = (int)(Random.value * 10);
                ExperienceMax = (int)(Random.value * 10) + ExperienceActual;
                WeaponProjectile = af.PreferredProjectile ? af.PreferredProjectile : 
                    Resources.Load<GameObject>(ArrowProjectile)?.transform;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Initiative = (int)(Random.value * 10);
            Health = Health != 0 ? Health : (int)(Random.value * 200);
            Damage = Damage != 0 ? Damage : (int)(Random.value * 100);

            FindWeaponPosition();
            FillWieldingWeapon();
        }

        private void FillWieldingWeapon()
        {
            if (_weaponPoint && WieldingWeapon)
            {
                WieldingWeapon = Instantiate(WieldingWeapon, _weaponPoint.transform);
                WeaponRange = WieldingWeapon.WeaponRange;
            }
        }

        private void FindWeaponPosition()
        {
            Transform targetElement = FindInChildren<WeaponPosition>(transform);

            if (targetElement)
                _weaponPoint = targetElement.GetComponent<WeaponPosition>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

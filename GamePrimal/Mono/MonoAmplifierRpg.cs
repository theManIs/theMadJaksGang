using Assets.TeamProjects.DemoAnimationScene.MiscellaneousWeapons.CommonScripts;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using UnityEngine;
using UnityScript.Steps;

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
        [SerializeField] private int MaxHealth;
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

        private void Awake()
        {
            _damageLogger = GetComponent<DamageLogger>();

            Unpack(CharacterFeatures);
        }

        private void OnEnable()
        {
            _damageLogger.EHitDetected.HitDetectedEvent += HitCapturedHandler;
        }

        private void OnDisable()
        {
            _damageLogger.EHitDetected.HitDetectedEvent += HitCapturedHandler;
        }

        private void HitCapturedHandler(AttackCaptureParams acp)
        {
            TurnPoints -= 2;
        }

        public bool CanAct(int actionCost)
        {
            return TurnPoints - actionCost >= 0;
        }

        private void Unpack(CharacterFeatures af)
        {
            if (CharacterFeatures)
            {
                WieldingWeapon = af.StartWeapon;
                Health = af.StartHealth;
                MaxHealth = af.MaxHealth;
                Initiative = af.Initiative;
                Damage = af.Damage;
                TurnPoints = af.TurnPoints;
                MaxTurnPoints = af.MaxTurnPoints;
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

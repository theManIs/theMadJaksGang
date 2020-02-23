using Assets.TeamProjects.GamePrimal.Helpers;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.AbilitiesTree;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using Assets.TeamProjects.GamePrimal.SeparateComponents.WeaponOrigins;
using UnityEngine;

namespace Assets.GamePrimal.Mono
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class MonoAmplifierRpg : MonoBehaviourBaseClass
    {
        [SerializeField] private int Health;
        public int MaxHealth;
        [SerializeField] private int Damage;
        [SerializeField] private int Initiative;
        [SerializeField] private float WeaponRange;
        [SerializeField] private CharacterFeatures CharacterFeatures;
        [SerializeField] private int InitialTurnPoints;
        [SerializeField] private Sprite CharacterPortrait;
        public int MaxTurnPoints;
        public int TurnPoints;
        public int ExperienceActual;
        public int ExperienceMax;
        public Transform WeaponProjectile;
        public int MoveSpeed;
        public string AutoAttack;
        private AbstractWeaponBased _autoAttack; //todo Shadow field because of string value AutoAttack
        public string[] AbilitiesSet;
        public string AbilityToUse;   //todo Make this field typed as its purpose
        private AbstractAbility _abilityToUse;   //todo Shadow field because of string value AbilityToUse
        public AbstractAbility GetActualAbility() => _abilityToUse;

        private DamageLogger _damageLogger;

        public float MeshSpeed { get; private set; } = 4f;
        public WeaponOperatorAbstract WieldingWeapon;
        public WeaponPosition _weaponPoint { get; private set; }

        public void SubtractHealth(int amount) => Health -= amount;
        public int CalcDamage() => Damage;
        public int ViewHealth() => Health;
        public bool HasDied() => Health <= 0;
        public int GetInitiative() => Initiative;
        public float GetMeleeRange() => WeaponRange;
        public int GetTurnPoints() => TurnPoints;
        public Sprite GetCharacterPortrait() => CharacterPortrait;

        public void SetActiveAbility(string abilityToUseActual)
        {
            bool localAbilityWasSet = false;

            foreach (string s in AbilitiesSet)  //todo This has to be type AbstractAbility
            {
                Debug.Log(s + " == " + abilityToUseActual + " = " + s.Equals(abilityToUseActual) + " " + Time.time);
                if (s.Equals(abilityToUseActual))
                {
                    localAbilityWasSet = true;
                    SetAnyAbility(abilityToUseActual);
                }
            }

            if (!localAbilityWasSet)
                SetAnyAbility(AutoAttack);
        }

        public void ResetActiveAbility() => SetAnyAbility(AutoAttack);

        private void SetAnyAbility(string abilityName)
        {
            AbilityToUse = abilityName;

//            Debug.Log(abilityName + " " + AbilitiesList.PoisonArrow + " " + Time.time);
            if (abilityName == AbilitiesList.PoisonArrow) //todo move Abilities list to monomech
            {
                _abilityToUse = new PoisonArrow();
//                Debug.Log(_abilityToUse + " " + Time.time);
            } 
            else if (abilityName == AbilitiesList.IceShard)
            {
                _abilityToUse = new IceShard();
            }
            else
                _abilityToUse = _autoAttack;

            Debug.Log(_abilityToUse + " " + Time.time);
        }

        private void SetRangedAutoAttack()
        {
            AutoAttack = AbilitiesList.AutoAttackRanged;
            _autoAttack = new AutoAttackRanged();
        }

        private void SetMeleeAutoAttack()
        {
            AutoAttack = AbilitiesList.AutoAttackMelee;
            _autoAttack = new AutoAttackMelee();
        }

        private void Awake()
        {
            _damageLogger = GetComponent<DamageLogger>();

            Unpack(CharacterFeatures);
        }

        private void OnEnable()
        {
//            _damageLogger.EHitDetected.HitDetectedEvent += HitCapturedHandler;
            StaticProxyEvent.ETurnWasFound.Event += TurnWasFoundHandler;
        }


        private void OnDisable()
        {
//            _damageLogger.EHitDetected.HitDetectedEvent += HitCapturedHandler;
            StaticProxyEvent.ETurnWasFound.Event -= TurnWasFoundHandler;
        }

        private void TurnWasFoundHandler(EventTurnWasFoundParams acp)
        {
            TurnPoints = InitialTurnPoints;
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
                WieldingWeapon = af.StartWeapon ? (WeaponOperatorAbstract) af.StartWeapon : new ZeroWeapon();
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
                AbilitiesSet = af.AbilitiesSet;
//                WeaponProjectile = af.PreferredProjectile ? af.PreferredProjectile : 
//                    Resources.Load<GameObject>(ArrowProjectile)?.transform;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Initiative = (int)(Random.value * 10);
            MaxHealth = Health = Health != 0 ? Health : (int)(Random.value * 200);
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
                WeaponProjectile = WieldingWeapon.DefaultProjectile;
//
//                if (WeaponProjectile)
//                    WieldingWeapon.SpawnProjectile(WeaponProjectile);

                if (WeaponProjectile && WieldingWeapon.isRanged) 
                    SetRangedAutoAttack();
                else
                    SetMeleeAutoAttack();

            }
        }

        private void FindWeaponPosition()
        {
            Transform targetElement = FindInChildren<WeaponPosition>(transform);

            if (targetElement)
                _weaponPoint = targetElement.GetComponent<WeaponPosition>();
        }
    }
}

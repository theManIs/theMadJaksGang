using Assets.TeamProjects.DemoAnimationScene.MiscellaneousWeapons.CommonScripts;
using Assets.TeamProjects.GamePrimal.Mono;
using UnityEngine;

namespace Assets.GamePrimal.Mono
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class MonoAmplifierRpg : MonoBehaviourBaseClass
    {
        [SerializeField] private int Health = 0;
        [SerializeField] private int Damage = 0;
        [SerializeField] private int Initiative = 0;
        [SerializeField] private float WeaponRange = 4;

        public WeaponOperator WieldingWeapon;
        public float MeshSpeed = 1.5f;

        private WeaponPosition _weaponPoint;

        public void SubtractHealth(int amount) => Health -= amount;
        public int CalcDamage() => Damage;
        public int ViewHealth() => Health;
        public bool HasDied() => Health <= 0;
        public int GetInitiative() => Initiative;
        public float GetMeleeRange() => WeaponRange;

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

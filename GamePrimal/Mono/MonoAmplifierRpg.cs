using UnityEngine;

namespace Assets.GamePrimal.Mono
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class MonoAmplifierRpg : MonoBehaviour
    {
        [SerializeField] private int Health = 0;
        [SerializeField] private int Damage = 0;
        [SerializeField] private int Initiative = 0;
        [SerializeField] private float MeleeRange = 0;

        public void SubtractHealth(int amount) => Health -= amount;
        public int CalcDamage() => Damage;
        public int ViewHealth() => Health;
        public bool HasDied() => Health <= 0;
        public int GetInitiative() => Initiative;
        public float GetMeleeRange() => MeleeRange;

        // Start is called before the first frame update
        void Start()
        {
            Initiative = (int)(Random.value * 10);
            Health = Health != 0 ? Health : (int)(Random.value * 200);
            Damage = Damage != 0 ? Damage : (int)(Random.value * 100);
            MeleeRange = 4;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

﻿using UnityEngine;

namespace Assets.GamePrimal.Mono
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class MonoAmplifierRpg : MonoBehaviour
    {
        [SerializeField] private int Health;
        [SerializeField] private int Damage;
        [SerializeField] private int Initiative;
        [SerializeField] private float MeleeRange = 2;

        public void SubtractHealth(int amount) => Health -= amount;
        public int CalcDamage() => Damage;
        public int ViewHealth() => Health;
        public bool HasDied() => Health <= 0;
        public int GetInitiative() => Initiative;
        public float GetMeleeRange() => MeleeRange;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

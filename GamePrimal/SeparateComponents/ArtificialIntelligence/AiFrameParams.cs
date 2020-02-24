using System;
using System.Collections;
using Assets.GamePrimal.Mono;
using UnityEngine;
using UnityEngine.AI;


namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public struct AiFrameParams
    {
        #region Fields
        public bool Enabled;
        public Transform CurrentTransform;
        public MonoMechanicus Monomech;
        public int AutoAttackCost;
        public float FightDistance;
        public float MeshError;
        public int ActionPoints;
        public int MovementSpeed;
        public NavMeshAgent Nma;
        public Func<int> GetTurnPointsDelegate;
        public Func<IEnumerator, Coroutine> StartCoroutine; 
        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.AbstractSources;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using Assets.TeamProjects.GamePrimal.SeparateComponents.UserMath;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public struct AiFrameParams
    {
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
    }

    public class AiFrame : EnableIncluded, IArtificial
    {
        public AiFrameParams Attr;

        private MonoMechanicus _pickedEnemy;
        private bool _returnControl = false;

        private MonoMechanicus[] GetAllMonomechs() => StaticProxyObjectFinder.FindObjectOfType<MonoMechanicus>();
        private MonoMechanicus PickFirstEnemyToAttack() => FilterWithinAttack().Count > 0 ? FilterWithinAttack().First().Value : null;
        private void PickEnemyForThisTurn() => _pickedEnemy = _pickedEnemy ?? PickFirstEnemyToAttack();
        private void ReleaseEnemyForThisTurn() => _pickedEnemy = null;
        private MonoMechanicus[] GetEnemies() => GetAllMonomechs()
            .Where(c => c.BlueRedTeam != Attr.Monomech.BlueRedTeam).ToArray();

        private SortedDictionary<float, MonoMechanicus> ResolveEnemies() =>
            GetEnemies().Aggregate(new SortedDictionary<float, MonoMechanicus>(), (carrier, monomech) =>
            {
                carrier.Add(Vector3.Distance(monomech.transform.position, Attr.Monomech.transform.position), monomech);

                return carrier;
            });

        private SortedDictionary<float, MonoMechanicus> FilterWithinAttack() =>
            ResolveEnemies().Aggregate(new SortedDictionary<float, MonoMechanicus>(), (carrier, monomech) =>
            {
                if (MovementMath.CalcHitRange(Attr.ActionPoints - Attr.AutoAttackCost, Attr.MovementSpeed, Attr.FightDistance) >= monomech.Key)
                    carrier.Add(Vector3.Distance(monomech.Value.transform.position, Attr.Monomech.transform.position), monomech.Value);

                return carrier;
            });

        private Vector3 ConnectVector(MonoMechanicus oppose)
        {
            float realDistance = Vector3.Distance(oppose.transform.position, Attr.Monomech.transform.position);
            float correctNormal = MovementMath.AttackVectorCoercion(realDistance, Attr.FightDistance, Attr.MeshError);

            if (DevelopFlag)
                Debug.Log($"_fightDistance {Attr.FightDistance} _meshError {Attr.MeshError} correctNormal: {correctNormal} realDistance {realDistance} distance {realDistance - correctNormal * realDistance}");

            return Vector3.Lerp(Attr.Monomech.transform.position, oppose.transform.position, correctNormal);
        }

        public AiFrame MoveToTarget()
        {
            if (!PickFirstEnemyToAttack())
                return this;

            Attr.Nma.SetDestination(ConnectVector(PickFirstEnemyToAttack()));

            return this;
        }

        private bool HitTarget()
        {
            PickEnemyForThisTurn();

            if (_pickedEnemy && Vector3.Distance(Attr.Monomech.transform.position, _pickedEnemy.transform.position) > Attr.FightDistance)
                return false;
            else if (_pickedEnemy) 
                DamageLoggerAdapter.AttackCapture(Attr.Monomech, _pickedEnemy);

            ReleaseEnemyForThisTurn();

            return true;
        }

        public IEnumerator HitTargetAsSoonAsPossible()
        {
            while (Attr.Nma.hasPath)
                yield return null;

//            yield return new WaitForSeconds(1);

            while (!HitTarget())
                yield return null;

            _returnControl = true;
        }

        public IEnumerator ConsumeActionPoints()
        {

            Debug.Log("ConsumeActionPoints " + Attr.GetTurnPointsDelegate());
            Attr.StartCoroutine(HitTargetAsSoonAsPossible());

            while (!_returnControl)
                yield return new WaitForSeconds(1);

            Debug.Log("ConsumeActionPoints " + Attr.GetTurnPointsDelegate());
//            Attr.Monomech._monoAmplifierRpg.ca
            yield return null;
        }
    }
}
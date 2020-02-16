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
        private bool _movementLocker = false;
        private bool _isControlBlocked;
        private Vector3 _lastDestination;
        private bool[] _lock = new bool[3];
        private int _visibleRange = 10;

        private MonoMechanicus[] GetAllMonomechs() => StaticProxyObjectFinder.FindObjectOfType<MonoMechanicus>();
        private void PickEnemyForThisTurn()
        {
            _pickedEnemy = _pickedEnemy ?? PickFirstEnemyToAttack();

            if (!_pickedEnemy)
                _pickedEnemy = PickEnemyToClose();
            
            if (!_pickedEnemy)
                _pickedEnemy = PickEnemyToStayCloser();
        }

//        private void PickEnemyToCloseForThisTurn()
//        {
//            _pickedEnemy = _pickedEnemy ?? PickEnemyToClose();
//
//            if (_pickedEnemy)
//                Debug.Log($"PickEnemyToCloseForThisTurn {_pickedEnemy.gameObject.name}");
//        }
//        private void PickEnemyToStayCloseForThisTurn()
//        {
//            _pickedEnemy = _pickedEnemy ?? PickEnemyToStayCloser();
//
//            if (_pickedEnemy)
//                Debug.Log($"PickEnemyToStayCloseForThisTurn {_pickedEnemy.gameObject.name}");
//        }

        private void ReleaseEnemyForThisTurn() => _pickedEnemy = null;
        private MonoMechanicus[] GetEnemies() => GetAllMonomechs()
            .Where(c => c.BlueRedTeam != Attr.Monomech.BlueRedTeam).ToArray();

        private SortedDictionary<float, MonoMechanicus> ResolveEnemies() =>
            GetEnemies().Aggregate(new SortedDictionary<float, MonoMechanicus>(), (carrier, monomech) =>
            {
                carrier.Add(Vector3.Distance(monomech.transform.position, Attr.Monomech.transform.position), monomech);

                return carrier;
            });

//        private SortedDictionary<float, MonoMechanicus> FilterWithinAttack() =>
//            FilterWithinRange(MovementMath.CalcHitRange(Attr.ActionPoints - Attr.AutoAttackCost, Attr.MovementSpeed, Attr.FightDistance));
//            ResolveEnemies().Aggregate(new SortedDictionary<float, MonoMechanicus>(), (carrier, monomech) =>
//            {
//                if (MovementMath.CalcHitRange(Attr.ActionPoints - Attr.AutoAttackCost, Attr.MovementSpeed, Attr.FightDistance) >= monomech.Key)
//                    carrier.Add(Vector3.Distance(monomech.Value.transform.position, Attr.Monomech.transform.position), monomech.Value);
//
//                return carrier;
//            });

        private SortedDictionary<float, MonoMechanicus> FilterWithinRange(float filterRange) =>
            ResolveEnemies().Aggregate(new SortedDictionary<float, MonoMechanicus>(), (carrier, monomech) =>
            {
                if (filterRange >= monomech.Key)
                    carrier.Add(Vector3.Distance(monomech.Value.transform.position, Attr.Monomech.transform.position), monomech.Value);

                return carrier;
            });

        private MonoMechanicus PickFirstEnemyToAttack() =>
            PickOneInRange(MovementMath.CalcHitRange(
                Attr.ActionPoints - Attr.AutoAttackCost, Attr.MovementSpeed, Attr.FightDistance
            ));

        private MonoMechanicus PickEnemyToClose() =>
            PickOneInRange(MovementMath.CalcMovementLength(Attr.ActionPoints, Attr.MovementSpeed));
        //        {
        //            SortedDictionary<float, MonoMechanicus> enemiesToClose = 
        //                FilterWithinRange(MovementMath.CalcMovementLength(Attr.ActionPoints, Attr.MovementSpeed));
        //
        //            return enemiesToClose.Count > 0 ? enemiesToClose.First().Value : null;
        //        }
        private MonoMechanicus PickEnemyToStayCloser() =>
            PickOneInRange(MovementMath.CalcMovementLength(Attr.ActionPoints, Attr.MovementSpeed) * _visibleRange);
//        {
//            SortedDictionary<float, MonoMechanicus> enemiesToClose =
//                FilterWithinRange(MovementMath.CalcMovementLength(Attr.ActionPoints, Attr.MovementSpeed) * _visibleRange);
//
//            return enemiesToClose.Count > 0 ? enemiesToClose.First().Value : null;
//        }

        private MonoMechanicus PickOneInRange(float scanRange)
        {
            SortedDictionary<float, MonoMechanicus> enemiesToClose = FilterWithinRange(scanRange);

            return enemiesToClose.Count > 0 ? enemiesToClose.First().Value : null;
        }

        private Vector3 ConnectVector(MonoMechanicus oppose)
        {
            float realDistance = Vector3.Distance(oppose.transform.position, Attr.Monomech.transform.position);
            float maxMovementRange = MovementMath.CalcMovementLength(Attr.ActionPoints, Attr.MovementSpeed);
            float correctNormal = MovementMath.AttackVectorCoercion
                (realDistance, Attr.FightDistance, Attr.MeshError, maxMovementRange);

           Debug.Log($"_fightDistance {Attr.FightDistance} " +
                     $"_meshError {Attr.MeshError} " +
                     $"correctNormal: {correctNormal} " +
                     $"realDistance {realDistance} " +
                     $"distance {realDistance - correctNormal * realDistance} " +
                     $"maxMovementRange {maxMovementRange} ");

            return Vector3.Lerp(Attr.Monomech.transform.position, oppose.transform.position, correctNormal);
        }

//        public IArtificial MoveToTarget()
//        {
//            if (!PickFirstEnemyToAttack())
//                return this;
//
//            Attr.Nma.SetDestination(ConnectVector(PickFirstEnemyToAttack()));
//
//            return this;
//        }

        private void LockMovement() => _movementLocker = true;
        private bool IsMovementLocked() => _movementLocker;
        private void UnlockMovement() => _movementLocker = false;
        private void UnlockMovementIf() => _movementLocker = _movementLocker && !IsWithingArmLength();

        private void SetDestination()
        {
            if (_pickedEnemy)
            {
                Debug.Log("SetDestination " + Attr.GetTurnPointsDelegate());

                Attr.Nma.SetDestination(ConnectVector(_pickedEnemy));

                LockMovement();
            }
        }

        private bool HitTarget()
        {
            if (_pickedEnemy && Vector3.Distance(Attr.Monomech.transform.position, _pickedEnemy.transform.position) > Attr.FightDistance)
                return false;
            else if (_pickedEnemy) 
                DamageLoggerAdapter.AttackCapture(Attr.Monomech, _pickedEnemy);

            return true;
        }

//        public IEnumerator HitTargetAsSoonAsPossible()
//        {
//            while (Attr.Nma.hasPath)
//                yield return null;
//
////            yield return new WaitForSeconds(1);
//
//            while (!HitTarget())
//                yield return null;
//
//            _returnControl = true;
//        }

//        public IEnumerator ConsumeActionPoints()
//        {
//
//            Debug.Log("ConsumeActionPoints " + Attr.GetTurnPointsDelegate());
//            Attr.StartCoroutine(HitTargetAsSoonAsPossible());
//
//            while (!_returnControl)
//                yield return new WaitForSeconds(1);
//
//            _returnControl = false;
//
//            Attr.StartCoroutine(HitTargetAsSoonAsPossible());
//
//            Debug.Log("ConsumeActionPoints " + Attr.GetTurnPointsDelegate());
////            Attr.Monomech._monoAmplifierRpg.ca
//            yield return null;
//        }

        private bool IsWithingArmLength() =>
            _pickedEnemy && (Attr.CurrentTransform.position - _pickedEnemy.transform.position).sqrMagnitude <= Attr.FightDistance * Attr.FightDistance;
//            _pickedEnemy && Vector3.Distance(Attr.CurrentTransform.position, _pickedEnemy.transform.position) <= Attr.FightDistance;

        public void StartAssault() => _isControlBlocked = true;
        private bool HitIsInProgress => StaticProxyRouter.GetControllerAttackCapture().HasHit;
        //        private void BlockControl() => _controlBlocker = true;
        //        private bool IsControlBlocked() => _controlBlocker;
        //        private void ReleaseControl() => _controlBlocker = false;

        public void DoAny()
        {
//            if (IsControlBlocked())
//                Debug.Log("ConsumeActionPoints " + Attr.GetTurnPointsDelegate());

            InternalDoAnyLogic();

//            if (IsControlBlocked())
//                Debug.Log("ConsumeActionPoints " + Attr.GetTurnPointsDelegate());
        }

        private void InternalDoAnyLogic()
        {
            if (!_isControlBlocked || HitIsInProgress)
                return;

            PickEnemyForThisTurn();
            UnlockMovementIf();
//            if (_pickedEnemy)
//                Debug.Log("IsWithingArmLength " + IsWithingArmLength() + " distance " + Vector3.Distance(Attr.CurrentTransform.position, _pickedEnemy.transform.position) + " " + Attr.FightDistance);

//            Debug.Log($"IsMovementLocked {IsMovementLocked()}");

            if (IsMovementLocked())
                return;
            else if (!IsMovementLocked() && !IsWithingArmLength())
                SetDestination();
            else if (IsWithingArmLength())
                if (HitTarget())
                    ReleaseEnemyForThisTurn();

            if (Attr.GetTurnPointsDelegate() < Attr.AutoAttackCost)
                _isControlBlocked = false;

        }
    }
}
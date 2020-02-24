using System.Collections.Generic;
using System.Linq;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using Assets.TeamProjects.GamePrimal.SeparateComponents.UserMath;
using UnityEngine;


namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public class AiFrame : IArtificial
    {
        #region Fields

        public AiFrameParams Attr;

        private MonoMechanicus _pickedEnemy;

        private bool _movementLocker = false;

        private bool _isControlBlocked;

        private readonly int _visibleRange = 10;

        private bool _hasControlBlocked;

        private bool _controlStateWasReleased;

        private bool _controlAndTurnEnded;

        #endregion


        #region IArtificial

        public void StartAssault() => _isControlBlocked = true;
        public bool CanNotDoAnyAction() => _controlAndTurnEnded;

        public void DoAny()
        {
            if (!Attr.Enabled)
                return;

            ControlChangeStage();
        }

        #endregion


        #region Methods

        private MonoMechanicus[] GetAllMonomechs() => StaticProxyObjectFinder.FindObjectOfType<MonoMechanicus>();
        private void PickEnemyForThisTurn()
        {
            _pickedEnemy = _pickedEnemy ?? PickFirstEnemyToAttack();

            if (!_pickedEnemy)
                _pickedEnemy = PickEnemyToClose();
            
            if (!_pickedEnemy)
                _pickedEnemy = PickEnemyToStayCloser();
        }

        private void ReleaseEnemyForThisTurn() => _pickedEnemy = null;
        private MonoMechanicus[] GetEnemies() => GetAllMonomechs()
            .Where(c => c.BlueRedTeam != Attr.Monomech.BlueRedTeam).ToArray();

        private SortedDictionary<float, MonoMechanicus> ResolveEnemies() =>
            GetEnemies().Aggregate(new SortedDictionary<float, MonoMechanicus>(), (carrier, monomech) =>
            {
                carrier.Add(Vector3.Distance(monomech.transform.position, Attr.Monomech.transform.position), monomech);

                return carrier;
            });

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

        private MonoMechanicus PickEnemyToStayCloser() =>
            PickOneInRange(MovementMath.CalcMovementLength(Attr.ActionPoints, Attr.MovementSpeed) * _visibleRange);

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

            return Vector3.Lerp(Attr.Monomech.transform.position, oppose.transform.position, correctNormal);
        }

        private void SetDestination()
        {
            if (_pickedEnemy)
            {
                Attr.Nma.SetDestination(ConnectVector(_pickedEnemy)); // todo put in some NavMeshAgentWrapper

                _movementLocker = true;
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

        private bool IsWithingArmLength() => _pickedEnemy && 
            (Attr.CurrentTransform.position - _pickedEnemy.transform.position).sqrMagnitude <= Attr.FightDistance * Attr.FightDistance;


        private bool HitIsInProgress => StaticProxyRouter.GetControllerAttackCapture().HasHit;
        private bool MoveInProgress => _movementLocker;

        private void ControlChangeStage()
        {
            _hasControlBlocked = _isControlBlocked;

            ControlLockStage();

            if (_hasControlBlocked != _isControlBlocked)
                _controlStateWasReleased = true;

            if (_controlStateWasReleased && !HitIsInProgress && !MoveInProgress)
            {
                _controlAndTurnEnded = true;
                _controlStateWasReleased = false;
            }
            else
                _controlAndTurnEnded = false;
        }

        private void ControlLockStage()
        {
            if (!_isControlBlocked || HitIsInProgress)
                return;

            InternalDoAnyLogic();

            if (Attr.GetTurnPointsDelegate() < Attr.AutoAttackCost)
                _isControlBlocked = false;
        }

        private void InternalDoAnyLogic()
        {
            PickEnemyForThisTurn();

            _movementLocker = MoveInProgress && !IsWithingArmLength();

            if (MoveInProgress)
                return;
            else if (!MoveInProgress && !IsWithingArmLength())
                SetDestination();
            else if (IsWithingArmLength())
                if (HitTarget())
                    ReleaseEnemyForThisTurn();
        }

        #endregion
    }
}
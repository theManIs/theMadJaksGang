using System.Collections.Generic;
using System.Linq;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using Assets.TeamProjects.GamePrimal.SeparateComponents.UserMath;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public class AiFrame : IArtificial
    {
        #region Fields

        public AiFrameParams Attr;

        private MonoMechanicus _pickedEnemy;

        private bool _movementLocker;

        private bool _isControlBlocked;

        private readonly int _visibleRange = 10;

        private bool _hasControlBlocked;

        private bool _controlStateWasReleased;

        private bool _controlAndTurnEnded;

        #endregion


        #region Properties

        private bool IsWithingArmLength => _pickedEnemy &&
            (Attr.CurrentTransform.position - _pickedEnemy.transform.position).sqrMagnitude <= Attr.FightDistance * Attr.FightDistance;

        private bool HitIsInProgress => StaticProxyRouter.GetControllerAttackCapture().HasHit;

        #endregion


        #region IArtificial

        public void StartAssault() => _isControlBlocked = true;
        public bool CanNotDoAnyAction() => _controlAndTurnEnded;
        public void ClearControlAndTurnEnded() => _controlAndTurnEnded = false;

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
            .Where(c => c.IsBlueTeam != Attr.Monomech.IsBlueTeam).ToArray();

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

        private Vector3 ConnectVector(Vector3 opposeVector, Vector3 sourceVector3)
        {
            float realDistance = Vector3.Distance(opposeVector, sourceVector3);
            float maxMovementRange = MovementMath.CalcMovementLength(Attr.ActionPoints, Attr.MovementSpeed);
            float correctNormal = MovementMath.AttackVectorCoercion
                (realDistance, Attr.FightDistance, Attr.MeshError, maxMovementRange);

            return Vector3.Lerp(sourceVector3, opposeVector, correctNormal);
        }

        private Vector3 PickPositionOnCircle(Vector3 opposeVector, Vector3 sourceVector3)
        {

            int randAngle = Random.Range(1, 360);
            float randSeedX = Mathf.Cos(randAngle * Mathf.Deg2Rad);
            float randSeedY = Mathf.Sin(randAngle * Mathf.Deg2Rad);
            Vector3 shiftVector3 = new Vector3(randSeedX, 0, randSeedY);
            Vector3 destination = opposeVector + shiftVector3;

            return destination;
        }

        private void SetDestination()
        {
            if (_pickedEnemy)
            {
                /* The another way to compute an attack position
                Vector3 attackPosition =
                    ConnectVector(_pickedEnemy.transform.position, Attr.Monomech.transform.position);*/
                Vector3 attackPosition =
                    PickPositionOnCircle(_pickedEnemy.transform.position, Attr.Monomech.transform.position);

                Attr.Nma.SetDestination(attackPosition); // todo put in some NavMeshAgentWrapper

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

        private void ControlChangeStage()
        {
            _hasControlBlocked = _isControlBlocked;

            ControlLockStage();

            if (_hasControlBlocked != _isControlBlocked)
                _controlStateWasReleased = true;

            if (_controlStateWasReleased && !HitIsInProgress && !_movementLocker)
            {
                _controlAndTurnEnded = true;
                _controlStateWasReleased = false;
            }
        }

        private void ControlLockStage()
        {
            if ((!_isControlBlocked || HitIsInProgress) && !_movementLocker)
                return;

            InternalDoAnyLogic();

            if (Attr.GetTurnPointsDelegate() < Attr.AutoAttackCost && !_movementLocker)
                _isControlBlocked = false;
        }

        private void InternalDoAnyLogic()
        {
            PickEnemyForThisTurn();

            _movementLocker = _movementLocker && !IsWithingArmLength;

            if (_movementLocker)
                return;
            else if (!_movementLocker && !IsWithingArmLength)
                SetDestination();
            else if (IsWithingArmLength)
                if (HitTarget())
                    ReleaseEnemyForThisTurn();
        }

        #endregion


        #region Debug

        private void ControlLockStageDebug()
        {
            Debug.Log($"gameObject:{Attr.CurrentTransform.gameObject.name}" +
                      $" _isControlBlocked:{_isControlBlocked} _controlStateWasReleased:{_controlStateWasReleased}" +
                      $" MoveInProgress:{_movementLocker} HitIsInProgress:{HitIsInProgress}" +
                      $" _controlAndTurnEnded:{_controlAndTurnEnded} GetTurnPointsDelegate:{Attr.GetTurnPointsDelegate()}");
        }

        private void SetDestinationDebug(Vector3 attackPosition)
        {
            Debug.Log($"Distance between {Attr.CurrentTransform.gameObject.name} " +
                      $" between enemies {Vector3.Distance(Attr.Nma.destination, _pickedEnemy.transform.position)}" +
                      $" between destinations {Vector3.Distance(Attr.Nma.destination, attackPosition)}");
        }

        private void PickPositionOnCircleDebug(float randSeedX, float randSeedY, Vector3 shiftVector3) => 
            Debug.Log($"randSeedX {randSeedX} randSeedY {randSeedY} shiftVector3 {shiftVector3}");

        #endregion
    }
}
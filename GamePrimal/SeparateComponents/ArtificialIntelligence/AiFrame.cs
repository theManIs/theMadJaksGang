

using System.Collections.Generic;
using System.Linq;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.AbstractSources;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using Assets.TeamProjects.GamePrimal.SeparateComponents.UserMath;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public class AiFrame : EnableIncluded, IArtificial
    {
        public Transform CurrentTransform;
        public MonoMechanicus Monomech;

        private SortedDictionary<float, MonoMechanicus> _seekTargets = new SortedDictionary<float, MonoMechanicus>();
        private int _actionPoints;
        private int _movementSpeed;
        private SortedDictionary<float, MonoMechanicus> _enemiesWithinReach = new SortedDictionary<float, MonoMechanicus>();
        private int _autoAttackCost;
        private float _fightDistance;
        private float _meshError;
        private MonoMechanicus _pickedEnemy;

        private MonoMechanicus[] GetAllMonomechs() => Object.FindObjectsOfType<MonoMechanicus>(); //todo move to smart proxy

        private MonoMechanicus PickFirstEnemyToAttack() =>
            _enemiesWithinReach.Count > 0 ? _enemiesWithinReach.First().Value : null;

        private MonoMechanicus[] GetEnemies() => GetAllMonomechs()
            .Where(c => c.BlueRedTeam != Monomech.BlueRedTeam).ToArray();

        #region Setter
        public AiFrame SetActionPoints(int actionPoints)
        {
            _actionPoints = actionPoints;

            return this;
        }

        public AiFrame SetMovementSpeed(int movementSpeed)
        {
            _movementSpeed = movementSpeed;

            return this;
        }

        public AiFrame SetAutoAttackCost(int autoAttackCost)
        {
            _autoAttackCost = autoAttackCost;

            return this;
        }

        public AiFrame SetMeshRadius(float meshError)
        {
            Debug.Log($"_meshError {_meshError}");
            _meshError = MovementMath.CalcRadiusError(meshError);
            Debug.Log($"_meshError {_meshError}");

            return this;
        }

        public AiFrame SetFightDistance(float fightDistance)
        {
            _fightDistance = fightDistance;

            return this;
        }

        #endregion

        public AiFrame SeekTarget()
        {
            SortedDictionary<float, MonoMechanicus> sd = new SortedDictionary<float, MonoMechanicus>();

            foreach (var monomechEnemy in GetEnemies())
                sd.Add(Vector3.Distance(monomechEnemy.transform.position, Monomech.transform.position), monomechEnemy);

            if (DevelopFlag)
                foreach (KeyValuePair<float, MonoMechanicus> kvp in sd)
                    UnityEngine.Debug.Log("Total distance " + kvp.Value.gameObject.name + " " + kvp.Key);

            _seekTargets = sd;

            return this;
        }

        public AiFrame FilterWithinReach()
        {
            if (_actionPoints == 0 || _movementSpeed == 0 || _seekTargets.Count == 0)
                return this;

            _enemiesWithinReach = new SortedDictionary<float, MonoMechanicus>();

            foreach (KeyValuePair<float, MonoMechanicus> obj in _seekTargets)
                if (MovementMath.CalcMovementLength(_actionPoints, _movementSpeed) >= obj.Key)
                    _enemiesWithinReach.Add(obj.Key, obj.Value);

            if (DevelopFlag)
                foreach (KeyValuePair<float, MonoMechanicus> kvp in _enemiesWithinReach)
                    UnityEngine.Debug.Log("Within range distance " + kvp.Value.gameObject.name + " " + kvp.Key);

            return this;
        }

        public AiFrame FilterWithinAttack()
        {
            if (_actionPoints == 0 || _movementSpeed == 0 || _autoAttackCost == 0 || _seekTargets.Count == 0)
                return this;

            _enemiesWithinReach = new SortedDictionary<float, MonoMechanicus>();

            foreach (KeyValuePair<float, MonoMechanicus> obj in _seekTargets)
                if (MovementMath.CalcMovementLength(_actionPoints - _autoAttackCost, _movementSpeed) >= obj.Key)
                    _enemiesWithinReach.Add(obj.Key, obj.Value);

            if (DevelopFlag)
                foreach (KeyValuePair<float, MonoMechanicus> kvp in _enemiesWithinReach)
                    UnityEngine.Debug.Log("Within range distance " + kvp.Value.gameObject.name + " " + kvp.Key);

            return this;
        }

        private Vector3 ConnectVector(MonoMechanicus oppose)
        {
            float realDistance = Vector3.Distance(oppose.transform.position, Monomech.transform.position);
            float correctNormal = MovementMath.AttackVectorCoercion(realDistance, _fightDistance, _meshError);

            if (DevelopFlag)
                Debug.Log($"_fightDistance {_fightDistance} _meshError {_meshError} correctNormal: {correctNormal} realDistance {realDistance} distance {realDistance - correctNormal * realDistance}");

            return Vector3.Lerp(Monomech.transform.position, oppose.transform.position, correctNormal);
        }

        public AiFrame SetDestination(NavMeshAgent nvm)
        {
            if (!PickFirstEnemyToAttack())
                return this;

            nvm.SetDestination(ConnectVector(PickFirstEnemyToAttack()));

            return this;
        }

        private void PickEnemyForThisTurn() => _pickedEnemy = _pickedEnemy ?? PickFirstEnemyToAttack();
        private void ReleaseEnemyForThisTurn() => _pickedEnemy = null;

        public bool HitTarget()
        {
            PickEnemyForThisTurn();

            if (_pickedEnemy && DevelopFlag)
                Debug.Log("Versus " + Monomech.gameObject.name + " " + _pickedEnemy.gameObject.name);

            if (_pickedEnemy && Vector3.Distance(Monomech.transform.position, _pickedEnemy.transform.position) > _fightDistance)
                return false;
            else if (_pickedEnemy) 
                DamageLoggerAdapter.AttackCapture(Monomech, _pickedEnemy);

            ReleaseEnemyForThisTurn();

            return true;
        }
    }
}
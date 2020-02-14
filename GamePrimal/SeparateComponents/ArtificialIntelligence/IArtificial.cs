using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AI;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public interface IArtificial
    {
        AiFrame SeekTarget();
        AiFrame SetActionPoints(int actionPoints);
        AiFrame SetMovementSpeed(int movementSpeed);
        AiFrame SetAutoAttackCost(int autoAttackCost);
        AiFrame SetMeshRadius(float meshError);
        AiFrame FilterWithinReach();
        AiFrame FilterWithinAttack();
        AiFrame SetDestination(NavMeshAgent nvm);
        AiFrame SetFightDistance(float fightDistance);
        bool HitTarget();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public interface IArtificial
    {
        AiFrame SeekTarget();
        AiFrame SetActionPoints(int actionPoints);
        AiFrame SetMovementSpeed(int movementSpeed);
        AiFrame SetAutoAttackCost(int autoAttackCost);
        AiFrame FilterWithinReach();
        AiFrame FilterWithinAttack();
    }
}

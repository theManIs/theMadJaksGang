using System.Collections;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public class AiFrameBuilderNullObject : IArtificial
    {
        public IEnumerator HitTargetAsSoonAsPossible()
        {
            throw new System.NotImplementedException();
        }

        public IArtificial MoveToTarget()
        {
            throw new System.NotImplementedException();
        }

        public void DoAny() { }
        public void StartAssault() { }
    }
}
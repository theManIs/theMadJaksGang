using Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.AbilitiesTree
{
    public class IceShard : AbstractMagicBased
    {
        private Transform IceShardEffect = Resources.Load<Transform>(ResourcesList.IceShardEffect);
        public override Transform GetEffect() => IceShardEffect;
    }
}

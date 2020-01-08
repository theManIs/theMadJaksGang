using System;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.AbilitiesTree
{
    public abstract class AbstractWeaponBasedRanged : AbstractWeaponBased
    {
        public abstract Transform GetProjectile();
        public abstract bool HasProjectiles();
    }
}

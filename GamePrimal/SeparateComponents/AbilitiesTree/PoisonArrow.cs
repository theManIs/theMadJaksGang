using System;
using Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.AbilitiesTree
{
    public class PoisonArrow : AbstractWeaponBasedRanged
    {
        public Transform ActualProjectile = Resources.Load<Transform>(ResourcesList.PoisonArrowProjectile);
        public override bool HasProjectiles() => true;
        public override Transform GetProjectile() => ActualProjectile;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.WeaponOrigins
{
    class ZeroWeapon : WeaponOperatorAbstract
    {
        public override bool HasLastProjectile() => false;

        public override void SpawnProjectile(Transform projectile)
        {
        }

        public override void ShootAnyProjectile(Transform enemy)
        {
        }
    }
}

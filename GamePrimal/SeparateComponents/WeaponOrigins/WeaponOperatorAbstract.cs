using Assets.TeamProjects.GamePrimal.Mono;
using UnityEngine;


namespace Assets.TeamProjects.GamePrimal.SeparateComponents.WeaponOrigins
{
    public abstract class WeaponOperatorAbstract : MonoBehaviourBaseClass
    {
        #region Fields

        public float WeaponRange = 1;
        public WeaponTypes WeaponType = WeaponTypes.None;
        public bool isRanged = false;
        public Transform SpawnPoint;
        public int ShootPower = 30;
        public bool PauseAfterShoot = false;
        public Transform DefaultProjectile;

        #endregion


        #region Methods

        public abstract bool HasLastProjectile();
        public abstract void SpawnProjectile(Transform projectile);
        public abstract void ShootAnyProjectile(Transform enemy); 

        #endregion
    }
}

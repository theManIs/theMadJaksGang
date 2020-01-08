using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.TeamProjects.DemoAnimationScene.MiscellaneousWeapons.CommonScripts
{
    public enum WeaponTypes {
        Knife = 1,
        Crossbow = 2,
        HeavyBladed = 3,
        OneHandedBladed = 4
    }

    public class WeaponOperator : MonoBehaviour
    {
        public float WeaponRange;
        public WeaponTypes WeaponType = WeaponTypes.Crossbow;
        public bool isRanged;
        public Transform SpawnPoint;
        public int ShootPower = 30;
        public bool PauseAfterShoot = false;
        public Transform DefaultProjectile;
        public bool HasLastProjectile() => _lastProjectile;

        private Transform _lastProjectile;

        public void SpawnProjectile(Transform projectile)
        {
            if (_lastProjectile)
                Destroy(_lastProjectile.gameObject);

            GameObject newProjectile = Instantiate(projectile.gameObject);
            newProjectile.transform.position = SpawnPoint.position;
            newProjectile.transform.rotation = SpawnPoint.rotation;
            newProjectile.transform.parent = SpawnPoint.transform;
            _lastProjectile = newProjectile.transform;

            if (newProjectile.GetComponent<Rigidbody>())
                Destroy(newProjectile.GetComponent<Rigidbody>());
        }

        public void ShootAnyProjectile(Transform enemy)
        {
            if (!_lastProjectile) return;

            Rigidbody rb = _lastProjectile.gameObject.AddComponent<Rigidbody>();
            rb.velocity = _lastProjectile.transform.forward * ShootPower;
            _lastProjectile = null;

            if (PauseAfterShoot)
                EditorApplication.isPaused = true;
        }
    }
}

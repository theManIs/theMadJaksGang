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

        public void ShootAnyProjectile(Transform projectile, Transform enemy)
        {
            transform.LookAt(enemy);

            GameObject newProjectile = Instantiate(projectile.gameObject);
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            newProjectile.transform.position = SpawnPoint.position;
            newProjectile.transform.rotation = SpawnPoint.rotation;
            rb.velocity = newProjectile.transform.forward * ShootPower;
//            EditorApplication.isPaused = true;
        }
    }
}

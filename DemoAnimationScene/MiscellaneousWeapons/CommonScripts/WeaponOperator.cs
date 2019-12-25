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

//        public void ShootAnyProjectile(Transform projectile, Transform enemy)
//        {
////            Vector3 estimatedPos = enemy.position - transform.position;
//            GameObject projectileInstance = Instantiate(projectile.gameObject, SpawnPoint.position, projectile.transform.rotation);
//            Quaternion towardEnemy = Quaternion.FromToRotation(projectileInstance.transform.position, enemy.position);
////            projectileInstance.transform.LookAt(enemy);
//            projectileInstance.transform.rotation = towardEnemy;
//
//            if (!projectileInstance.GetComponent<Rigidbody>())
//                projectileInstance.AddComponent<Rigidbody>();
//
////            projectileInstance.GetComponent<Rigidbody>().AddRelativeForce(projectileInstance.transform.forward * 3000);
////            projectileInstance.GetComponent<Rigidbody>().velocity = Vector3.forward * 2;
//            projectileInstance.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000);
//
////            EditorApplication.isPaused = true;
////            Invoke("Stop", 0.1f);
//
//            //            Destroy(projectileInstance, 5f);
//        }
//
//        public void Stop()
//        {
//            EditorApplication.isPaused = true;
//        }
    }
}

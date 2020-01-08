using UnityEditor;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.AbilitiesTree
{
    public abstract class AbstractMagicBased : AbstractAbility
    {
        public int ActionCost = 2;
        public override bool IsWeaponBased() => false;
        public abstract Transform GetEffect();

        public void SpawnWithEnemyDirection(Transform ally, Transform enemy)
        {
            Quaternion lookRot = Quaternion.LookRotation((enemy.position + enemy.up / 2) - ally.position, ally.up);
            Transform objTrans = Object.Instantiate(GetEffect(), ally.position + ally.up, lookRot);

//            EditorApplication.isPaused = true;
    
            Object.Destroy(objTrans.gameObject, 10);
        }
    }
}

using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.TeamProjects.DemoAnimationScene.MiscellaneousWeapons.CommonScripts
{
    public enum WeaponTypes {
        Knife = 1,
        Crossbow = 2
    }

    public class WeaponOperator : MonoBehaviour
    {
        public float WeaponRange;
        public WeaponTypes WeaponType = WeaponTypes.Crossbow;

        private Transform _localTransform;


        // Start is called before the first frame update
        void Start()
        {
            _localTransform = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

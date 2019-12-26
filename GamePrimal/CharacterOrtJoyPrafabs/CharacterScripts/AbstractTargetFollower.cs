using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.CharacterOrtJoyPrafabs.CharacterScripts
{
    public enum UpdateType // The available methods of updating are:
    {
        FixedUpdate, // UserUpdate in UserUpdate (for tracking rigidbodies).
        LateUpdate, // UserUpdate in LateUpdate. (for tracking objects that are moved in UserUpdate)
        ManualUpdate, // user must call to update camera
    }

    public abstract class AbstractTargetFollower : MonoBehaviour
    {

        public bool LockTargetFerocious = false;
        public Transform m_Target;            // The target object to follow
        public UpdateType m_UpdateType;         // stores the selected update type

        public Transform Target => m_Target;

        protected Rigidbody targetRigidbody;

        private bool m_AutoTargetPlayer = false;

        protected abstract void FollowTarget(float deltaTime);

        protected virtual void Start()
        {
            if (m_Target)
                targetRigidbody = m_Target.GetComponent<Rigidbody>();
        }


        private void FixedUpdate()
        {
            if (m_UpdateType == UpdateType.FixedUpdate)
            {
                FollowTarget(Time.deltaTime);
            }
        }


        private void LateUpdate()
        {
            if (m_UpdateType == UpdateType.LateUpdate)
            {
                FollowTarget(Time.deltaTime);
            }
        }


        public void ManualUpdate()
        {
            if (m_UpdateType == UpdateType.ManualUpdate)
            {
                FollowTarget(Time.deltaTime);
            }
        }

        public virtual void SetTarget(Transform newTransform)
        {
            if (!LockTargetFerocious)
                m_Target = newTransform;
        }
    }
}

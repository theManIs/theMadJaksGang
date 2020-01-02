using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Proxies;
using UnityEngine;

namespace Assets.GamePrimal.CharacterOrtJoyPrafabs.Enemies.UnityChan
{
    [RequireComponent(typeof(Animator))]
    public class ConsoleUnityChan : MonoBehaviour
    {
        private Animator _animator;
        private ControllerEvent _ce;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _ce = StaticProxyRouter.GetControllerEvent();
        }

        private void OnEnable() => ControllerEvent.HitAppliedHandler += GetDamage;
        private void OnDisable() => ControllerEvent.HitAppliedHandler -= GetDamage;

        public void GetDamage(Transform captured, Transform broadcaster)
        {
            if (captured.GetInstanceID() != transform.GetInstanceID()) return;

            _animator.SetBool("Next", true);

//            transform.LookAt(broadcaster);
//            Invoke(nameof(ResetSpeed), 1);
        }

        private void ResetSpeed()
        {
            _animator.SetFloat("Speed", 0);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

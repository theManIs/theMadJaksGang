using Assets.GamePrimal.Controllers;
using UnityEngine;
using UnityEngine.AI;
using Assets.GamePrimal.Mono;
using Assets.GamePrimal.TextDamage;
using Assets.TeamProjects.GamePrimal.MainScene;
using Assets.TeamProjects.GamePrimal.Proxies;

namespace Assets.GamePrimal.CharacterOrtJoyPrafabs.Enemies.Skeleton
{
    public class AnimationControllerSkeleton : MonoBehaviour
    {
        public int DesiredRange = 4;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private bool _isBlip = false;
        private MainScene _manMainScene;
        private ControllerEvent _ce;
        private Transform lastEnemy;
        private Transform lastAlly;
        private MonoAmplifierRpg _amplifier;

        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _manMainScene = FindObjectOfType<MainScene>();
            _ce = StaticProxyRouter.GetControllerEvent();
            _amplifier = GetComponent<MonoAmplifierRpg>();

            ControllerFloatingText.Initialize();
        }

        private void ApplyDamage(Transform ally, Transform enemy)
        {
            if (ally.GetInstanceID() != transform.GetInstanceID()) return;

            int damageAmount = enemy.GetComponent<MonoAmplifierRpg>().CalcDamage();

            _amplifier.SubtractHealth(damageAmount);
            _animator.SetTrigger(_amplifier.HasDied() ? "Die" : "Damage");
            ControllerFloatingText.CreateFloatingText(damageAmount.ToString(), transform);
        }

        public void ImpactStage()
        {
            HitApply();
        }

//        private void OnEnable()
//        {
//            ControllerEvent.HitDetectedHandler += AttackTheEnemy;
//            ControllerEvent.HitAppliedHandler += ApplyDamage;
//        }
//
//        private void OnDisable()
//        {
//            ControllerEvent.HitDetectedHandler -= AttackTheEnemy;
//            ControllerEvent.HitAppliedHandler -= ApplyDamage;
//        }

        public void AttackTheEnemy(Transform enemy, Transform ally)
        {
            ClickToMove enemyClickToMove = enemy.GetComponent<ClickToMove>();
            ClickToMove allyClickToMove = gameObject.GetComponent<ClickToMove>();

            if (!enemyClickToMove || !allyClickToMove)
                return;

//            Debug.Log(enemyClickToMove.GetInstanceID() +  " " + allyClickToMove.GetInstanceID());
    
            if (enemyClickToMove.GetInstanceID() != allyClickToMove.GetInstanceID() && Vector3.Distance(enemy.position, transform.position) < DesiredRange)
            {
                _animator.SetTrigger("Attacking");
                transform.LookAt(enemy);

                lastAlly = ally;
                lastEnemy = enemy;

//                Invoke(nameof(HitApply), 0.5f);
            }
        }

        private void HitApply()
        {
            _ce.HitAppliedHandlerInvoke(lastEnemy, lastAlly);
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isBlip && _navMeshAgent.hasPath)
            {
                _isBlip = true;
                _animator.SetBool("IsStoped", false);
            }
            else if (_isBlip && !_navMeshAgent.hasPath)
            {
                _isBlip = false;
                _animator.SetBool("IsStoped", true);
            }

            if (Input.GetKeyDown(KeyCode.H))
                if (_manMainScene.GetFocus().gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    _animator.SetTrigger("Attacking");
        }
    }
}

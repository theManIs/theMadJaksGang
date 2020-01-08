using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Proxies;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.TeamProjects.GamePrimal.Controllers
{
    public class ControllerCharacterMovement
    {
        private ControllerAttackCapture _cAttackCapture;
        private ControllerEvent _cEvent;
        private NavMeshAgent _lastAgent;

        public ControllerCharacterMovement UserAwake()
        {
            _cAttackCapture = StaticProxyRouter.GetControllerAttackCapture();
            _cEvent = StaticProxyRouter.GetControllerEvent();

            return this;
        }

        public void UserEnable() => ControllerEvent.HitDetectedHandler += ClearDestination;
        public void UserDisable() => ControllerEvent.HitDetectedHandler -= ClearDestination;

        private void ClearDestination(AttackCaptureParams acp)
        {
            if (_lastAgent && _lastAgent.isOnNavMesh)
                _lastAgent.ResetPath();
        }

        public Vector3 GetClickPoint()
        {
//            Debug.Log("Contact click " + Time.deltaTime);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//                Debug.Log("Contact before " + Time.deltaTime);

                if (Physics.Raycast(ray, out RaycastHit hit, 1000))
                {
//                    Debug.Log("Contact " + hit.point + " " + Time.deltaTime);
                    return hit.point;
                }
            }

            return Vector3.zero;
        }

        private void MoveAnyMesh(Transform navMeshAgentTransform, Vector3 moveToPoint)
        {
            if (!navMeshAgentTransform) return;

            _lastAgent = navMeshAgentTransform.GetComponent<NavMeshAgent>();

            if (_lastAgent && _lastAgent.isOnNavMesh && moveToPoint != Vector3.zero)
                _lastAgent.destination = moveToPoint;
        }

        public void FixedUpdate(Transform focusedObject, bool doMove)
        {
            MonoMechanicus mop = focusedObject?.GetComponent<MonoMechanicus>();

            if (!StaticProxyStateHolder.UserOnUi && !StaticProxyStateHolder.LockModeOn)
                if (mop && doMove && (mop._monoAmplifierRpg.GetTurnPoints() > 0 || mop.InfiniteMoving))
                    this.MoveAnyMesh(focusedObject, this.GetClickPoint());
        }
    }
}

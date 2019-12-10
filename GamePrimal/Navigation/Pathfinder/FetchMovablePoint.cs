using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.PlayerLoop;

namespace Assets.GamePrimal.Navigation.Pathfinder
{
    public class FetchMovablePoint
    {
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

            NavMeshAgent nvm = navMeshAgentTransform.GetComponent<NavMeshAgent>();
            
            if (nvm && nvm.isOnNavMesh && moveToPoint != Vector3.zero)
                nvm.destination = moveToPoint;
        }

        public void FixedUpdate(Transform focusedObject, bool doMove)
        {
            if (doMove)
                this.MoveAnyMesh(focusedObject, this.GetClickPoint());
        }
    }
}

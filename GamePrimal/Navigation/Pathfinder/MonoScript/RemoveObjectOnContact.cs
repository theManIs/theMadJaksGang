using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Navigation.Pathfinder.MonoScript
{
    public class RemoveObjectOnContact : MonoBehaviour
    {
        public EventMoveCostApplied EMoveCostApplied = new EventMoveCostApplied();

        void OnTriggerEnter(Collider collider)
        {
            EMoveCostApplied.Invoke(new EventMoveCostAppliedParams() { ActualInvoker = transform });
            Destroy(gameObject);
        }
    }
}

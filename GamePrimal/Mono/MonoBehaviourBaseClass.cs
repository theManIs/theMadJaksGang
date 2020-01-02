using System;
using Assets.TeamProjects.GamePrimal.Helpers;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Mono
{
    public class MonoBehaviourBaseClass : MonoBehaviour
    {
        protected Transform FindInChildren<T>(Transform root)
        {
            Transform targetComponent = null;

            if (root.TryGetComponent(typeof(T), out Component component))
                if (component.GetType() == typeof(T))
                    return component.transform;

            if (root.transform.childCount > 0) 
                foreach (Transform branch in root.transform)
                    if (!targetComponent)
                        targetComponent = FindInChildren<T>(branch);

            return targetComponent;
        }

        protected Rigidbody AddAndGetRigidbody(Transform root)
        {
            Rigidbody rb = root.GetComponent<Rigidbody>();

            if (!rb)
                return root.gameObject.AddComponent<Rigidbody>();

            return rb;
        }
    }
}
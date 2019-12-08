using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObjectOnContact : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // FixedUpdate is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        Destroy(gameObject);
    }
}

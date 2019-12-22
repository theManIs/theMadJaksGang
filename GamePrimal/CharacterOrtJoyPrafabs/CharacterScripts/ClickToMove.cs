using System.Collections;    
using System.Collections.Generic;
using Assets.GamePrimal.Controllers;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    public Animator Animator;
    public bool IsActive = false;
    public float distanceToTheGround = .6f;

    private NavMeshAgent _navMeshAgent;

    private bool _running = false;

    // Start is called before the first frame update
    void Awake()
    {
        Animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
    }
//    // UserUpdate is called once per frame
//    void Update()
//    {
//        if (IsActive)
//            MoveMeshAgent();
//    }

    void UpdateAnimator()
    {
        if (!Animator)
            return;

        Animator.SetBool("Running", _running);

        SimulateJumping();
    }

    void SimulateJumping()
    {
        if (!IsGrounded())
        {
            Debug.Log("Jumping");
            Animator.SetBool("Jump", true);
        }
        else
        {
            Animator.SetBool("Jump", false);
        }
    }
     
//    void MoveMeshAgent()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//
//            RaycastHit hit;
//            
//            if (Physics.Raycast(ray, out hit, 1000))
//            {
//                Debug.Log("Contact " + hit.point + " " + Time.deltaTime);
//                _navMeshAgent.destination = hit.point;
//            }
//        }
//
//        _running = _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance;
//
//    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanceToTheGround);
    }
}

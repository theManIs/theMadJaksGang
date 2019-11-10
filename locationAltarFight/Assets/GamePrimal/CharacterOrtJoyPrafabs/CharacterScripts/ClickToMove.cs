using System.Collections;    
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    public Animator Animator;
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

    // Update is called once per frame
    void Update()
    {
        MoveMeshAgent();
    }

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
     
    void MoveMeshAgent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                _navMeshAgent.destination = hit.point;
            }
        }

        _running = _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance;

    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanceToTheGround);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugPathTrace : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _NavMeshAgentPath;
    private LineRenderer _LineRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        _LineRenderer = GetComponent<LineRenderer>();
    }

    // UserUpdate is called once per frame
    void Update()
    {
        if (_NavMeshAgentPath.hasPath)
        {
            _LineRenderer.positionCount = _NavMeshAgentPath.path.corners.Length;

            //Debug.Log(_NavMeshAgentPath.path.corners[0] + " " + _NavMeshAgentPath.path.corners[1]);

            _LineRenderer.SetPositions(_NavMeshAgentPath.path.corners);

            //_LineRenderer.enabled = true;
        }
        else
        {
            _LineRenderer.enabled = false;
        }
    }
}

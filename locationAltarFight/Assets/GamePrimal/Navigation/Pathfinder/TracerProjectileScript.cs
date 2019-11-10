using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;

public class TracerProjectileScript : MonoBehaviour
{
    public float StepLength = 3; 

    [SerializeField] private NavMeshAgent _NavMeshAgentFollow;
    [SerializeField] private GameObject _wayPoint;
    [SerializeField] private GameObject m_FinalDestination;
    [SerializeField] private GameObject m_CheckWayPoint;

    private bool _pathWasSet = false;
    private List<GameObject> _objectsList = new List<GameObject>();
    private Vector3 _lastEndWayPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void WasteWayPoints()
    {
        foreach (GameObject wayPoint in _objectsList)
            Destroy(wayPoint);

        _pathWasSet = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3[] corners = _NavMeshAgentFollow.path.corners;
        Vector3 lastCorner = _NavMeshAgentFollow.path.corners[_NavMeshAgentFollow.path.corners.Length - 1];

        if (_pathWasSet && !_NavMeshAgentFollow.hasPath)
        {
            WasteWayPoints();
        }
        else if (corners.Length < 2 || (_pathWasSet || !_NavMeshAgentFollow.hasPath) && _lastEndWayPoint == lastCorner)
            return;

        if (corners.Length > 1)
        {
            if (_lastEndWayPoint != lastCorner)
                WasteWayPoints();


            for (int ii = 0; ii < corners.Length - 1; ii++)
            {
                int distanceCounter = Convert.ToInt32(Vector3.Distance(corners[ii], corners[ii + 1]) / 3);

                if (distanceCounter <= .5)
                    continue;

                float coefX = (corners[ii + 1].x - corners[ii].x) / distanceCounter;
                float coefZ = (corners[ii + 1].z - corners[ii].z) / distanceCounter;
                _lastEndWayPoint = lastCorner;
                //Debug.Log(coefX + " " + coefZ + " " + distanceCounter);
                for (int i = 1; i < distanceCounter + 1; i++)
                {
                    if (i % StepLength == 0)
                        SpawnCheckPoint(coefX * i, coefZ * i, corners[ii], (int)(i / StepLength));
                    else if (i == (distanceCounter))
                        SpawnLastWayPoint(coefX * i, coefZ * i, corners[ii]);
                    else
                        SpawnNewWayPoint(coefX * i, coefZ * i, corners[ii]);
                    
                    //GameObject objToSpawn = Instantiate(_wayPoint);
                    ////GameObject objToSpawn = new GameObject("TraceProjectileCheckpoint");
                    //objToSpawn.tag = "TraceProjectileCheckpoint";
                    ////objToSpawn.AddComponent<BoxCollider>();
                    //objToSpawn.transform.position = corners[ii] + new Vector3(coefX * i, 15, coefZ * i);

                    //RaycastHit hit;

                    //Physics.Raycast(objToSpawn.transform.position, Vector3.down, out hit, 30);

                    //Vector3 bottomOffset = new Vector3(0, objToSpawn.GetComponent<MeshRenderer>().bounds.size.y, 0);
                    //Debug.Log(bottomOffset);
                    //objToSpawn.transform.position = hit.point + bottomOffset;

                    //_objectsList.Add(objToSpawn);
                }
            }

            _pathWasSet = true;
        }
    }

    private void SpawnCheckPoint(float positionOffsetX, float positionOffsetZ, Vector3 currentCorner, int checkPointPos)
    {
        GameObject objToSpawn = Instantiate(m_CheckWayPoint);
        objToSpawn.tag = "TraceProjectileCheckpoint";
        objToSpawn.transform.position = currentCorner + new Vector3(positionOffsetX, 15, positionOffsetZ);

        RaycastHit hit;

        Physics.Raycast(objToSpawn.transform.position, Vector3.down, out hit, 30);

        Vector3 bottomOffset = new Vector3(0, objToSpawn.GetComponent<MeshRenderer>().bounds.size.y, 0);
        objToSpawn.transform.position = hit.point + bottomOffset;

        objToSpawn.GetComponentInChildren<TextMeshProUGUI>().SetText(Convert.ToString(checkPointPos));

        _objectsList.Add(objToSpawn);
    }

    private void SpawnLastWayPoint(float positionOffsetX, float positionOffsetZ, Vector3 currentCorner)
    {
        GameObject objToSpawn = Instantiate(m_FinalDestination);
        objToSpawn.tag = "TraceProjectileCheckpoint";
        objToSpawn.transform.position = currentCorner + new Vector3(positionOffsetX, 15, positionOffsetZ);

        RaycastHit hit;

        Physics.Raycast(objToSpawn.transform.position, Vector3.down, out hit, 30);

        Vector3 bottomOffset = new Vector3(0, 1, 0);
        objToSpawn.transform.position = hit.point + bottomOffset;

        _objectsList.Add(objToSpawn);
    }

    void SpawnNewWayPoint(float positionOffsetX, float positionOffsetZ, Vector3 currentCorner)
    {
        GameObject objToSpawn = Instantiate(_wayPoint);
        objToSpawn.tag = "TraceProjectileCheckpoint";
        objToSpawn.transform.position = currentCorner + new Vector3(positionOffsetX, 15, positionOffsetZ);

        RaycastHit hit;

        Physics.Raycast(objToSpawn.transform.position, Vector3.down, out hit, 30);

        Vector3 bottomOffset = new Vector3(0, objToSpawn.GetComponent<MeshRenderer>().bounds.size.y, 0);
        objToSpawn.transform.position = hit.point + bottomOffset;

        _objectsList.Add(objToSpawn);
    }

    void Unsatisfy()
    {
        if (_NavMeshAgentFollow.hasPath)
        {
            //if ((Int32)DateTime.Now.Subtract(_lastEmbded).TotalSeconds > 5)
            //{
            GameObject objToSpawn = new GameObject("TraceProjectileCheckpoint");
            objToSpawn.tag = "TraceProjectileCheckpoint";
            objToSpawn.AddComponent<BoxCollider>();
            //objToSpawn.GetComponent<BoxCollider>().isTrigger = true;
            objToSpawn.transform.position = transform.position;
            //}

            GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("TraceProjectileCheckpoint");

            for (int i = 0; i < checkpoints.Length - 1; i++)
            {
                Debug.DrawLine(checkpoints[i].transform.position, checkpoints[i + 1].transform.position, Color.green);
            }
        }
        else
        {
            GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("TraceProjectileCheckpoint");

            for (int i = 0; i < checkpoints.Length - 1; i++)
            {
                Destroy(checkpoints[i]);
            }
        }

       

        
    }
}

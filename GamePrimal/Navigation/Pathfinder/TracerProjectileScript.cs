﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Assets.GamePrimal.Navigation.Pathfinder
{
    public class TracerProjectileScript : MonoBehaviour
    {
        public float StepLength = 3; 

        private NavMeshAgent _NavMeshAgentFollow;
        private GameObject _wayPoint;
        private GameObject m_FinalDestination;
        private GameObject m_CheckWayPoint;

        private bool _pathWasSet = false;
        private List<GameObject> _objectsList = new List<GameObject>();
        private Vector3 _lastEndWayPoint;
        private Vector3 _bottomPadding;
        private Vector3 _lastCorner;
        private readonly int InitPositionToPlungeRay = 15;
        private readonly int MaxRayCastDistance = 30;

        // Start is called before the first frame update
        public void Start()
        {
            m_FinalDestination = Resources.Load<GameObject>("Horizontal_Beacon_11894");
            m_CheckWayPoint = Resources.Load<GameObject>("WayPointMove Variant_12046");
            _wayPoint = Resources.Load<GameObject>("WayPointWhite_11970");
            _bottomPadding = new Vector3(0, _wayPoint.GetComponent<MeshRenderer>().bounds.size.y, 0);
        }

        void WasteWayPoints()
        {
            foreach (GameObject wayPoint in _objectsList)
                Object.Destroy(wayPoint);

            _pathWasSet = false;
        }

        public void SetNavAgent(Transform navAgentTransform)
        {
            if (navAgentTransform)
            {
                NavMeshAgent nvm = navAgentTransform.GetComponent<NavMeshAgent>();

                if (nvm)
                    _NavMeshAgentFollow = nvm;
            }
        }

        // FixedUpdate is called once per frame
        public void FixedUpdate()
        {
            if (!_NavMeshAgentFollow) return;

            Vector3[] corners = _NavMeshAgentFollow.path.corners;

            if (corners.Length <= 0) return;

            _lastCorner = _NavMeshAgentFollow.path.corners[_NavMeshAgentFollow.path.corners.Length - 1];

            if (_pathWasSet && !_NavMeshAgentFollow.hasPath)
            {
                WasteWayPoints();
            }
            else if (corners.Length < 2 || (_pathWasSet || !_NavMeshAgentFollow.hasPath) && _lastEndWayPoint == _lastCorner)
                return;

            if (corners.Length > 1)
            {
                if (_lastEndWayPoint != _lastCorner)
                    WasteWayPoints();


                for (int ii = 0; ii < corners.Length - 1; ii++)
                {
                    int distanceCounter = Convert.ToInt32(Vector3.Distance(corners[ii], corners[ii + 1]) / 3);

                    if (distanceCounter <= .5)
                        continue;

                    float coefX = (corners[ii + 1].x - corners[ii].x) / distanceCounter;
                    float coefZ = (corners[ii + 1].z - corners[ii].z) / distanceCounter;
                    _lastEndWayPoint = _lastCorner;
                    //Debug.Log(coefX + " " + coefZ + " " + distanceCounter);
                    for (int i = 1; i < distanceCounter + 1; i++)
                    {
                        if (i == (distanceCounter))
                            SpawnLastWayPoint(coefX * i, coefZ * i, corners[ii]);
                        else if(i % StepLength == 0)
                            SpawnCheckPoint(coefX * i, coefZ * i, corners[ii], (int)(i / StepLength));
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

        private GameObject SpawnPlaceRotate(GameObject gm, Vector3 curPos)
        {
            GameObject objToSpawn = Object.Instantiate(gm, curPos, gm.transform.rotation);
            objToSpawn.tag = "TraceProjectileCheckpoint";

            return objToSpawn;
        }

        private void RotateToward(GameObject curObj, Vector3 toward)
        {
            Vector3 relativeShift = toward - curObj.transform.position;
            float eulerAngleY = UnityEngine.Quaternion.LookRotation(relativeShift, curObj.transform.position).eulerAngles.y;
            curObj.transform.rotation = Quaternion.Euler(0, (int)eulerAngleY, 0);
        }

        private GameObject PutNewPointOnTheLine(GameObject targetObject, float positionOffsetX, float positionOffsetZ, Vector3 currentCorner)
        {
            GameObject objToSpawn = SpawnPlaceRotate(targetObject, currentCorner + new Vector3(positionOffsetX, InitPositionToPlungeRay, positionOffsetZ));

            Physics.Raycast(objToSpawn.transform.position, Vector3.down, out RaycastHit hit, MaxRayCastDistance);

            objToSpawn.transform.position = hit.point + _bottomPadding;

            _objectsList.Add(objToSpawn);

            return objToSpawn;
        }

        private void SpawnCheckPoint(float positionOffsetX, float positionOffsetZ, Vector3 currentCorner, int checkPointPos)
        {
            GameObject gm = PutNewPointOnTheLine(m_CheckWayPoint, positionOffsetX, positionOffsetZ, currentCorner);

            RotateToward(gm, _lastCorner);
            gm.GetComponentInChildren<TextMeshProUGUI>().SetText(Convert.ToString(checkPointPos));
        }

        private void SpawnLastWayPoint(float positionOffsetX, float positionOffsetZ, Vector3 currentCorner)
        {
            PutNewPointOnTheLine(m_FinalDestination, positionOffsetX, positionOffsetZ, currentCorner);
        }

        void SpawnNewWayPoint(float positionOffsetX, float positionOffsetZ, Vector3 currentCorner)
        {
            GameObject gm = PutNewPointOnTheLine(_wayPoint, positionOffsetX, positionOffsetZ, currentCorner);

            RotateToward(gm, _lastCorner);
        }

//    void Unsatisfy()
//    {
//        if (_NavMeshAgentFollow.hasPath)
//        {
//            //if ((Int32)DateTime.Now.Subtract(_lastEmbded).TotalSeconds > 5)
//            //{
//            GameObject objToSpawn = new GameObject("TraceProjectileCheckpoint");
//            objToSpawn.tag = "TraceProjectileCheckpoint";
//            objToSpawn.AddComponent<BoxCollider>();
//            //objToSpawn.GetComponent<BoxCollider>().isTrigger = true;
//            objToSpawn.transform.position = transform.position;
//            //}
//
//            GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("TraceProjectileCheckpoint");
//
//            for (int i = 0; i < checkpoints.Length - 1; i++)
//            {
//                Debug.DrawLine(checkpoints[i].transform.position, checkpoints[i + 1].transform.position, Color.green);
//            }
//        }
//        else
//        {
//            GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("TraceProjectileCheckpoint");
//
//            for (int i = 0; i < checkpoints.Length - 1; i++)
//            {
//                Destroy(checkpoints[i]);
//            }
//        }
//
//       
//
//        
//    }
    }
}

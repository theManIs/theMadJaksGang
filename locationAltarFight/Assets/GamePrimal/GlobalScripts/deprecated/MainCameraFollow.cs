using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MainCameraFollow : MonoBehaviour
{
    public Func<Vector3> GetCameraFollowPosition;
    public Transform PlayerTransform;
    public Camera myCamera;
    private static float _zoomCameraPositionZ = 0;
    private static float _zoomCameraPositionY = 0;
    private float multiplierWheelSpeed = 10f;

    // Start is called before the first frame update
    public void Setup(Func<Vector3> GetCameraFollowPosition)
    {
        this.GetCameraFollowPosition = GetCameraFollowPosition;
    }

    void Start()
    {
        //myCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
         HandleMovement();
         HandleZoom();
    }

    void HandleMovement()
    {
        //Vector3 cameraFollowPosition = PlayerTransform.position;

        if (this.GetCameraFollowPosition is Func<Vector3>)
        {
            float cameraMoveSpeed = 0.1f;

            Vector3 cameraFollowPosition = this.GetCameraFollowPosition();
            cameraFollowPosition.y += 3f;
            cameraFollowPosition.z -= 5f;
            cameraFollowPosition.z += _zoomCameraPositionZ;
            cameraFollowPosition.y += _zoomCameraPositionY;

            Vector3 cameraMoveDirection = (cameraFollowPosition - transform.position).normalized;
            float distance = Vector3.Distance(cameraFollowPosition, transform.position);
            Vector3 cameraFollowPositionInitial = this.GetCameraFollowPosition();

            if (distance > 0)
            {
                Vector3 newCameraPosition = cameraFollowPositionInitial + cameraMoveDirection * distance * cameraMoveSpeed * Time.deltaTime;

                Debug.Log(cameraMoveDirection * distance * cameraMoveSpeed * Time.deltaTime);

                float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

                if (distanceAfterMoving > distance)
                {
                    newCameraPosition = cameraFollowPosition;
                }

                transform.position = newCameraPosition;
            }

        }
    }

    void HandleZoom()
    {
        if (myCamera is Camera) 
        {
            float radianAngleSlantZ = Convert.ToSingle((transform.eulerAngles.x * Math.PI) / 180);
            float radianAngleSlantY = Convert.ToSingle((transform.eulerAngles.x * Math.PI) / 180);
            _zoomCameraPositionZ += Convert.ToSingle(Input.GetAxis("Mouse ScrollWheel") * multiplierWheelSpeed * Math.Sin(radianAngleSlantZ));
            _zoomCameraPositionY -= Convert.ToSingle(Input.GetAxis("Mouse ScrollWheel") * (multiplierWheelSpeed / 5 ) * Math.Cos(radianAngleSlantY));
        }
    }
}

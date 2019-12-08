using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float RotationSpeed = 50;
    public GameObject MainCharacter;
    public Camera CurrentCamera;
    private float multiplierWheelSpeed = 10f;
    private float _zoomCameraPositionZ = 0f;
    private float _zoomCameraPositionY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // FixedUpdate is called once per frame
    void Update()
    {
        float axisXShift = Input.GetAxis("Mouse X");
        float axisYShift = Input.GetAxis("Mouse Y");

        if ((axisXShift != 0.0 || axisYShift != 0.0) && Input.GetMouseButton(2))
            transform.Rotate(RotationSpeed * Time.deltaTime * axisYShift, RotationSpeed * Time.deltaTime * axisXShift, -transform.eulerAngles.z);

        if (Math.Abs(Vector3.Distance(MainCharacter.transform.position, transform.position)) > 0)
            transform.position = MainCharacter.transform.position;


        //if (Math.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
        //{
        //    //Vector3 currentCameraRotatorPosition = CurrentCamera.transform.position;
        //    //float radianAngleSlantZ = Convert.ToSingle(((CurrentCamera.transform.eulerAngles.x + transform.eulerAngles.x) * Math.PI) / 180);
        //    //float radianAngleSlantY = Convert.ToSingle(((transform.eulerAngles.y) * Math.PI) / 180);
        //    //currentCameraRotatorPosition.z += Convert.ToSingle(Input.GetAxis("Mouse ScrollWheel") * multiplierWheelSpeed * Math.Sin(radianAngleSlantZ) * Math.Cos(radianAngleSlantY));
        //    //Debug.Log(Convert.ToSingle(Input.GetAxis("Mouse ScrollWheel") * multiplierWheelSpeed * Math.Sin(radianAngleSlantZ) * Math.Cos(radianAngleSlantY)));
        //    //currentCameraRotatorPosition.y -= Convert.ToSingle(Input.GetAxis("Mouse ScrollWheel") * (multiplierWheelSpeed / 5) * Math.Sin(radianAngleSlantZ));
        //    //CurrentCamera.transform.position = currentCameraRotatorPosition;

        //    Vector3 currentCameraRotatorPosition = CurrentCamera.transform.position;

        //    currentCameraRotatorPosition.x += .1f;
        //    currentCameraRotatorPosition.y += .1f;
        //    currentCameraRotatorPosition.z += transform.position.z;

        //    CurrentCamera.transform.position = currentCameraRotatorPosition;
        //}
    }
}

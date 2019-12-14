using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseCameraScript : MonoBehaviour
{
    public GameObject CameraLeadPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // FixedUpdate is called once per frame
    void Update()
    {
        VectorZoom();

        if (Input.GetKeyDown(KeyCode.W))
        {

        }
        else
        {

        }
    }

    int AxisSign(float cameraCoord, float pointCoord)
    {
        if (cameraCoord < pointCoord)
            return -1;
        else
            return 1;
    }

    void VectorZoom()
    {
        float axisShift = Input.GetAxis("Mouse ScrollWheel");

        if (Math.Abs(axisShift) > 0)
        {
            //Vector3 currentCameraRotatorPosition = CurrentCamera.transform.position;
            //float radianAngleSlantZ = Convert.ToSingle(((CurrentCamera.transform.eulerAngles.x + transform.eulerAngles.x) * Math.PI) / 180);
            //float radianAngleSlantY = Convert.ToSingle(((transform.eulerAngles.y) * Math.PI) / 180);
            //currentCameraRotatorPosition.z += Convert.ToSingle(Input.GetAxis("Mouse ScrollWheel") * multiplierWheelSpeed * Math.Sin(radianAngleSlantZ) * Math.Cos(radianAngleSlantY));
            //Debug.Log(Convert.ToSingle(Input.GetAxis("Mouse ScrollWheel") * multiplierWheelSpeed * Math.Sin(radianAngleSlantZ) * Math.Cos(radianAngleSlantY)));
            //currentCameraRotatorPosition.y -= Convert.ToSingle(Input.GetAxis("Mouse ScrollWheel") * (multiplierWheelSpeed / 5) * Math.Sin(radianAngleSlantZ));
            //CurrentCamera.transform.position = currentCameraRotatorPosition;

            Vector3 leadPointPosition = CameraLeadPoint.transform.position;
            Vector3 cameraCurPosition = transform.position;

            int theSign = axisShift < 0 ? -1 : 1;

            cameraCurPosition.x -= theSign * (cameraCurPosition.x - leadPointPosition.x) * .2f;

            if (Math.Abs(cameraCurPosition.y - leadPointPosition.y) < 1)
                cameraCurPosition.y = leadPointPosition.y;
            else
                cameraCurPosition.y -= theSign * (cameraCurPosition.y - leadPointPosition.y) * .2f;

            cameraCurPosition.z -= theSign * (cameraCurPosition.z - leadPointPosition.z) * .2f;

            Debug.Log("d" + Vector3.Distance(leadPointPosition, cameraCurPosition) + " x y z: " + cameraCurPosition.x + " " + cameraCurPosition.y + " " + cameraCurPosition.z);

            transform.position = cameraCurPosition;
        }
    }
}

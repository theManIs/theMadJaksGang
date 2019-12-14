using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// -6.04 -294.71 22.26
public class WindowCharacterPortrait : MonoBehaviour
{
    public Transform CameraTransform;
    public Transform FallowTransform;
    public float RequiredDistance;

    void Update()
    {
        //CameraTransform.position = FallowTransform.position + new Vector3(RequiredDistance, RequiredDistance, RequiredDistance);
    }
}

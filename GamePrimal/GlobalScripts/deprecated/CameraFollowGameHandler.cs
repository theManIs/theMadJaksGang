using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowGameHandler : MonoBehaviour
{
    public MainCameraFollow MainCameraFollow;
    public Transform PlayerTransform;

    // Start is called before the first frame update
    void Start()
    {
        MainCameraFollow.Setup(() => PlayerTransform.position);
    }
}

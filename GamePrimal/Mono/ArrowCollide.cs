using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollide : MonoBehaviour
{
    private void Start() => Destroy(gameObject, 10);
    private void OnCollisionEnter() => Destroy(gameObject);
}

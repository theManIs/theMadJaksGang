using System.Collections;
using System.Collections.Generic;
using Assets.TeamProjects.GamePrimal.SeparateComponents.WeaponOrigins;
using UnityEditor;
using UnityEngine;

public class ArrowShooter : MonoBehaviour
{
    private GameObject _arrow;
    public int ShootPower = 20;
    public WeaponOperator Wo;
    public Transform EnemyCube;
    public bool ShootRealProjectile = true;

    void Start()
    {
//        _arrow = Resources.Load("ArrowProjectile_16716") as GameObject;
        _arrow = Resources.Load("ArrowProjectile_-405228") as GameObject;
//        Wo = GetComponent<WeaponOperator>();

        if (!_arrow.GetComponent<Rigidbody>())
            _arrow.AddComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ShootRealProjectile && Input.GetKeyDown(KeyCode.Mouse0)) 
            transform.LookAt(EnemyCube);
//        if (Input.GetKeyDown(KeyCode.Mouse0))
//        {
//            GameObject newProjectile = Instantiate(_arrow);
//            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
//            newProjectile.transform.position = transform.position;
//            newProjectile.transform.rotation = transform.rotation;
//
//            rb.velocity = gameObject.transform.forward * ShootPower;
//        }

        if (ShootRealProjectile && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Wo.SpawnProjectile(_arrow.transform);
            Wo.ShootAnyProjectile( EnemyCube);
        }
    }
}

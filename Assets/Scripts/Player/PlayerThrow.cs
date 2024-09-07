using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    public float throwForce = 40f;
    public float throwUpFroce = 10f;

    private InputManager inputManager;
    public Transform playerGun;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        if (inputManager.OnFoot.Throw.triggered)
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {

        GameObject granade = GameObject.Instantiate(Resources.Load("Prefabs/Grenade") as GameObject, playerGun.position, playerGun.rotation);



        //Vector3 shootDirection =  (playerGun.position - Camera.main.transform.position).normalized;
        Vector3 forceToAdd = Camera.main.transform.forward * throwForce + transform.up * throwUpFroce;



        granade.GetComponent<Rigidbody>().AddForce(forceToAdd, ForceMode.Impulse);
    }
}

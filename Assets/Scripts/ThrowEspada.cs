using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class ThrowEspada : Espada {

    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    public int totalThrows;
    public float throwCooldown;

    public KeyCode throwkey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;
    public float thrustForce;
    public float rotationThrow;

    bool readyToThrow;

    private Rigidbody projectileRb;

    private void Start() {
        readyToThrow = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(throwkey) && readyToThrow && totalThrows > 0)
        {
            Use();
        }
        if (projectileRb != null)
        {
            projectileRb.AddRelativeForce(Vector3.up * -thrustForce, ForceMode.Force);
        }
    }
    public override void Use()
    {
        readyToThrow = false;
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.transform.rotation);
        projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;
        Vector3 torque = projectile.transform.right * rotationThrow;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        projectileRb.AddTorque(torque);

        totalThrows--;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

}

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

    PhotonView PV;

    private void Start() {
        PV = GetComponent<PhotonView>();
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

        if (PV.IsMine)
        {
            PV.RPC("RPC_Throw", RpcTarget.AllBuffered);
        }
            totalThrows--;

            Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    [PunRPC]
    void RPC_Throw()
    {
        if (PV.IsMine)
        {

        Transform cam = FindFirstObjectByType<Camera>().transform;

        GameObject projectilePhoton = PhotonNetwork.Instantiate(objectToThrow.name, attackPoint.position, cam.transform.rotation);
        projectileRb = projectilePhoton.GetComponent<Rigidbody>();

        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;
        Vector3 torque = projectilePhoton.transform.right * rotationThrow;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        projectileRb.AddTorque(torque);
        }

    }

}

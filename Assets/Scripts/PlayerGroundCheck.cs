using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{

    PlayerController playerController;

   void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(true);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("ON TRIGGER EXIT");
        if (other.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(false);

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
            return;
        playerController.SetGroundedState(true);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(true);
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("ONCOLLISIONEXIT");

        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(false);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(true);
    }
}

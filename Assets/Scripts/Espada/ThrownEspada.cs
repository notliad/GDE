
using Photon.Pun;
using UnityEngine;

public class ThrownEspada : Espada
{
    public float throwForce = 3;
    public float maxThrowForce = 5;
    public float throwUpwardForce = 5;
    public float thrustForce = 5;
    public float rotationThrow = 5;
    private Rigidbody projectileRb;
    private AudioSource audioSource;
    private void Start()
    {
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        var forward = (Vector3)instantiationData[0];
        var time = (float)instantiationData[1];
        projectileRb = GetComponent<Rigidbody>();

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        Vector3 forceToAdd = forward * (throwForce * time) + transform.up * throwUpwardForce;
        Vector3 torque = transform.right * rotationThrow;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        projectileRb.AddTorque(torque);
    }
    void FixedUpdate()
    {
        if (projectileRb != null)
        {
            projectileRb.AddRelativeForce(Vector3.up * -thrustForce, ForceMode.Force);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(collision.relativeVelocity.magnitude, collision.collider);
    }

    public override void Use(float time)
    {
    }

    public override void LetGo()
    {
    }
}


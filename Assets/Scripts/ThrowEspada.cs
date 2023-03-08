using Photon.Pun;
using UnityEngine;


public class ThrowEspada : Espada
{

    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    public int totalThrows;
    public float throwCooldown;

    public KeyCode throwkey = KeyCode.Mouse0;
    public KeyCode letgoKey = KeyCode.Mouse1;

    public float throwForce;
    public float maxThrowForce;
    public float throwUpwardForce;
    public float thrustForce;
    public float rotationThrow;
    AudioSource audioSource;
    [SerializeField] Collider espadaCollider;
    [SerializeField] Collider fogoCollider;

    bool readyToThrow;

    private Rigidbody projectileRb;

    PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        readyToThrow = true;
    }

    private void FixedUpdate()
    {
        if (projectileRb != null)
        {
            projectileRb.AddRelativeForce(Vector3.up * -thrustForce, ForceMode.Force);
        }
        //Velocidade da espada
        //if (projectileRb != null)
        //{
        //    Debug.Log(projectileRb.velocity.magnitude);
        //}
    }

    public override void LetGo()
    {
        readyToThrow = false;

        if (PV.IsMine)
        {
            PV.RPC("RPC_LetGo", RpcTarget.AllBuffered);
        }
        totalThrows--;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    public override void Use(float time)
    {
        readyToThrow = false;

        if (PV.IsMine)
        {
            PV.RPC("RPC_Throw", RpcTarget.AllBuffered, time);
        }
        totalThrows--;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(collision.relativeVelocity.magnitude, collision.collider);

        // Perform other actions as needed
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    [PunRPC]
    void RPC_Throw(float time)
    {
        if (PV.IsMine)
        {
            Transform cam = FindFirstObjectByType<Camera>().transform;

            GameObject projectilePhoton = PhotonNetwork.Instantiate(objectToThrow.name, attackPoint.position, cam.transform.rotation);
            projectileRb = projectilePhoton.GetComponent<Rigidbody>();

            audioSource = projectilePhoton.GetComponent<AudioSource>();
            audioSource.Play();

            Vector3 forceToAdd = cam.transform.forward * (throwForce * time) + transform.up * throwUpwardForce;
            Vector3 torque = projectilePhoton.transform.right * rotationThrow;

            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
            projectileRb.AddTorque(torque);
        }

    }

    [PunRPC]
    void RPC_LetGo()
    {
        if (PV.IsMine)
        {
            Transform cam = FindFirstObjectByType<Camera>().transform;

            GameObject projectilePhoton = PhotonNetwork.Instantiate(objectToThrow.name, attackPoint.position, cam.transform.rotation);
            projectileRb = projectilePhoton.GetComponent<Rigidbody>();

            audioSource = projectilePhoton.GetComponent<AudioSource>();
            audioSource.Play();

            Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;
            Vector3 torque = projectilePhoton.transform.right * rotationThrow;

            projectileRb.AddForce(Vector3.zero, ForceMode.Impulse);
            projectileRb.AddTorque(Vector3.zero);
        }

    }

}

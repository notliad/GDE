using Assets.Scripts.Player;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] Image healthbarImage;
    [SerializeField] Image throwPower;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] Animator animator;

    [SerializeField] Collider headCollider;
    [SerializeField] Collider armsCollider;
    [SerializeField] Collider chestCollider;
    [SerializeField] Collider legsCollider;
    [SerializeField] Collider feetCollider;


    [SerializeField] Item[] items;

    int itemIndex;
    int previousItemIndex = -1;
    bool grounded;

    Rigidbody rb;
    PhotonView PV;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    private float startTime;
    private float maxHoldTime = 2;


    PlayerManager playerManager;
    PauseManager pauseMenu;
    public PlayerMechanics playerMechanics;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        pauseMenu = GetComponentInChildren<PauseManager>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        playerMechanics = new PlayerMechanics(this, cameraHolder, animator);
    }

    void Update()
    {
        if (!PV.IsMine)
            return;
        if (!pauseMenu.isPaused)
        {
            playerMechanics.OnUpdate();
        }


        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (itemIndex >= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1);
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (itemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }
        if (!pauseMenu.isPaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTime = Time.time;
            }

            if (Input.GetMouseButton(0))
            {
                float holdTime = Time.time - startTime;
                throwPower.fillAmount = holdTime / maxHoldTime; // set the fill amount based on how long the button has been held down
            }
            if (Input.GetMouseButtonUp(0))
            {
                throwPower.fillAmount = 0;
                float endTime = Time.time;
                if (endTime > startTime)
                {
                    if (endTime - startTime < 1)
                    {
                        items[itemIndex].Use(endTime - startTime + 1f);
                    }
                    else
                    {
                        if (endTime - startTime > maxHoldTime)
                        {
                            items[itemIndex].Use(maxHoldTime + 1f);
                        }
                        else
                        {
                            items[itemIndex].Use(endTime - startTime + 1f);
                        }

                    }
                }

            }

            if (Input.GetMouseButtonDown(1))
            {
                //items[itemIndex].LetGo();
            }
        }

        if (transform.position.y < -10f)
        {
            Die();
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.isPaused = !pauseMenu.isPaused;
        }
    }

    void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }
    }
    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        if (!pauseMenu.isPaused)
        {
            playerMechanics.OnFixedUpdate();
        }
    }

    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;

        itemIndex = _index;

        items[itemIndex].gameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].gameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    public void TakeDamage(float damage, Collider collider)
    {
        if (collider == headCollider)
        {
            PV.RPC("RPC_TakeDamage", RpcTarget.All, damage * 2f);

        }
        if (collider == chestCollider)
        {
            PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);

        }
        if (collider == armsCollider)
        {
            PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);

        }
        if (collider == legsCollider)
        {
            PV.RPC("RPC_TakeDamage", RpcTarget.All, damage * 0.5f);

        }
        if (collider == feetCollider)
        {
            PV.RPC("RPC_TakeDamage", RpcTarget.All, damage * 0.5f);

        }
    }
    public void SetGroundedState(bool state)
    {
        //Debug.Log("playerMechanics:" + (playerMechanics?.ToString() ?? "null"));
        if (playerMechanics != null)
            playerMechanics.SetGroundedState(state);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine)
            return;

        currentHealth -= damage;

        healthbarImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerManager.Die();
    }
}

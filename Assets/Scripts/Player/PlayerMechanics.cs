using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMechanics
    {
        private MonoBehaviour player;
        private GameObject camera;
        private PlayerAnimation PlayerAnimation { get; set; }

        private AudioSource runFootsteps;
        private Rigidbody rb;
        private Config config;

        private float verticalLookRotation;
        float mouseSensitivity;
        Vector3 smoothMoveVelocity;
        Vector3 moveAmount;
        bool grounded;
        public PlayerMechanics(MonoBehaviour _player, GameObject _camera,  Animator animatior, AudioSource runFootsteps)
        {
            config = GameObject.FindGameObjectsWithTag(Config.ConfigTag).First().GetComponent<Config>();
            rb = _player.GetComponent<Rigidbody>();
            PlayerAnimation = new PlayerAnimation(animatior);
            this.runFootsteps = runFootsteps;
            player = _player;
            camera = _camera;
        }

        public void OnUpdate()
        {
            Look();
            Move();
            Jump();
            PlayerAnimation.Animate();
        }
        public void OnFixedUpdate()
        {
            rb.MovePosition(rb.position + player.transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        }

        public void SetGroundedState(bool _grounded)
        {
            Debug.Log("Grounded:" + _grounded);
            grounded = _grounded;
        }

        void Move()
        {
            Vector3 moveDir;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDir = new Vector3(0, 0, Input.GetAxisRaw("Vertical")).normalized;

            }
            else
            {
                moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
                if (moveDir.magnitude > 0)
                {
                    runFootsteps.enabled = true;
                }
                else
                {
                    runFootsteps.enabled = false;
                }

            }

            moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? config.sprintspeed : config.walkspeed), 
                ref smoothMoveVelocity, config.SMOOTH_TIME);
        }

        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                rb.AddForce(player.transform.up * 2);
                PlayerAnimation.SetJump(true);
            }
            if (!grounded)
            {
                PlayerAnimation.SetJump(false);
            }
        }

        void Look()
        {
            player.transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

            verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

            camera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
        }
    }
}

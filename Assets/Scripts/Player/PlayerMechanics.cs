using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player
{
    public class PlayerMechanics
    {
        private MonoBehaviour player;
        private GameObject camera;
        private PlayerAnimation PlayerAnimation { get; set; }

        private Rigidbody rb;
        private Config config;

        private float verticalLookRotation;
        Vector3 smoothMoveVelocity;
        Vector3 moveAmount;
        bool grounded;
        public PlayerMechanics(MonoBehaviour _player, GameObject _camera, Animator animatior)
        {
            config = GameObject.FindGameObjectsWithTag(Config.ConfigTag).First().GetComponent<Config>();
            rb = _player.GetComponent<Rigidbody>();
            PlayerAnimation = new PlayerAnimation(animatior);
            player = _player;
            camera = _camera;
        }

        public void OnUpdate()
        {
            Look();
            Move();
            Jump();
        }
        public void OnFixedUpdate()
        {
            //rb.MovePosition(rb.position + player.transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        }

        public void SetGroundedState(bool _grounded)
        {
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

            }
            moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * config.walkspeed,
                ref smoothMoveVelocity, config.SMOOTH_TIME);
            PlayerAnimation.Animate(moveAmount);
        }

        void Jump()
        {
            PlayerAnimation.SetGrounded(grounded);
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                rb.AddForce(player.transform.up * config.JUMP_FORCE);
            }
            if (!grounded)
            {
                if (moveAmount.z > 1 || moveAmount.x != 0)
                {
                    rb.MovePosition(rb.position + player.transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
                }
                PlayerAnimation.SetJump(true);
                if (rb.velocity.y < 0)
                {
                    PlayerAnimation.SetFalling(true);
                }
            }
            else
            {
                PlayerAnimation.SetJump(false);
                PlayerAnimation.SetFalling(false);
            }
        }

        void Look()
        {
            player.transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * config.MOUSE_SENSIVITY);

            verticalLookRotation += Input.GetAxisRaw("Mouse Y") * config.MOUSE_SENSIVITY;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

            camera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
        }
    }
}
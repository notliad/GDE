using UnityEngine;

namespace Assets.Scripts.Player
{
    internal class PlayerAnimation
    {
        private Animator _animator;
        public PlayerAnimation(Animator animator)
        {
            _animator = animator;
        }
        public void Animate(Vector3 moveDir)
        {
            //_animator.SetBool("isMoving", moveDir.magnitude > 0);
            _animator.SetFloat("Horizontal", moveDir.x);
            _animator.SetFloat("Vertical", moveDir.z);
            //_animator.SetBool("RunLeft", Input.GetAxisRaw("Vertical") > 0 && Input.GetAxisRaw("Horizontal") < 0);
            //_animator.SetBool("RunRight", Input.GetAxisRaw("Vertical") > 0 && Input.GetAxisRaw("Horizontal") > 0);
            //_animator.SetBool("BackLeft", Input.GetAxisRaw("Vertical") < 0 && Input.GetAxisRaw("Horizontal") < 0);
            //_animator.SetBool("BackRight", Input.GetAxisRaw("Vertical") < 0 && Input.GetAxisRaw("Horizontal") > 0);
            //_animator.SetBool("StrafeLeft", Input.GetAxisRaw("Horizontal") < 0);
            //_animator.SetBool("RunForward", Input.GetAxisRaw("Vertical") > 0);
            //_animator.SetBool("Back", Input.GetAxisRaw("Vertical") < 0);
            //_animator.SetBool("StrafeRight", Input.GetAxisRaw("Horizontal") > 0);
            //_animator.SetBool("Sprint", Input.GetKey(KeyCode.LeftShift));
            //_animator.SetBool("ThrowingLow", Input.GetMouseButtonDown(1));
        }

        public void SetJump(bool jump)
        {
            _animator.SetBool("Jump", jump);
        }

        public void SetGrounded(bool grounded)
        {
            _animator.SetBool("isGrounded", grounded);
        }

        public void SetFalling(bool falling)
        {
            _animator.SetBool("isFalling", falling);
        }
    }
}
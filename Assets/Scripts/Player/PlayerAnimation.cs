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
        public void Animate()
        {
            _animator.SetBool("RunLeft", Input.GetAxisRaw("Vertical") > 0 && Input.GetAxisRaw("Horizontal") < 0);
            _animator.SetBool("RunRight", Input.GetAxisRaw("Vertical") > 0 && Input.GetAxisRaw("Horizontal") > 0);
            _animator.SetBool("BackLeft", Input.GetAxisRaw("Vertical") < 0 && Input.GetAxisRaw("Horizontal") < 0);
            _animator.SetBool("BackRight", Input.GetAxisRaw("Vertical") < 0 && Input.GetAxisRaw("Horizontal") > 0);
            _animator.SetBool("StrafeLeft", Input.GetAxisRaw("Horizontal") < 0);
            _animator.SetBool("RunForward", Input.GetAxisRaw("Vertical") > 0);
            _animator.SetBool("Back", Input.GetAxisRaw("Vertical") < 0);
            _animator.SetBool("StrafeRight", Input.GetAxisRaw("Horizontal") > 0);
            _animator.SetBool("Sprint", Input.GetKey(KeyCode.LeftShift));
        }

        public void SetJump(bool jump)
        {
            _animator.SetBool("Jump", jump);
        }
    }
}
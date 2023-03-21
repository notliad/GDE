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
            _animator.SetFloat("Horizontal", moveDir.x);
            _animator.SetFloat("Vertical", moveDir.z);
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

        public void SetJumpRunning(bool jumpRunning)
        {
            _animator.SetBool("JumpRunning", jumpRunning);
        }

        public void SetThrowEspada(bool throwEspada)
        {
            _animator.SetBool("Throw", throwEspada);
        }

        public void SetEquipped(bool equipped)
        {
            _animator.SetBool("Equipped", equipped);
        }

        public void SetIgnite(bool ignite)
        {
            _animator.SetBool("Ignite", ignite);
        }
    }
}
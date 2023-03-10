using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class PlayerHealthMechanics
    {
        Image healthbarImage;
        const float maxHealth = 100f;
        float currentHealth = maxHealth;

        PlayerManager playerManager;
        public PlayerHealthMechanics(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        void TakeDamage(float damage)
        {

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
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator animator;
    private bool dead;
    

    private void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            animator.SetTrigger("IsHurt");
        }
        else
        {
            if (!dead)
            {
                animator.SetTrigger("IsDead");
                animator.SetBool("isAttacking", false);
                GetComponent<EnemyAIController>().enabled = false;
                dead = true;
                Destroy(gameObject, 9f); // Destroy enemy object after 1 seconds
                GameEventsManager.instance.PlayerDeath();

            }
        }
    }
}
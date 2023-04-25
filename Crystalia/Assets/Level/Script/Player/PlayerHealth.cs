using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
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
            Debug.Log("Player Is Damaged,");
            animator.SetTrigger("IsHurt");
            animator.SetBool("IsAttacking", false);
        }
        else
        {
            if (!dead)
            {
                Debug.Log("Player Is Dead");
                animator.SetTrigger("IsDead");
                GetComponent<PlayerMotor>().enabled = false;
                GetComponent<InputManager>().enabled = false;
                dead = true;
                Invoke(nameof(RestartScene), 5f); // Restart scene after  seconds
            }
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene("EndScreen"); // Replace "EndScreen" with the name of your end screen scene
    }
}
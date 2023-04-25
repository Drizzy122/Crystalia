using UnityEngine;

public enum EnvironmentType
{
    Normal,
    Water,
    Mud
}

public class EnvironmentalEffects : MonoBehaviour
{
    private PlayerMotor motor;
    private PlayerHealth health;

    private EnemyHealth enemyHealth;
    private EnemyAIController enemyAIController;
    
    [SerializeField] private float waterSpeedModifier = 0.5f; // Slows down movement by 50%
    [SerializeField] private float mudSpeedModifier = 0.75f; // Slows down movement by 25%

    private EnvironmentType currentEnvironment = EnvironmentType.Normal;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the player or an enemy
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Check if the collider is standing on water
            if (other.CompareTag("Water"))
            {
                currentEnvironment = EnvironmentType.Water;
                ApplyEnvironmentalEffects(other.gameObject); // Pass the game object to the method
            }
            // Check if the collider is standing on mud
            else if (other.CompareTag("Mud"))
            {
                currentEnvironment = EnvironmentType.Mud;
                ApplyEnvironmentalEffects(other.gameObject); // Pass the game object to the method
            }
        }
    }

    private void ApplyEnvironmentalEffects(GameObject gameObject)
    {
        // Reduce the movement speed based on the current environment
        switch (currentEnvironment)
        {
            case EnvironmentType.Water:
                gameObject.GetComponent<PlayerMotor>().speed *= waterSpeedModifier;
                break;
            case EnvironmentType.Mud:
                gameObject.GetComponent<PlayerMotor>().speed *= mudSpeedModifier;
                break;
        }
    }
}

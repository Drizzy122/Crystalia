using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour, IDataPersistence
{
    [Header ("Player Movement")]
    bool sprinting = false;
    public float gravity = -9.8f; //add gravity 
    private bool isGrounded; //Check if the player is grounded or not
    private Vector3 playerVelocity;
    public float speed = 5f; //Set tge speed of the player


    [Header ("Player Attack")]
    private bool attacking = false;
    public float attackSpeed = 5f;
    public float attackDamage = 10f;
    public float attackRange = 1f;
    private float attackCooldown = 100f;
    public List<GameObject> EIList = new List<GameObject>();
    SphereCollider attackTrigger;

    [Header ("Player Component")]
    private CharacterController controller;
    [SerializeField] private Animator animator;

    [Header("Attributes SO")]
    [SerializeField] private AttributesScriptableObject playerAttributesSO;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        attackTrigger = gameObject.AddComponent<SphereCollider> ();
        attackTrigger.isTrigger = true;
        attackTrigger.radius = attackRange;

    }
    void Update()
    {
        isGrounded = controller.isGrounded;

        if(attackCooldown < attackSpeed)
            attackCooldown += Time.deltaTime;
    }


    public void LoadData(GameData data)
    {
       // Debug.Log("Loading player position: " + data.playerPosition.ToString());
        this.transform.position = data.playerPosition;
        playerAttributesSO.vitality = data.playerAttributesData.vitality;
        playerAttributesSO.strength = data.playerAttributesData.strength;
        playerAttributesSO.dexterity = data.playerAttributesData.dexterity;
        playerAttributesSO.intellect= data.playerAttributesData.intellect;
        playerAttributesSO.endurance = data.playerAttributesData.endurance;


    }
    public void SaveData(GameData data)
    {
       // Debug.Log("Saving player position: " + this.transform.position.ToString());
        data.playerPosition = this.transform.position;
        data.playerAttributesData.vitality = playerAttributesSO.vitality;
        data.playerAttributesData.strength = playerAttributesSO.strength;
        data.playerAttributesData.dexterity = playerAttributesSO.dexterity;
        data.playerAttributesData.intellect = playerAttributesSO.intellect;
        data.playerAttributesData.endurance = playerAttributesSO.endurance;

    }

    public void ProcessMove(Vector2 input)
    {
        // Calculate movement direction
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        // Apply rotation
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }

        // Apply movement
        controller.Move(moveDirection * speed * Time.deltaTime);
        // Set animator parameter
        animator.SetFloat("Speed", controller.velocity.magnitude);

        // Apply gravity
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 5;
            animator.SetBool("IsRunning", true);

            // add an impulse to the player's movement when they start sprinting
           // playerVelocity += transform.forward * 5f;
        }
        else
        {
            speed = 3;
            animator.SetBool("IsRunning", false);

            // reset playerVelocity when the player stops sprinting
          //  playerVelocity = Vector3.zero;
        }
    }

    public void Attack()
    {
        attacking = !attacking;

        animator.SetBool("IsAttacking", true);
        Invoke("SetAnimatorbool", .1f);

        if (attacking)
        {
            attackCooldown = 0;
            foreach (GameObject enemy in EIList)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
                print("hit");
            }
        }
    }
    void SetAnimatorbool()
    {
        animator.SetBool("IsAttacking", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            foreach (GameObject enemy in EIList)
            {
                if (enemy == other.gameObject)
                    return;
            }
            print(other.name);
            EIList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EIList.Remove(other.gameObject);
        }
    }

    

   

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); ;
    }
}
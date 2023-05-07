using UnityEngine;
using System.Collections;

public class bouncyToy : MonoBehaviour {

    public float jumpForce = 5.0f; // The force of the jump
    public float minJumpInterval = 1.0f; // The minimum time between jumps
    public float maxJumpInterval = 5.0f; // The maximum time between jumps
    private float timeToJump; // The time when the next jump should occur

    void Start(){
        // Create Physics Material
        PhysicMaterial rubber = new PhysicMaterial ("Rubber");
        // Set bounciness to 1 (max)
        rubber.bounciness = 1.0f;
        // Set friction when static
        rubber.staticFriction = 0.6f;
        // Set friction when moving
        rubber.dynamicFriction = 0.6f;
        // Update the Spheres Collider
        GetComponent<SphereCollider> ().material = rubber;

        // Set the initial time to jump
        timeToJump = Time.time + Random.Range(minJumpInterval, maxJumpInterval);
    }
    // Update is called once per frame
    void Update()
    {
        // Check if it's time to jump
        if (Time.time >= timeToJump)
        {
            // Add a force in a random direction
            Vector3 jumpDirection = new Vector3(Random.Range(-1.0f, 1.0f), 1.0f, Random.Range(-1.0f, 1.0f)).normalized;
            GetComponent<Rigidbody>().AddForce(jumpDirection * jumpForce, ForceMode.Impulse);

            // Set the time for the next jump
            timeToJump = Time.time + Random.Range(minJumpInterval, maxJumpInterval);
        }
    }
}
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 10f; // Adjust this value to control the speed of rotation.

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

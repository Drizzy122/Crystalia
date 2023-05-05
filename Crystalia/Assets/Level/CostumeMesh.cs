using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeMesh : MonoBehaviour
{
    // Set up width and height variables
    // These are required to define our vertices
    public float meshWidth = 10f;
    public float meshHeight = 10f;
    
    // Use this for initialisation
    void Start()
    {
        // Create mesh filter using GetComponent<meshfilter>
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;
        // Vertices
        Vector3[] vertices = new Vector3[4] {
new Vector3(0,0,0),new Vector3(meshWidth,0,0),new Vector3(0,meshHeight,0),new Vector3(meshWidth, meshHeight,0)
};
        // Triangles
        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;

        Vector3[] normals = new Vector3[4];
        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;
        // Update mesh with vertices, triangles and normals
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
    }
    // Update is called once per frame
    void Update()
    {
        
        // Get handle on the mesh
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        // Create temp vertices array
        Vector3[] vertices = mesh.vertices;
        // Create temp normals array
        Vector3[] normals = mesh.normals;
        // temp control int
        int i = 0;
        // Loop through vertices and modify
        while (i < vertices.Length)
        {
            // Modify vertices by adding sin and move in direction of normals
            vertices[i] += normals[i] * Mathf.Sin(Time.time);
            i++;
        }
        // Update mesh vertices
        mesh.vertices = vertices;
    }

   
}
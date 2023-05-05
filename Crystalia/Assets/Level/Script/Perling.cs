using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perling : MonoBehaviour
{
    public float scale;
    public float waveSpeed;
    public float waveHeight;


    void Update()
    {
        CalcNoise();
    }
    void CalcNoise()
    {
        // Get a reference to the Mesh Filter
        MeshFilter mF = GetComponent<MeshFilter>();
        // Create array from Mesh vertices
        Vector3[] verts = mF.mesh.vertices;
        // Iterate through vertices, multiply by scale value and add wavespeed
        //
        for (int i = 0; i < verts.Length; i++)
        {
            float pX = (verts[i].x * scale) + (Time.time * waveSpeed);
            float pZ = (verts[i].z * scale) + (Time.time * waveSpeed);
            // Update the Y component of each vertices using perlin noise
            verts[i].y = Mathf.PerlinNoise(pX, pZ) * waveHeight;
        }
        // Update the vertices
        mF.mesh.vertices = verts;
        // Recalculate normals as triangles have changed
        mF.mesh.RecalculateNormals();
        // Recalculate bounds as triangles have changed
        mF.mesh.RecalculateBounds();
    }
}


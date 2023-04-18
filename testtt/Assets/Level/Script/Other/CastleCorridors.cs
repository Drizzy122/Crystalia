using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleCorridors : MonoBehaviour
{
    [Header("Roof")]
    public MeshRenderer Ceiling = null;
    public MeshRenderer Ceiling2 = null;
    public MeshRenderer SideWall = null;
    public MeshRenderer SideWall2 = null;
    public MeshRenderer Crenellations = null;
    public MeshRenderer Crenellations2 = null;





    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {

            Ceiling.enabled = false;
            Ceiling2.enabled = false;
            SideWall.enabled = false;
            SideWall2.enabled = false;
            Crenellations.enabled = false;
            Crenellations2.enabled = false;
        }
    }

    void OnTriggerExit(Collider col)
    {
        
        Ceiling.enabled = true;
        Ceiling2.enabled = true;
        SideWall.enabled = true;
        SideWall2.enabled = true;
        Crenellations.enabled = true;
        Crenellations2.enabled = true;

    }
}
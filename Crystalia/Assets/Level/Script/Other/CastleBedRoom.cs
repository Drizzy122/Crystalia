using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBedRoom : MonoBehaviour
{
    //Small Castle part
    [Header("Roof")]
    public MeshRenderer Ceiling = null;
    public MeshRenderer SideWall = null;
   public MeshRenderer Crenellations = null;






    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            
            Ceiling.enabled = false;
            SideWall.enabled = false;
            Crenellations.enabled = false;
            
        }
    }

    void OnTriggerExit(Collider col)
    {
       
        Ceiling.enabled = true;
        SideWall.enabled = true;
        Crenellations.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Attributes", menuName = "ScriptableObjects/Attributes", order = 1)]
public class AttributesScriptableObject : ScriptableObject
{
    public int vitality;
    public int strength;
    public int dexterity;
    public int intellect;
    public int endurance;
}

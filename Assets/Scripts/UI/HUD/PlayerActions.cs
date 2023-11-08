using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerActions", menuName = "PlayerActions")]
public class PlayerActions : ScriptableObject
{
    [Space(5)]
    [Header("Units")]
    public List<GameObject> basicUnits = new List<GameObject>();
        
}


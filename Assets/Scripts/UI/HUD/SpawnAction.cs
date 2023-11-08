using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAction : MonoBehaviour
{ 
    public void OnClick()
    {
        ActionFrame.Instance.SpawnUnit(name);
    }
    
}

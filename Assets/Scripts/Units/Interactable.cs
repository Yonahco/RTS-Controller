using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isInteracting = false;
    public virtual void OnInteractEnter()
    {
        isInteracting = true;
    }
    public virtual void OnInteractExit()
    {
        isInteracting = false;
    }
    public virtual void ShowHighlight(GameObject gameObject)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
    public virtual void HideHighlight(GameObject gameObject)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}


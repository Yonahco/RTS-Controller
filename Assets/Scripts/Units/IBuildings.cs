using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IBuildings : Interactable
{
    public PlayerActions actions;

    // Start is called before the first frame update
    void Start()
    {
        UnitSelection.Instance.buildingList.Add(this.gameObject);
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        UnitSelection.Instance.buildingList.Remove(this.gameObject);
    }

    public override void OnInteractEnter()
    {
        base.OnInteractEnter();
        ActionFrame.Instance.SetActionButtons(actions);
    }

    public override void OnInteractExit()
    {
        ActionFrame.Instance.ClearActions();
        base.OnInteractExit();
    }
}

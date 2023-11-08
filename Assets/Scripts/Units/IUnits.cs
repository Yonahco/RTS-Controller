using System;
using UnityEngine;

public class IUnits : Interactable
{
    private PlayerActions actions;

    // Start is called before the first frame update
    void Start()
    {
        UnitSelection.Instance.unitList.Add(this.gameObject);
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        UnitSelection.Instance.unitList.Remove(this.gameObject);
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


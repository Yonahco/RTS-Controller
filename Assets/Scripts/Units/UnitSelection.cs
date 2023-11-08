using System;
using System.Collections.Generic;
using UnityEngine;
public class UnitSelection : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();
    
    public List<GameObject> buildingList = new List<GameObject>();
    public List<GameObject> buildingSelected = new List<GameObject>();

    private static UnitSelection _instance;
    public static UnitSelection Instance { get { return _instance; } }

    private void Awake()
    {
        // If an instance of this already exists and it isn't this one
        if (_instance != null && _instance)
        {
            // We destroy this instance
            Destroy(this.gameObject);
        }
        else
        {
            // Make this instance
            _instance = this;
        }
    }

    public void ClickSelect(GameObject unitToAdd)
    {
        DeselectAll();
        
        if (unitToAdd.GetComponent<IUnits>())
        {
            unitsSelected.Add(unitToAdd);
            // unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.gameObject.GetComponent<IUnits>().ShowHighlight(unitToAdd);
            unitToAdd.gameObject.GetComponent<IUnits>().OnInteractEnter();
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
        
        if (unitToAdd.GetComponent<IBuildings>())
        {
            buildingSelected.Add(unitToAdd);
            unitToAdd.gameObject.GetComponent<IBuildings>().ShowHighlight(unitToAdd);
            unitToAdd.gameObject.GetComponent<IBuildings>().OnInteractEnter();
            
        }
        
    }
    public void ShiftClickSelect(GameObject unitToAdd)
    {
        if (!unitsSelected.Contains(unitToAdd) && (unitToAdd.GetComponent<IUnits>())) 
        {
            unitsSelected.Add(unitToAdd);
            unitToAdd.gameObject.GetComponent<IUnits>().ShowHighlight(unitToAdd);
            unitToAdd.gameObject.GetComponent<IUnits>().OnInteractEnter();
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
        else
        {
            unitToAdd.GetComponent<UnitMovement>().enabled = false;
            unitToAdd.gameObject.GetComponent<IUnits>().HideHighlight(unitToAdd);
            unitToAdd.gameObject.GetComponent<IUnits>().OnInteractExit();
            
            unitsSelected.Remove(unitToAdd);
            
        }
    }
    public void DragSelect(GameObject unitToAdd)
    {
        if (!unitsSelected.Contains(unitToAdd))
        {
                unitsSelected.Add(unitToAdd);
                unitToAdd.gameObject.GetComponent<IUnits>().ShowHighlight(unitToAdd);
                unitToAdd.gameObject.GetComponent<IUnits>().OnInteractEnter();
                unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in unitsSelected)
        {
            // Disable Unit Scripts and Objects
            unit.GetComponent<UnitMovement>().enabled = false;
            unit.gameObject.GetComponent<IUnits>().OnInteractExit();
            unit.gameObject.GetComponent<IUnits>().HideHighlight(unit);
        }
        unitsSelected.Clear();
        
        foreach (var building in buildingSelected)
        {
            // Disable Building Scripts and Objects
            building.gameObject.GetComponent<IBuildings>().OnInteractExit();
            building.gameObject.GetComponent<IBuildings>().HideHighlight(building);
        }
        buildingSelected.Clear();
        ActionFrame.Instance.ClearActions();
    }

    public void Deselect(GameObject unitToDeselect)
    {
        
    }
    
}


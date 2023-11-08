using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionFrame : MonoBehaviour
    {
        // Instance
        private static ActionFrame _instance;
        public static ActionFrame Instance { get { return _instance; } }
    
        [SerializeField] private Button actionButton = null;
        [SerializeField] private Transform layoutGroup = null;
    
        private List<Button> buttons = new List<Button>();
        private PlayerActions actionsList = null;
    
        public List<GameObject> spawnQueue = new List<GameObject>();
    

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

        public void SetActionButtons(PlayerActions actions)
        {
            actionsList = actions;

            // When we have units with actions tied to them this will be used, as for now NO
            // if (UnitSelection.Instance.unitsSelected.Count > 0)
            // {
            //     Button btn = Instantiate(actionButton, layoutGroup);
            //     //When we start having Unit Prefabs
            //     //btn.name = unit.name;
            //     buttons.Add(btn);
            // }
        
            if (UnitSelection.Instance.buildingSelected.Count > 0)
            {
                if (actions.basicUnits.Count > 0)
                {
                    foreach (GameObject unit in actions.basicUnits)
                    {
                        Button btn = Instantiate(actionButton, layoutGroup);
                        btn.name = unit.name;
                        buttons.Add(btn);
                    }
                }
            }
        
        }
        public void ClearActions()
        {
            foreach (Button btn in buttons)
            {
                Destroy(btn.gameObject);
            }
            buttons.Clear();
        }
    
        private GameObject IsUnit(string name)
        {
            if (actionsList.basicUnits.Count > 0)
            {
                foreach(GameObject unit in actionsList.basicUnits)
                {
                    if (unit.name == name)
                    {
                        return unit;
                    }
                }
            }
            return null;
        }
    
        public void SpawnUnit(string objectToSpawn)
        {
            if (IsUnit(objectToSpawn))
            {
                Debug.Log("Spawned Unit via OnClick");
                GameObject unit = IsUnit(objectToSpawn);
                // Grab Spawn Point Transform Child from Building Prefab
                var building = UnitSelection.Instance.buildingSelected[0];
                var spawnPoint = building.transform.GetChild(1).gameObject;
        
                Instantiate(unit, new Vector3(spawnPoint.transform.position.x - 4,
                    spawnPoint.transform.position.y, spawnPoint.transform.position.z), Quaternion.identity);
            }


        }
    }


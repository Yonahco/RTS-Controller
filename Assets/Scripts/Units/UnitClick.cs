using UnityEngine;
using UnityEngine.EventSystems;

public class UnitClick : MonoBehaviour
{
    private Camera myCam;
    public GameObject groundMarker;

    public LayerMask clickable;
    public LayerMask floor;
    
    void Start()
    {
        myCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                // If we hit a clickable Object
                
                // Normal Click and Shift Click
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    // Shift Clicked
                    UnitSelection.Instance.ShiftClickSelect(hit.collider.gameObject);
                }
                else
                {
                    // Normal Clicked
                    UnitSelection.Instance.ClickSelect(hit.collider.gameObject);
                }
            }
            else
            { 
                // if we DONT hit a clickable Object and not SHIFT clicking
                if (!Input.GetKey(KeyCode.LeftShift) && !(EventSystem.current.IsPointerOverGameObject()))
                {
                    UnitSelection.Instance.DeselectAll();
                }
            }
           
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, floor))
            {
                groundMarker.transform.position = hit.point;
                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }
        }
    }
}

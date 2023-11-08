using UnityEngine;

public class UnitDrag : MonoBehaviour
{
    private Camera myCam;
    // Graphical
    [SerializeField] RectTransform boxVisual;
    
    // Logical
    Rect selectionBox;
    Vector2 startPosition;
    Vector2 endPosition;
    
    void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            selectionBox = new Rect();
        }

        if (Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectUnits();
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }
    }

    void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        // do X calculations
        if (Input.mousePosition.x < startPosition.x)
        {
            // Dragging Left
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            // Dragging Right
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }
        // do Y calculations
        if (Input.mousePosition.y < startPosition.y)
        {
            // Dragging Left
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            // Dragging Right
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    void SelectUnits()
    {
        // Loop through all the units
        foreach (var unit in UnitSelection.Instance.unitList)
        {
            // If unit is within the bounds of the selection box/rect
            if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position)))
            {
                // If an unit is within the selection add them to the selection
                UnitSelection.Instance.DragSelect(unit);
            }
        }
    }
}

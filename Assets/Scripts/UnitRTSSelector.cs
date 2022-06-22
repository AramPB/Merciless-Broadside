using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils; //PACKAGE EXTERN!!!!!!

public class UnitRTSSelector : MonoBehaviour
{

    public RectTransform selectorImage;

    private Rect selectionRect;

    private Vector2 startPos;
    private Vector2 endPos;

    private bool isDown = false;

    [SerializeField]
    private FollowSelecteds followSelecteds;



    // Start is called before the first frame update
    void Start()
    {
        DrawRectangle();
    }

    // Update is called once per frame
    void Update()
    {
        #region SelectionInputs
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUIIgnores())
        {
            //Left Mouse Button Pressed

            startPos = Input.mousePosition;

            selectionRect = new Rect();

            isDown = true;
        }

        if (Input.GetMouseButton(0) && isDown)
        {
            //Left Mouse Button Held Down

            endPos = Input.mousePosition;

            DrawRectangle();

            //RECTANGLE FOR SELECTION
            //X
            if(Input.mousePosition.x < startPos.x)
            {
                selectionRect.xMin = Input.mousePosition.x;
                selectionRect.xMax = startPos.x;
            }
            else
            {
                selectionRect.xMin = startPos.x;
                selectionRect.xMax = Input.mousePosition.x;
            }
            //Y
            if (Input.mousePosition.y < startPos.y)
            {
                selectionRect.yMin = Input.mousePosition.y;
                selectionRect.yMax = startPos.y;
            }
            else
            {
                selectionRect.yMin = startPos.y;
                selectionRect.yMax = Input.mousePosition.y;
            }
        }

        if (Input.GetMouseButtonUp(0) && isDown)
        {
            //Left Mouse Button Released

            isDown = false;

            DeactivateAllUnits();

            CheckSelectedUnits();

            startPos = Vector2.zero;
            endPos = Vector2.zero;

            DrawRectangle();

        }
        #endregion

        #region MovingInputs
        if (Input.GetMouseButtonDown(1) && !IsMouseOverUIIgnores() && SelectionManager.unitRTsList.Count > 0)
        {
            //TODO?: potser un if de que comprovi si clica en area valida o no
            //depenent si es valida mostrar fletxes valides/ i si no invalides
            //tmb es pot detectar si hi ha enemic per atacar

            int quantitySelected = SelectionManager.unitRTsList.Count;

            foreach (UnitRTS unit in SelectionManager.unitRTsList)
            {
                if (unit.IsSelected()) {
                    //Vector3 margins = unit.GetMargins(); TODO: calcular amb els marges la posicio en formacio de la unitat especifica

                    unit.Move();
                }
            }
            
        }
        #endregion
    }
    private void DrawRectangle()
    {
        Vector2 boxStart = startPos;
        Vector2 boxCenter = (boxStart + endPos) / 2;

        selectorImage.position = boxCenter;

        float sizeX = Mathf.Abs(boxStart.x - endPos.x);
        float sizeY = Mathf.Abs(boxStart.y - endPos.y);

        selectorImage.sizeDelta = new Vector2(sizeX, sizeY);
    }

    private void CheckSelectedUnits()
    {
        foreach(UnitRTS unit in SelectionManager.unitRTsList)
        {
            if (selectionRect.Contains(Camera.main.WorldToScreenPoint(unit.transform.position)))
            {
                unit.SetSelectedVisible(true);
            }
            if (unit.GetClicked())
            {
                unit.SetSelectedVisible(true);
                unit.SetClicked(false);
            }
        }
        followSelecteds.NewSelection();
    }

    private void DeactivateAllUnits()
    {
        foreach(UnitRTS unit in SelectionManager.unitRTsList)
        {
            unit.SetSelectedVisible(false);
        }
    }

    private bool IsMouseOverUIIgnores()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultsList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);
        for (int i = 0; i < raycastResultsList.Count; i++)
        {
            if (raycastResultsList[i].gameObject.GetComponent<MouseUIClickThrough>() != null)
            {
                raycastResultsList.RemoveAt(i);
                i--;
            }
        }
        return raycastResultsList.Count > 0;
    }
}

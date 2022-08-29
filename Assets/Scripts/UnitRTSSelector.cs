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

    private bool isDown = false,
        isCommand = false,
        enemySelected = false,
        unitsSelected = false;

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
        //if (Input.GetMouseButtonDown(0) && !IsMouseOverUIIgnores())
        if (Input.GetButtonDown(Constants.SELECTION) && !IsMouseOverUIIgnores())
        {
            //Left Mouse Button Pressed

            startPos = Input.mousePosition;

            selectionRect = new Rect();

            isDown = true;
        }

        //if (Input.GetMouseButton(0) && isDown)
        if (Input.GetButton(Constants.SELECTION) && isDown)
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

        //if (Input.GetMouseButtonUp(0) && isDown)
        if (Input.GetButtonUp(Constants.SELECTION) && isDown)
        {
            //Left Mouse Button Released

            isDown = false;

            
            if (!isCommand && unitsSelected && !CheckIfSelectedEnemy()) {
                DeactivateAllUnits();
            }
            CheckSelectedUnits();

            if ((!unitsSelected && enemySelected) || CheckIfSelectedEnemy()) {
                DeactivateAllEnemies();
            }

            CheckSelectedEnemy();

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
            bool toAttack = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit))
            {
                //Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.gameObject.GetComponent<ShipMovement>())
                {
                    ShipMovement sm = hit.transform.gameObject.GetComponent<ShipMovement>();
                    if (sm.isEnemy)
                    {
                        toAttack = true;
                        foreach (UnitRTS unit in SelectionManager.unitRTsList)
                        {
                            if (unit.IsSelected())
                            {
                                unit.Attack(hit.transform.gameObject);
                            }
                        }
                    }
                    //TODO: else maybe algo de si clica a aliats
                }
            }

            if (!toAttack) {

                int quantitySelected = SelectionManager.unitRTsList.Count;

                foreach (UnitRTS unit in SelectionManager.unitRTsList)
                {
                    if (unit.IsSelected()) {
                        //Vector3 margins = unit.GetMargins(); TODO: calcular amb els marges la posicio en formacio de la unitat especifica

                        unit.Move();
                    }
                }
            }
        }
        #endregion

        #region CommandInputs
        if (Input.GetButtonDown(Constants.COMMAND))
        {
            isCommand = true;
        }
        if (Input.GetButtonUp(Constants.COMMAND))
        {
            isCommand = false;
        }
        #endregion

        //TMP?
        /*#region AttackButton
        if (Input.GetButtonDown(Constants.BONUS))
        {
            foreach (UnitRTS unit in SelectionManager.unitRTsList)
            {
                if (unit.IsSelected())
                {
                    unit.Attack();
                }
            }
        }
        #endregion*/
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
                unitsSelected = true;
            }
            if (unit.GetClicked())
            {
                unit.SetSelectedVisible(true);
                unit.SetClicked(false);
                unitsSelected = true;
            }
        }
        followSelecteds.NewSelection();
    }

    private bool CheckIfSelectedEnemy()
    {
        foreach (UnitRTS enemy in SelectionManager.unitRTsEnemyList)
        {
            if (enemy.GetClicked())
            {
                return true;
            }
        }
        return false;
    }

    private void CheckSelectedEnemy()
    {
        foreach (UnitRTS enemy in SelectionManager.unitRTsEnemyList)
        {
            if (enemy.GetClicked())
            {
                enemy.SetSelectedVisible(true);
                enemy.ChangeEnemyColorSelection();
                enemy.SetClicked(false);
                enemySelected = true;
            }
        }
    }

    private void DeactivateAllUnits()
    {
        foreach(UnitRTS unit in SelectionManager.unitRTsList)
        {
            unit.SetSelectedVisible(false);
        }
        unitsSelected = false;
    }

    private void DeactivateAllEnemies()
    {
        foreach (UnitRTS enemy in SelectionManager.unitRTsEnemyList)
        {
            enemy.SetSelectedVisible(false);
        }
        enemySelected = false;
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

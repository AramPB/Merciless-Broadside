using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            bool outMap = false;
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
            else
            {
                outMap = true;
            }

            if (!toAttack) {

                int quantitySelected = SelectionManager.NumberSelecteds();

                int pos = 0;
                //Debug.Log("1:  " + quantitySelected);
                Vector3 newPos = Vector3.zero;
                foreach (UnitRTS unit in SelectionManager.unitRTsList)
                {
                    if (unit.IsSelected()) {
                        //Debug.Log("pos:" + pos);
                        newPos = CalculateNewPos(unit, pos, quantitySelected, hit.point, newPos);
                        //Vector3 margins = unit.GetMargins(); TODO: calcular amb els marges la posicio en formacio de la unitat especifica

                        if (GameManager._instance.inPlacement)
                        {
                            Vector3 size = GameManager._instance.playerSpawnAera.GetComponent<BoxCollider>().size;
                            Vector3 center = GameManager._instance.playerSpawnAera.GetComponent<BoxCollider>().center;
                            if (center.x - size.x / 2 < newPos.x
                                && center.x + size.x / 2 > newPos.x
                                && center.z - size.z / 2 < newPos.z
                                && center.z + size.z / 2 > newPos.z)
                            {
                                unit.transform.position = new Vector3(newPos.x, 0, newPos.z);
                            }
                            else
                            {
                                //outBounds
                            }
                        }
                        else
                        {
                            if (!outMap) {
                                unit.Move(newPos);
                            }
                        }
                        pos++;
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
        if (Input.GetButtonDown(Constants.TAB))
        {
            foreach (UnitRTS unit in SelectionManager.unitRTsList)
            {
                unit.SetUnitPanelActive(true);
            }
            foreach (UnitRTS unit in SelectionManager.unitRTsEnemyList)
            {
                unit.SetUnitPanelActive(true);
            }
        }
        if (Input.GetButtonUp(Constants.TAB))
        {
            foreach (UnitRTS unit in SelectionManager.unitRTsList)
            {
                unit.SetUnitPanelActive(false);
            }
            foreach (UnitRTS unit in SelectionManager.unitRTsEnemyList)
            {
                unit.SetUnitPanelActive(false);
            }
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



    private Vector3 CalculateNewPos(UnitRTS unit, int pos, int quantity, Vector3 hit, Vector3 ansPos)
    {
        float max;

        ShipMovement sm = unit.GetShipMovement();
        max = Mathf.Max(sm.GetMargins().x, sm.GetMargins().z);

        int quad = Mathf.RoundToInt(Mathf.Sqrt(quantity));
        int quad2 = quantity / quad;
        //Debug.Log("atkfhfdjhdj: " + quantity + "/" + Mathf.Sqrt(quantity) + "/" + quad);
        int others = quantity - quad * quad2;
        //Debug.Log("POS: " + pos +"/QUAD: " +quad);
        if (pos < quad * quad2 || (quantity == 1 || quantity == 2 || quantity == 3)) {
            switch (quantity)
            {
                case 1:

                    return hit;
                case 2:
                    if (pos == 0)
                    {
                        return new Vector3(hit.x - max / 4, 0, hit.z);
                    }
                    else
                    {
                        if (pos == 1)
                        {
                            return new Vector3(hit.x + max / 4, 0, hit.z);
                        }
                    }
                    break;
                case 3:
                    if (pos == 0)
                    {
                        return new Vector3(hit.x - max / 4, 0, hit.z);
                    }
                    else
                    {
                        if (pos == 1)
                        {
                            return new Vector3(hit.x + max / 4, 0, hit.z);
                        }
                        else
                        {
                            if (pos == 2)
                            {
                                return new Vector3(hit.x, 0, hit.z + max);
                            }
                        }
                    }
                    break;
                default:
                    if (quad % 2 == 0)//Par
                    {

                        if (pos == 0)
                        {
                            //Debug.Log("hitposss (0)" + new Vector3(hit.x - max / 4, 0, hit.z));
                            return new Vector3(hit.x - max / 4, 0, hit.z);
                        }
                        else
                        {
                            pos += 1;
                            int column = pos % quad;
                            float x;
                            float z;
                            bool careEquals = false;
                            if ((quad / 2) + 1 == quad && column == 0)
                            {
                                careEquals = true;
                            }
                            //Debug.Log("column:" + column + "/" + pos + "/" + quad);
                            //X pos
                            if (column != (quad / 2) + 1 && !careEquals) {
                                if (column != 0) {
                                    if (column != 1) {
                                        if (column <= quad / 2)//left
                                        {
                                            x = ansPos.x - max / 2;
                                        }
                                        else//right
                                        {
                                            x = ansPos.x + max / 2;
                                        }
                                    }
                                    else
                                    {
                                        x = hit.x - max / 4; //if 1 column L
                                    }
                                }
                                else
                                {
                                    x = ansPos.x + max / 2; //if last row
                                }
                            }
                            else
                            {
                                x = hit.x + max / 4; //if 1 column R
                            }
                            pos -= 1;
                            int row = pos / quad;
                            //Y pos
                            if (row != 0)
                            {
                                if (pos % quad == 0) {
                                    z = ansPos.z - max;
                                }
                                else
                                {
                                    z = ansPos.z;
                                }
                            }
                            else
                            {
                                z = hit.z;
                            }
                            //Debug.Log(new Vector3(x, 0, z));
                            return new Vector3(x, 0, z);
                        }

                    }
                    else //Impar
                    {

                        if (pos == 0)
                        {
                            //Debug.Log(new Vector3(hit.x, 0, hit.z));
                            return new Vector3(hit.x, 0, hit.z);
                        }
                        else
                        {
                            pos += 1;
                            int column = pos % quad;
                            float x;
                            float z;
                            bool careEquals = false;
                            if (((quad + 1) / 2) + 1 == quad && column == 0)
                            {
                                careEquals = true;
                            }
                            //X pos
                            if (column != ((quad + 1) / 2) + 1 && !careEquals)
                            {
                                if (column != 0)
                                {
                                    if (column != 1)
                                    {
                                        if (column <= (quad + 1) / 2)//left
                                        {
                                            x = ansPos.x - max / 2;

                                        }
                                        else//right
                                        {
                                            x = ansPos.x + max / 2;

                                        }
                                    }
                                    else
                                    {
                                        x = hit.x; //if 1 column L

                                    }
                                }
                                else
                                {
                                    x = ansPos.x + max / 2; //if last row

                                }
                            }
                            else
                            {
                                x = hit.x + max / 2; //if 1 column R

                                
                            }
                            pos -= 1;
                            int row = pos / quad;
                            //Y pos
                            if (row != 0)
                            {
                                if (pos % quad == 0)
                                {
                                    z = ansPos.z - max;
                                }
                                else
                                {
                                    z = ansPos.z;
                                }
                            }
                            else
                            {
                                z = hit.z;
                            }
                            //Debug.Log(new Vector3(x, 0, z));
                            return new Vector3(x, 0, z);
                        }
                    }
            }
        }
        else//rest
        {

            float x;
            float z;
            pos += 1;
            int relativePos = pos - quad * quad2;
            if (others%2 == 0)//par
            {
                //X pos

                if (relativePos != 1)
                {
                    if (relativePos != (others / 2) + 1)
                    {
                        if (relativePos <= (others / 2))//left
                        {
                            x = ansPos.x - max / 2;
                        }
                        else//right
                        {
                            x = ansPos.x + max / 2;
                        }
                    }
                    else
                    {
                        x = hit.x + max / 4;//if 1 R rest
                    }
                }
                else
                {
                    x = hit.x - max / 4;//if 1 rest
                }
                //Y pos
                if (relativePos != 1)
                {
                    z = ansPos.z;
                }
                else
                {
                    z = ansPos.z - max;
                }
            }
            else//impar
            {
                //X pos
                if (relativePos != 1)
                {
                    if (relativePos != ((others + 1) / 2) + 1)
                    {
                        if (relativePos <= ((others + 1) / 2))//left
                        {
                            x = ansPos.x - max / 2;
                        }
                        else//right 
                        {
                            x = ansPos.x + max / 2;
                        }
                    }
                    else
                    {
                        x = hit.x + max / 2;//if 1 R rest
                    }
                }
                else
                {
                    x = hit.x;//if 1 rest
                }
                //Y pos
                if (relativePos != 1)
                {
                    z = ansPos.z;
                }
                else
                {
                    z = ansPos.z - max ;
                }
            }
            //Debug.Log(new Vector3(x, 0, z));
            return new Vector3(x, 0, z);
        }
        return Vector3.zero;
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

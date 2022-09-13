using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class UnitRTS : MonoBehaviour
{
    public GameObject selectedGameObject;
    private Renderer renderer;

    [SerializeField]
    private ShipMovement sm;

    private bool clicked;
    private bool imSelected = false;
    private bool hasRenderer = false;

    [SerializeField]
    private GameObject unitPanel;

    private UnitUIPanelController unitUIPanel;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        sm = GetComponent<ShipMovement>();
        if (renderer != null)
        {
            hasRenderer = true;
        }
        //selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);


    }

    private void Update()
    {
        if (!IsEnemy())
        {
            if (unitUIPanel.hasToChange)
            {
                SetSelectedVisible(unitUIPanel.IsSelected());
            }
        }
    }

    public void AddUnit()
    {
        if (sm.isEnemy)
        {
            SelectionManager.unitRTsEnemyList.Add(this);
        }
        else
        {
            SelectionManager.unitRTsList.Add(this);
        }
    }

    public void SetUnitUIPanel(UnitUIPanelController ui)
    {
        unitUIPanel = ui;
    }

    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
        imSelected = visible;
        if (!IsEnemy()) {
            unitUIPanel.SetSelected(visible);
        }
        sm.SelectionChange(visible);
    }

    //TMP
    public void ChangeEnemyColorSelection()
    {
        selectedGameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    public ShipMovement GetShipMovement()
    {
        return sm;
    }

    private void OnMouseEnter()
    {
        if (hasRenderer)
        {
            renderer.material.color = Color.green;
        }
    }

    private void OnMouseExit()
    {
        if (hasRenderer)
        {
            renderer.material.color = Color.white;
        }

    }

    public void OnMouseDown()
    {
        //CameraController.instance.followTransform = transform;

        SetClicked(true);
    }

    public void SetClicked(bool click)
    {
        clicked = click;
    }

    public bool GetClicked()
    {
        return clicked;
    }

    public bool IsSelected()
    {
        return imSelected;
    }

    public bool IsEnemy()
    {
        if (sm != null)
        {
            return sm.isEnemy;
        }
        else
        {
            Debug.Log(transform.name + " has not ShipMovement");
            return false;
        }
    }

    public void Move(Vector3 hitPoint)
    {
        if (sm != null)
        {
            sm.MoveShip(hitPoint);
        }
        else
        {
            Debug.Log(transform.name + " has not ShipMovement");
        }
    }

    public void Attack(GameObject target)
    {
        if (sm != null)
        {
            sm.AttackShip(target);
        }
        else
        {
            Debug.Log(transform.name + " has not ShipMovement");
        }
    }

    public void GetDamage(int damage)
    {
        if (sm != null)
        {
            if (!sm.isSink) {

                if (sm.CurrentHealth() - damage <= 0)
                {
                    if (sm.isEnemy)
                    {
                        SelectionManager.unitRTsEnemyList.RemoveAt(SelectionManager.unitRTsEnemyList.IndexOf(this));
                    }
                    else
                    {
                        unitUIPanel.SetHealth(0);
                        unitUIPanel.Die();
                        unitPanel.SetActive(false);//avoid buggs
                        SelectionManager.unitRTsList.RemoveAt(SelectionManager.unitRTsList.IndexOf(this));
                    }
                }
                else
                {
                    if (!sm.isEnemy) {
                        unitUIPanel.SetHealth(sm.CurrentHealth() - damage);
                    }
                }

                sm.GetDamage(damage);
            }
            else
            {
                Debug.Log("Already ded");
            }
        }
        else
        {
            Debug.Log(transform.name + " has not ShipMovement");
        }
    }

    public Vector3 GetMargins()
    {
        if (sm != null)
        {
            return sm.GetMargins();
        }
        else
        {
            Debug.Log(transform.name + " has not ShipMovement");
            return Vector3.zero;
        }
    }

    public void SetUnitPanelActive(bool activate)
    {

         unitPanel.SetActive(activate);

    }
}

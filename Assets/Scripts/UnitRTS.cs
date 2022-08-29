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
        if (sm.isEnemy)
        {
            SelectionManager.unitRTsEnemyList.Add(this);
        }
        else
        {
            SelectionManager.unitRTsList.Add(this);
        }

    }

    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
        imSelected = visible;
    }

    //TMP
    public void ChangeEnemyColorSelection()
    {
        selectedGameObject.GetComponent<Renderer>().material.color = Color.red;
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

    public void Move()
    {
        if (sm != null)
        {
            sm.MoveShip();
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
}

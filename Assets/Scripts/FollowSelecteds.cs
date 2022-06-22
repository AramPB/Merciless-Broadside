using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class FollowSelecteds : MonoBehaviour
{
    private bool locked;

    public static FollowSelecteds followSelecteds;

    [SerializeField]
    private GameObject focusSelected;

    [SerializeField]
    private Image lockedIndicator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (locked)
        {
            focusSelected.transform.position = UpdateFollow();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (locked)
            {
                locked = false;
                lockedIndicator.color = Color.red;
                CameraController.instance.followTransform = null;
            }
            else
            {
                locked = true;
                lockedIndicator.color = Color.green;
                CameraController.instance.followTransform = focusSelected.transform;
            }
        }
    }

    private Vector3 UpdateFollow()
    {
        Vector3 center = new Vector3(0, 0, 0);
        int count = 0;
        foreach(UnitRTS unit in SelectionManager.unitRTsList)
        {
            if (unit.IsSelected()) {
                center += unit.transform.position;
                count++;
            }
        }
        if (count == 0)
        {
            CameraController.instance.followTransform = null;
            return center;
        }
        return center / count;
    }

    public void NewSelection()
    {
        if (locked) {
            CameraController.instance.followTransform = focusSelected.transform;
            focusSelected.transform.position = UpdateFollow();
        }
    }
}

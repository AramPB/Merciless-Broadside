using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager _instance;

    [SerializeField]
    private Texture2D selection,
        attack;

    private Vector2 cursorHotspot;

    private int mode = Constants.SELECTION_CURSOR;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public void Selection()
    {
        cursorHotspot = new Vector2(0, 0);
        mode = Constants.SELECTION_CURSOR;
        Cursor.SetCursor(selection, cursorHotspot, CursorMode.Auto);
    }

    public void Attack()
    {
        cursorHotspot = new Vector2(0, 0);
        mode = Constants.ATTACK_CURSOR;
        Cursor.SetCursor(attack, cursorHotspot, CursorMode.Auto);
    }

    public int Mode()
    {
        return mode;
    }
}

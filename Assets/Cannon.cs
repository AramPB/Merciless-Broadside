using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    private bool isLoaded;

    private bool isActive;

    private float loadStartTime;

    private float loadDuration;

    //private Vector3 firePosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLoaded)
        {
            if (Time.time >= loadStartTime + loadDuration)
            {
                isLoaded = true;
            }
        }
    }

    public void SetCannon(float loadDuration, bool isLoaded = true, bool isActive = true)
    {
        this.loadDuration = loadDuration;
        this.isLoaded = isLoaded;
        this.isActive = isActive;
    }

    public void FireCannon()
    {
        isLoaded = false;
        loadStartTime = Time.time;
        //Debug.Log(loadDuration);
    }

    public bool IsLoaded()
    {
        return isLoaded;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
}

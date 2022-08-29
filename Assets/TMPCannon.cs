using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMPCannon : MonoBehaviour
{
    private Vector3 startPos = new Vector3(100, 20, -100);
    private Vector3 endPos = new Vector3(0, 0, 0);
    private float velocity = 450;
    private float grade;
    private float startTime = 0;
    private bool start = false;
    private bool move = false;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(MathParabola.MaxDistance(450,5)); ;
    }

    // Update is called once per frame
    void Update()//maybe FIXED?? TODO
    {
        if (Input.GetKey("l"))
        {
            ReStart();
        }
        if (start) {
            transform.position = MathParabola.GetPosParabola2(startPos, endPos, velocity, Time.time - startTime, grade);
        }
        if (transform.position.y < 0)
        {   
            start = false;
            //Debug.Log(transform.position);
        }/*
        if (move)
        {
            transform.position = MathParabola.GetPosParabola(startPos, endPos, velocity, Time.time - startTime, grade);
        }
        if (transform.position.y < 0)
        {
            Destroy();
        }*/
    }

    private void ReStart()
    {
        //Debug.Log("EI");
        start = true;
        transform.position = startPos;
        startTime = Time.time;
        var g = MathParabola.GetValues(startPos, endPos, velocity,5);
        grade = g.grade;
    }
}

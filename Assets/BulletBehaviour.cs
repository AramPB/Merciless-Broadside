using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Vector3 startPos = new Vector3(68f, 0, 88);
    private Vector3 endPos = new Vector3(0, 0, 0);
    private float velocity = 350;
    private float grade;
    private float startTime = 0;
    private bool start = false;
    private bool move = false;

    private void OnEnable()
    {
        Invoke("Destroy", 10f);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(MathParabola.MaxDistance(35));
    }

    // Update is called once per frame
    void Update()//maybe FIXED?? TODO
    {
        /*if (Input.GetKey("l"))
        {
            ReStart();
        }
        if (start) {
            transform.position = MathParabola.GetPosParabola(startPos, endPos, velocity, Time.time - startTime, grade);
        }
        if (transform.position.y < 0)
        {   
            start = false;
            //Debug.Log(transform.position);
        }*/
        if (move)
        {
            transform.position = MathParabola.GetPosParabola2(startPos, endPos, velocity, Time.time - startTime, grade);
        }
        if (transform.position.y < 0)
        {
            Destroy();
        }
    }
    /*
    private void ReStart()
    {
        Debug.Log("EI");
        start = true;
        transform.position = startPos;
        startTime = Time.time;
        var g = MathParabola.GetValues(startPos, endPos, velocity);
        grade = g.grade;
    }
    */
    public void SetStats(Vector3 start, Vector3 end, float speed, float maxAngle = 45)
    {
        startPos = start;
        endPos = end;
        velocity = speed;

        transform.position = startPos;
        startTime = Time.time;
        var g = MathParabola.GetValues(startPos, endPos, velocity, maxAngle);
        grade = g.grade;

        move = true;
        Debug.Log($"{startPos} , {endPos} , {velocity} , {grade} , {startTime}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform)
        {

        }
    }

    private void Destroy()
    {
        move = false;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}

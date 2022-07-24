using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Vector3 startPos = new Vector3(68.1f, 0, 88.1f);
    private float grade;
    private float startTime = 0;
    private bool start = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("l"))
        {
            ReStart();
        }
        if (start) {
            transform.position = MathParabola.GetPosParabola(startPos, Vector3.zero, 350, Time.time - startTime, grade);
        }
        if (transform.position.y < 0)
        {
            start = false;
        }
    }

    private void ReStart()
    {
        Debug.Log("EI");
        start = true;
        transform.position = startPos;
        startTime = Time.time;
        var g = MathParabola.GetValues(startPos, Vector3.zero, 350);
        grade = g.grade;
    }
}

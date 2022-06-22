using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private Vector3 dir;

    [SerializeField]
    private Vector3 windDir;

    [SerializeField]
    private float magnitude,
        changeWindDuration;

    //[SerializeField]
    //private GameObject testAngle;

    private float changeWindStartTime;

    // Start is called before the first frame update
    void Start()
    {
        dir.x = transform.localEulerAngles.x;
        windDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        changeWindStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //TMP (GameManager TODO en Update)
        if (Time.time >= changeWindStartTime + changeWindDuration)
        {
            windDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            changeWindStartTime = Time.time;
        }

        windDir.Normalize();
        float angle = Vector3.Angle(windDir, Vector3.forward);
        Vector3 cross = Vector3.Cross(windDir, Vector3.forward);
        if (cross.y < 0) angle = -angle;
        //Debug.Log("mew : " + angle);

        dir.z = player.eulerAngles.y + 90 + angle; //+90 per la fletxa torssada
        transform.localEulerAngles = dir;

        /*

        float test = Vector3.Angle(testAngle.transform.forward, windDir);
        if (test < 45)
        {
            Debug.Log("Close!");
        }*/
    }

    public Vector3 GetWindDir()
    {
        return windDir;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, windDir*magnitude);
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(Vector3.zero, Vector3.forward * magnitude);
        //Gizmos.DrawRay(testAngle.transform.position, testAngle.transform.forward * magnitude);
    }
}

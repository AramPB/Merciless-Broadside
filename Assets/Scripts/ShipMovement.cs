using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;


public class ShipMovement : MonoBehaviour
{

    //public Camera cam;

    public NavMeshAgent agent;

    public int cost, attack, health;

    public float windAngle, windFaced, windNothing, windFavour;

    private Vector3 windDir;

    [SerializeField]
    private BoxCollider margins;

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = windNothing;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(1) && !IsMouseOverUIIgnores())
        {
            Ray rayCam = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if(Physics.Raycast(rayCam, out hit))
            {
                //move
                agent.SetDestination(hit.point);
            }
        }*/
        windDir = GameManager._instance.GetWind();
        float test = Vector3.Angle(transform.forward, windDir);
        if (test < windAngle)
        {
            agent.speed = windFavour;
            Debug.Log($"Favour! {transform.name} -> {windAngle} and {windDir}");
        }else if (test > 180-windAngle)
        {
            Debug.Log($"Faced! {transform.name} -> {windAngle} and {windDir}");
            agent.speed = windFaced;
        }
        else
        {
            Debug.Log($"Nothing! {transform.name} -> {windAngle} and {windDir}");
            agent.speed = windNothing;
        }

    }

    public void MoveShip()
    {
        Ray rayCam = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(rayCam, out hit))
        {
            //move
            agent.SetDestination(hit.point);
        }


    }

    public Vector3 GetMargins()
    {
        return margins.size;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        //Gizmos.DrawLine(new Vector3);
        Gizmos.DrawWireCube(transform.position + margins.center, margins.size);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 20);
    }
}

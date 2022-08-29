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

    public float cannonPower, maxAngle;

    public bool isEnemy;

    private Vector3 windDir;

    [SerializeField]
    private BoxCollider margins;

    private FireBalls FB;

    [SerializeField]
    private Transform shootPoint;

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = windNothing;
        //Debug.Log(transform.name + "//" + MathParabola.MaxDistance(100));
        FB = transform.GetComponent<FireBalls>();
        //var g = MathParabola.GetValues(transform.position, Vector3.zero, 350);
        //MathParabola.GetPosParabola(transform.position, Vector3.zero, 350, 0.1f, g.grade);
        //MathParabola.GetValues(transform.position, new Vector3(12500f, 0, 0), 350);
        //MathParabola.GetValues(new Vector3(20,3,0), Vector3.zero, 27.77f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("l"))
        {
            if (FB != null)
            {
                FB.Fire(shootPoint.position, new Vector3(2000,0,0), cannonPower, maxAngle);
                //Debug.Log(MathParabola.GetValues(Vector3.zero, new Vector3(80f, 10, 0), 30));
            }
        }
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
            //Debug.Log($"Favour! {transform.name} -> {windAngle} and {windDir}");
        }else if (test > 180-windAngle)
        {
            //Debug.Log($"Faced! {transform.name} -> {windAngle} and {windDir}");
            agent.speed = windFaced;
        }
        else
        {
            //Debug.Log($"Nothing! {transform.name} -> {windAngle} and {windDir}");
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

    public void AttackShip(GameObject target)
    {
        Debug.Log(transform.gameObject.name + " attacking");
        if (FB != null)
        {
            FB.Fire(shootPoint.position, target.transform.position, cannonPower, maxAngle);
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
        //Gizmos.DrawRay(transform.position, transform.forward * 200);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawRay(transform.position, windDir * 200);
        Gizmos.DrawWireSphere(transform.position, MathParabola.MaxDistance(cannonPower,maxAngle));
    }

    private void OnMouseEnter()
    {
        if (isEnemy && SelectionManager.AreSelecteds())
        {
            CursorManager._instance.Attack();
        }
    }

    private void OnMouseExit()
    {
        if (CursorManager._instance.Mode() == Constants.ATTACK_CURSOR)
        {
            CursorManager._instance.Selection();
        }
    }
}

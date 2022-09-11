using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

[DefaultExecutionOrder(2)]
public class ShipMovement : MonoBehaviour
{

    //public Camera cam;

    public NavMeshAgent agent;

    public int cost, attack, health;

    public float windAngle, windFaced, windNothing, windFavour;

    public float cannonPower, maxAngle, fireCannonRate, cannonVisionAngle;

    public float rotationSpeed;

    public bool isEnemy;

    private Vector3 windDir;

    private bool isAttacking, isSelected, isMoving = false, firstLoopMove = false;

    private GameObject target;

    [SerializeField]
    private BoxCollider margins;

    private FireBalls FB;

    [SerializeField]
    private GameObject objCannonsR, objCannonsL, movementVisuals;

    private List<Cannon> cannonsRight;
    private List<Cannon> cannonsLeft;

    private LineRenderer lrA, lrM;

    private Vector3 hitPoint;

    [SerializeField]
    private float distanceOfSlow;

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = windNothing;
        //Debug.Log(transform.name + "//" + MathParabola.MaxDistance(100));
        FB = transform.GetComponent<FireBalls>();
        cannonsRight = new List<Cannon>();
        cannonsLeft = new List<Cannon>();

        lrA = transform.GetComponent<LineRenderer>();
        if (lrA != null)
        {
            lrA.positionCount = 2;
            lrA.enabled = false;
        }
        if (movementVisuals != null) {
            lrM = movementVisuals.GetComponent<LineRenderer>();
            if (lrM != null)
            {
                lrM.positionCount = 2;
                lrM.enabled = false;
            }
        }
        //var g = MathParabola.GetValues(transform.position, Vector3.zero, 350);
        //MathParabola.GetPosParabola(transform.position, Vector3.zero, 350, 0.1f, g.grade);
        //MathParabola.GetValues(transform.position, new Vector3(12500f, 0, 0), 350);
        //MathParabola.GetValues(new Vector3(20,3,0), Vector3.zero, 27.77f);
        /*if (objCannonsR != null)
        {
            foreach (Transform child in objCannonsR.GetComponentInChildren<Transform>())
            {
                Cannon c = child.GetComponent<Cannon>();
                c.SetCannon(fireCannonRate);
                cannonsRight.Add(c);
            }
        }
        if (objCannonsL != null)
        {
            foreach (Transform child in objCannonsL.GetComponentInChildren<Transform>())
            {
                Cannon c = child.GetComponent<Cannon>();
                c.SetCannon(fireCannonRate);
                cannonsLeft.Add(c);
            }
        }*/
        //Debug.Log(Vector2.Angle(new Vector2(0,-1), new Vector2(Mathf.Sqrt(2)/2, Mathf.Sqrt(2) / 2)));
        //Debug.Log(Vector2.Angle(new Vector2(0,1), new Vector2(Mathf.Sqrt(2)/2, Mathf.Sqrt(2) / 2)));
        //Debug.Log(Vector2.Angle(new Vector2(0,1), new Vector2(1, 0)));
        //Debug.Log(Vector2.Angle(new Vector2(0,1), new Vector2(-1, 0)));
        //Debug.Log(Vector2.Angle(new Vector2(-1,0), new Vector2(0, 1)));


    }

    public void UpdateStartStats()
    {
        agent.radius = 1;
        if (objCannonsR != null)
        {
            foreach (Transform child in objCannonsR.GetComponentInChildren<Transform>())
            {
                Cannon c = child.GetComponent<Cannon>();
                c.SetCannon(fireCannonRate);
                cannonsRight.Add(c);
            }
        }
        if (objCannonsL != null)
        {
            foreach (Transform child in objCannonsL.GetComponentInChildren<Transform>())
            {
                Cannon c = child.GetComponent<Cannon>();
                c.SetCannon(fireCannonRate);
                cannonsLeft.Add(c);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(agent.isStopped);

        #region TMP
        if (Input.GetKey("l"))
        {
            if (FB != null)
            {
                FB.Fire(cannonsRight[0].GetPosition(), new Vector3(2000, 0, 0), cannonPower, maxAngle);
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
        #endregion

        #region Visuals
        if (lrA != null && lrM != null) {
            if (isSelected && isAttacking)
            {
                lrA.enabled = true;
                lrA.SetPosition(0, new Vector3(transform.position.x, 5, transform.position.z));
                lrA.SetPosition(1, new Vector3(target.transform.position.x, 5, target.transform.position.z));
            }
            else
            {
                lrA.enabled = false;
            }
            if (isSelected && isMoving)
            {
                lrM.enabled = true;
                lrM.SetPosition(0, new Vector3(transform.position.x, 5, transform.position.z));
                lrM.SetPosition(1, new Vector3(hitPoint.x, 5, hitPoint.z));
            }
            else
            {
                lrM.enabled = false;
            }
        }
        #endregion

        #region Wind
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
        #endregion

        if (isMoving)
        {
            //TODO(parcialdone): maybe el moviment es fara codificat rotant quan no estigui close i quan si que faci agent, o potser ja ni cal agent
            //TODO: arreglar el "nope" que fan al canviar de angle
            //TODO:Tmb lo de parar amb ancla i acceleracio i el temps de arranque
            if (Vector3.Distance(transform.position, hitPoint) > agent.stoppingDistance) {
                Vector3 rotatedAux = Quaternion.Euler(0, -20, 0) * transform.forward;
                Vector3 rotated2Aux = Quaternion.Euler(0, 20, 0) * transform.forward;

                Vector2 rotated = new Vector2(rotatedAux.x, rotatedAux.z);
                Vector2 rotated2 = new Vector2(rotated2Aux.x, rotated2Aux.z);

                Vector2 targetDir = new Vector2(hitPoint.x - transform.position.x, hitPoint.z - transform.position.z);

                if (Vector3.Distance(transform.position, hitPoint) > distanceOfSlow) {

                    if (Vector2.Angle(rotated, targetDir) < 20 * 2 && Vector2.Angle(rotated2, targetDir) < 20 * 2)
                    {
                        //agent.isStopped = false;
                        if (firstLoopMove) {
                            agent.SetDestination(hitPoint);
                            agent.angularSpeed = 10;
                            firstLoopMove = false;
                        }
                        Debug.Log("CLOSE");
                    }
                    else
                    {
                        if (firstLoopMove) {
                            //agent.isStopped = true;
                            //agent.angularSpeed = agent.speed * 4;
                            Vector2 right2D = new Vector2(transform.right.x, transform.right.z);
                            Vector2 forward2D = new Vector2(transform.forward.x, transform.forward.z);
                            if ((Vector2.Angle(right2D, targetDir) <= 90 && Vector2.Angle(forward2D, targetDir) <= 90) || (Vector2.Angle(right2D, targetDir) <= 90 && Vector2.Angle(-forward2D, targetDir) <= 90))
                            {
                                Debug.Log("R");
                                transform.Rotate(new Vector3(0, 1, 0) * rotationSpeed * Time.deltaTime);
                            }
                            else
                            {
                                if ((Vector2.Angle(-right2D, targetDir) < 90 && Vector2.Angle(-forward2D, targetDir) < 90) || (Vector2.Angle(-right2D, targetDir) < 90 && Vector2.Angle(forward2D, targetDir) < 90))
                                {
                                    Debug.Log("L");
                                    transform.Rotate(new Vector3(0, -1, 0) * rotationSpeed * Time.deltaTime);
                                }
                            }

                            //transform.position += transform.forward * agent.speed * Time.deltaTime;
                            agent.angularSpeed = 0;
                            agent.SetDestination(transform.position + transform.forward * 100);
                        }
                        Debug.Log("NOPE");
                    }
                }
                else
                {
                    //reallyclose point
                    if (Vector2.Angle(rotated, targetDir) < 20 * 2 && Vector2.Angle(rotated2, targetDir) < 20 * 2)
                    {
                        //face
                        Debug.Log("a");
                    }
                    else
                    {
                        if (Vector2.Angle(rotated, targetDir) < 90 && Vector2.Angle(rotated2, targetDir) < 90)
                        {
                            agent.speed = windFaced;
                            Debug.Log("b");
                        }
                        else
                        {
                            agent.speed = 1;
                            Debug.Log("c");
                        }
                    }

                    agent.SetDestination(hitPoint);
                    agent.angularSpeed = 10;
                }
            }
            else
            {
                Debug.Log("Nothing");
            }
        }

        if (GameManager._instance.inPlacement)
        {
            if (ImOutOfBounds() && !isEnemy)
            {
                transform.position = GameManager._instance.GetNewPos();
            }
        }
        //25 graus de cono
        if (isAttacking)
        {
            //agent.SetDestination(target.transform.position);


            if (IsInRange())
            {

                TrianglePositions(true, out Vector3 oneR, out Vector3 otherR);
                TrianglePositions(false, out Vector3 oneL, out Vector3 otherL);

                if (IsBetween2Vectors(oneR, otherR, cannonVisionAngle)) {
                    foreach (Cannon cannon in cannonsRight)
                    {
                        if (cannon.IsActive() && cannon.IsLoaded())
                        {
                            cannon.FireCannon();
                            FB.Fire(cannon.GetPosition(), target.transform.position, cannonPower, maxAngle);
                        }
                    }
                }
                else
                {
                    if (IsBetween2Vectors(oneL, otherL, cannonVisionAngle))
                    {
                        foreach (Cannon cannon in cannonsLeft)
                        {
                            if (cannon.IsActive() && cannon.IsLoaded())
                            {
                                cannon.FireCannon();
                                FB.Fire(cannon.GetPosition(), target.transform.position, cannonPower, maxAngle);
                            }
                        }
                    }
                    else
                    {
                        //Rotate
                        Debug.Log("ROTATE");
                        Vector2 targetDir = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.z - transform.position.z);
                        Vector2 right2D = new Vector2(transform.right.x, transform.right.z);
                        Vector2 forward2D = new Vector2(transform.forward.x, transform.forward.z);
                        //transform.position += transform.forward * agent.speed*2/3 * Time.deltaTime;
                        //agent.isStopped = true;
                        
                        if ((Vector2.Angle(right2D, targetDir) <= 90 && Vector2.Angle(forward2D, targetDir) <= 90) || (Vector2.Angle(-right2D, targetDir) <= 90 && Vector2.Angle(-forward2D, targetDir) <= 90))
                        {
                            transform.Rotate(new Vector3(0, -1, 0) * rotationSpeed * Time.deltaTime);
                            //Debug.Log("A" + Vector2.Angle(transform.right, targetDir) + "/" + Vector2.Angle(-transform.right, targetDir));
                        }
                        else
                        {
                            if ((Vector2.Angle(right2D, targetDir) < 90 && Vector2.Angle(-forward2D, targetDir) < 90) || (Vector2.Angle(-right2D, targetDir) < 90 && Vector2.Angle(forward2D, targetDir) < 90))
                            {
                                transform.Rotate(new Vector3(0, 1, 0) * rotationSpeed * Time.deltaTime);
                            }
                        }
                        agent.SetDestination(transform.position + transform.forward * 50);
                    }
                }
            }
            else
            {
                //Move closer
                agent.SetDestination(target.transform.position);
            }
        }

    }

    public void MoveShip(Vector3 hitPosition)
    {

        agent.isStopped = false;
        //Ray rayCam = Camera.main.ScreenPointToRay(Input.mousePosition);

        //RaycastHit hit;

        isAttacking = false;
        isMoving = true; //maybe fa algun bugg que no estigui en hit

        //if (Physics.Raycast(rayCam, out hit))
        //{
        //move
        //agent.SetDestination(hit.point);
        //    hitPoint = hit.point;
        hitPoint = hitPosition;
        //}
        firstLoopMove = true;

    }

    public void AttackShip(GameObject target)
    {
        Debug.Log(transform.gameObject.name + " attacking");

        this.target = target;

        isAttacking = true;
        isMoving = false;

        if (FB != null)
        {
            //FB.Fire(shootPoint.position, target.transform.position, cannonPower, maxAngle);
        }
    }
    public void SelectionChange(bool visible)
    {
        isSelected = visible;
    }

    public Vector3 GetMargins()
    {
        return margins.size;
    }

    private bool IsInRange()
    {
        return (Vector3.Distance(target.transform.position, gameObject.transform.position) < MathParabola.MaxDistance(cannonPower, maxAngle));
    }

    private void TrianglePositions(in bool right, out Vector3 one, out Vector3 other)
    {
        Vector2 v = new Vector2(transform.right.x, transform.right.z);

        //Debug.Log("R->" + transform.right);
        
        float a = v.x * v.x + v.y * v.y;


        if (transform.right.x > 0) {
            float b = -2 * Mathf.Cos(cannonVisionAngle * Mathf.Deg2Rad) * v.y;
            //Debug.Log(Mathf.Cos(angle) + "/" + Mathf.Cos(angle*Mathf.Deg2Rad) + "/" +  Mathf.Cos(angle)*Mathf.Rad2Deg);
            float c = -(v.x * v.x) + Mathf.Cos(cannonVisionAngle * Mathf.Deg2Rad) * Mathf.Cos(cannonVisionAngle * Mathf.Deg2Rad);

            //Debug.Log(v.x + "/" + v.y + "/////" + a + "/" + b + "/" + c);
            float u2 = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
            float u2v2 = (-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
            //Debug.Log(u2 + "/" + u2v2);
            /*if (1 - u2 * u2 > 0)
            {
                Debug.Log("A");
            }
            else
            {
                Debug.Log("badA");
            }
            if (1 - u2v2 * u2v2 > 0)
            {
                Debug.Log("B");
            }
            else
            {
                Debug.Log("badB");
            }*/
            float u1 = Mathf.Sqrt(1 - u2 * u2);
            float u1v2 = Mathf.Sqrt(1 - u2v2 * u2v2);

            if (Vector2.Angle(new Vector2(0, 1), v) < cannonVisionAngle)
            {
                if (v.y > 0)
                {
                    u1 = u1 * (-1);
                }
            }
            if (Vector2.Angle(new Vector2(0, -1), v) < cannonVisionAngle)
            {
                if (v.y < 0)
                {
                    u1v2 = u1v2 * (-1);
                }
            }

            one = new Vector3(u1, 0, u2);
            other = new Vector3(u1v2, 0, u2v2);

        }
        else
        {
            float b = -2 * Mathf.Cos(cannonVisionAngle * Mathf.Deg2Rad) * v.x;
            //Debug.Log(Mathf.Cos(angle) + "/" + Mathf.Cos(angle*Mathf.Deg2Rad) + "/" +  Mathf.Cos(angle)*Mathf.Rad2Deg);
            float c = -(v.y * v.y) + Mathf.Cos(cannonVisionAngle * Mathf.Deg2Rad) * Mathf.Cos(cannonVisionAngle * Mathf.Deg2Rad);

            //Debug.Log(v.x + "/" + v.y + "/////" + a + "/" + b + "/" + c);
            float u1 = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
            float u1v2 = (-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
            //Debug.Log(u2 + "/" + u2v2);
            /*if (1 - u1 * u1 > 0)
            {
                Debug.Log("A");
            }
            else
            {
                Debug.Log("badA");
            }
            if (1 - u1v2 * u1v2 > 0)
            {
                Debug.Log("B");
            }
            else
            {
                Debug.Log("badB");
            }*/
            float u2 = Mathf.Sqrt(1 - u1 * u1);
            float u2v2 = Mathf.Sqrt(1 - u1v2 * u1v2);


            
            if (Vector2.Angle(new Vector2(-1, 0), v) < cannonVisionAngle)
            {
                if (v.y > 0)
                {
                    u2v2 = u2v2 * (-1);
                }
            }
            
            if (Vector2.Angle(new Vector2(-1, 0), v) < cannonVisionAngle)
            {
                if (v.y < 0)
                {
                    u2 = u2 * (-1);
                }
            }
            else
            {
                if (v.x < 0 && v.y < 0)
                {
                    //u1 = u1 * (-1);
                    //u1v2 = u1v2 * (-1);
                    u2 = u2 * (-1);
                    u2v2 = u2v2 * (-1);
                }
            }

            /*if (Vector2.Angle(new Vector2( 0, -1), v) < angle)
            {
                if (v.x < 0)
                {
                    u2 = u2 * (-1);
                    u2v2 = u2v2 * (-1);
                }
            }*/

            one = new Vector3(u1, 0, u2);
            other = new Vector3(u1v2, 0, u2v2);
        }
        if (!right)
        {
            one = -one;
            other = -other;
        }

    }

    private bool IsBetween2Vectors( Vector3 one, Vector3 other, float angle = 20)
    {
        Vector2 targetDir = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.z - transform.position.z);
        if (Vector2.Angle(new Vector2(one.x, one.z), targetDir) < angle*2 && Vector2.Angle(new Vector2(other.x, other.z), targetDir) < angle*2)
        {
            //Debug.Log("YES");
            return true;
        }
        else
        {
            //Debug.Log("NO");
            return false;
        }
    }

    private bool ImOutOfBounds()
    {
        Vector3 size = GameManager._instance.playerSpawnAera.GetComponent<BoxCollider>().size;
        Vector3 center = GameManager._instance.playerSpawnAera.GetComponent<BoxCollider>().center;
        if (center.x - size.x / 2 < transform.position.x
            && center.x + size.x / 2 > transform.position.x
            && center.z - size.z / 2 < transform.position.z
            && center.z + size.z / 2 > transform.position.z)
        {
            return false;
        }
        else
        {
            return true;
        }
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
        TrianglePositions(true, out Vector3 one, out Vector3 other);
        Gizmos.DrawRay(transform.position, one * MathParabola.MaxDistance(cannonPower, maxAngle));
        Gizmos.DrawRay(transform.position, other * MathParabola.MaxDistance(cannonPower, maxAngle));

        TrianglePositions(false, out Vector3 one2, out Vector3 other2);
        Gizmos.DrawRay(transform.position, one2 * MathParabola.MaxDistance(cannonPower, maxAngle));
        Gizmos.DrawRay(transform.position, other2 * MathParabola.MaxDistance(cannonPower, maxAngle));

        Gizmos.DrawWireSphere(transform.position, MathParabola.MaxDistance(cannonPower,maxAngle));
        //Gizmos.DrawRay(transform.position, Mathf.Cos(12)/transform.right);

        Gizmos.color = Color.blue;
        //Gizmos.DrawRay(transform.position, transform.right * 100);
        //Gizmos.DrawRay(transform.position, -transform.right * 100);

        Vector3 rotated = Quaternion.Euler(0, -20, 0) * transform.forward;
        Vector3 rotated2 = Quaternion.Euler(0, 20, 0) * transform.forward;
        Gizmos.DrawRay(transform.position,rotated*500);
        Gizmos.DrawRay(transform.position,rotated2*500);

        Gizmos.DrawWireSphere(transform.position, distanceOfSlow);
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

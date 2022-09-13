using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(3)]
public class EnemyIA : MonoBehaviour
{
    [SerializeField]
    private ShipMovement sm;

    private float turnToMoveDuration = 20;
    private float turnToMoveStartTime = 0;

    private float moveNextDuration = 10;
    private float moveNextStartTime = 0;

    private bool isAttacking = false;
    private bool isMoving = false;

    private Vector3 mapCenter;
    private Vector3 mapSize;

    // Start is called before the first frame update
    void Start()
    {
        sm = GetComponent<ShipMovement>();
        sm.UpdateStartStats();

        mapCenter = GameManager._instance.mapArea.transform.position;
        mapSize = GameManager._instance.mapArea.transform.localScale;

        moveNextStartTime = Time.time;
        moveNextDuration = Random.Range(30f, 180f);
        //Debug.Log("/" + moveNextDuration + "/" + moveNextStartTime + "/" + mapCenter + "/" + mapSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager._instance.inPlacement) {
            #region attack
            float closeDistance = Mathf.Infinity;
            int posCloser = -1;
            for (int i = 0; i < SelectionManager.unitRTsList.Count; i++)
            {
                float thisDistance = Vector3.Distance(SelectionManager.unitRTsList[i].transform.position, gameObject.transform.position);

                if (thisDistance < MathParabola.MaxDistance(sm.cannonPower, sm.maxAngle))
                {
                    if (closeDistance > thisDistance)
                    {
                        closeDistance = thisDistance;
                        posCloser = i;
                    }
                }
            }
            
            if (posCloser != -1)
            {
                turnToMoveStartTime = Time.time;
                isAttacking = true;
                isMoving = false;
                sm.AttackShip(SelectionManager.unitRTsList[posCloser].gameObject);
            }
            else
            {
                if (Time.time >= turnToMoveStartTime + turnToMoveDuration)
                {
                    isAttacking = false;
                    //sm.MoveShip(Vector3.zero);
                }
            }
            #endregion

            #region move
            if (!isAttacking)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    moveNextStartTime = Time.time;
                    moveNextDuration = Random.Range(30f, 180f);
                    //move
                    sm.MoveShip(GenerateRandomPosition(mapSize, mapCenter));
                }
                else
                {
                    if (Time.time >= moveNextStartTime + moveNextDuration)
                    {
                        //movenext
                        moveNextStartTime = Time.time;
                        moveNextDuration = Random.Range(30f, 180f);
                        sm.MoveShip(GenerateRandomPosition(mapSize, mapCenter));
                    }
                }
            }
            #endregion
            if (sm.isDoingNothing)
            {
                sm.isDoingNothing = false;
                isAttacking = false;
                isMoving = false;
            }
            if (sm.isDestination)
            {
                sm.isDestination = false;
                isAttacking = false;
                isMoving = false;
            }
        }
    }
    private Vector3 GenerateRandomPosition(Vector3 sizeArea, Vector3 centerArea)
    {

        float x = Random.Range(centerArea.x - sizeArea.x / 2, centerArea.x + sizeArea.x / 2);
        float z = Random.Range(centerArea.z - sizeArea.z / 2, centerArea.z + sizeArea.z / 2);

        return new Vector3(x, 0, z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, MathParabola.MaxDistance(sm.cannonPower, sm.maxAngle));
        Gizmos.DrawRay(transform.position, sm.agent.destination - transform.position);
    }
}

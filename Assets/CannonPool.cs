using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPool : MonoBehaviour
{
    public static CannonPool cannonPoolInstance;

    [SerializeField]
    private GameObject cannonBall;

    private bool notEnoughtCannonBallsInPool = true;

    private List<GameObject> cannonBalls;

    private void Awake()
    {
        cannonPoolInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cannonBalls = new List<GameObject>();
    }

    public GameObject GetCannonBall()
    {
        if (cannonBalls.Count > 0)
        {
            for (int i = 0; i < cannonBalls.Count; i++)
            {
                if (!cannonBalls[i].activeInHierarchy)
                {
                    return cannonBalls[i];
                }
            }

        }

        if (notEnoughtCannonBallsInPool)
        {
            GameObject ball = Instantiate(cannonBall);
            ball.SetActive(false);
            cannonBalls.Add(ball);
            return ball;
        }

        return null;
    }
}

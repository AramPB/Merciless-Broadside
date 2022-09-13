using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBalls : MonoBehaviour
{
    public void Fire(Vector3 start, Vector3 end, float speed, float maxAngle, int damage, bool isEnemy)
    {
        GameObject ball = CannonPool.cannonPoolInstance.GetCannonBall();

        ball.SetActive(true);
        ball.GetComponent<BulletBehaviour>().SetStats(start, end, speed, isEnemy, damage, maxAngle);
    }
}

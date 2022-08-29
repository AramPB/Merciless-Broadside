using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MathParabola
{

    private static float modifiedGravity = 98.1f;

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
    
    public static float MaxDistance(float velocity, float maxAngle = 45)
    {
        return (velocity * velocity * Mathf.Sin(2 * maxAngle * Mathf.Deg2Rad)) / (modifiedGravity / 10);
    }

    public static (float maxTime, float grade) GetValues(Vector3 origin, Vector3 target, float power, float maxAngle = 45)
    {
        //float dist = Vector3.Distance(origin, target);
        Vector2 origin2D = new Vector2(origin.x, origin.z);
        Vector2 target2D = new Vector2(target.x, target.z);
        float relativeX = Vector2.Distance(origin2D, target2D);
        //Debug.Log(relativeX);
        float relativeY = Mathf.Abs(target.y - origin.y);

        //Mod
        relativeY = 0;

        float aux = -(((modifiedGravity / 10) * Mathf.Pow(relativeX, 2)) / (2 * power * power));

        float angle = Mathf.Atan(-(relativeX + Mathf.Sqrt(Mathf.Pow(relativeX, 2) - 4 * aux * (aux - relativeY))) / (2 * aux)) * Mathf.Rad2Deg;
        //Debug.Log(-(Mathf.Pow(relativeX, 2) + Mathf.Sqrt(Mathf.Pow(relativeX, 4) - 4 * aux * (aux - target.y + origin.y))) / (2 * aux));
        float angle2 = Mathf.Atan(-(relativeX - Mathf.Sqrt(Mathf.Pow(relativeX, 2) - 4 * aux * (aux - relativeY))) / (2 * aux)) * Mathf.Rad2Deg;

        //Debug.Log($"Angles: {angle} // {angle2} |||| Pos: {origin} |||| Power: {power} |||| Target: {target} |||| Aux: {aux}");

        //--Necessari el temps?

        //float maxTime = (target.x - origin.x) / (Mathf.Cos(angle * Mathf.Rad2Deg) * power);
        //float maxTime2 = (target.x - origin.x) / (Mathf.Cos(angle2 * Mathf.Rad2Deg) * power);
        float maxTime = 2 * power * Mathf.Sin(angle * Mathf.Deg2Rad) / (modifiedGravity / 10);
        float maxTime2 = 2 * power * Mathf.Sin(angle2 * Mathf.Deg2Rad) / (modifiedGravity / 10);
        //float maxTime2 = 2 * 30 * Mathf.Sin(25 * Mathf.Deg2Rad) / (9.8f);
        
        //Debug.Log($"Times: {maxTime} // {maxTime2}");

        if (angle > maxAngle && angle2 > maxAngle)
        {
            //Debug.Log("Angle issues");
            return (0.0f, 0.0f);
        }
        if (angle > maxAngle)
        {
            return (0.0f, angle2);
        }
        if (angle2 > maxAngle)
        {
            return (0.0f, angle);
        }

        //Debug.Log("Both valid angles");
        return (0.0f, 0.0f);
    }

    public static Vector3 GetPosParabola(Vector3 origin, Vector3 target, float power, float time, float grade)
    {
        //Debug.Log($"Variables: {grade} |||| Pos: {origin} |||| Power: {power} |||| Target: {target} |||| Time: {time}");
        //Debug.Log(grade);
        Vector3 absO = new Vector3 (Mathf.Abs(origin.x), Mathf.Abs(origin.y), Mathf.Abs(origin.z));
        //falta vigilar direccions i el calcul que el fa amb un sist de coord diferent
        float x = absO.x + power * time * Mathf.Cos(grade * Mathf.Deg2Rad);
        float y = absO.y + power * time * Mathf.Sin(grade * Mathf.Deg2Rad) - (modifiedGravity / 20) * time * time;
        float auxX = Mathf.Abs(origin.x - target.x);
        float auxZ = Mathf.Abs(origin.z - target.z);
        //Debug.Log($"x : {x} || y : {y} || auxX : {auxX} || auxZ : {auxZ}");
        float z = Mathf.Abs(absO.x - Mathf.Abs(x)) * auxZ / auxX;

        if (target.x < origin.x)
        {
            x = origin.x - Mathf.Abs(absO.x - x);
            //Debug.Log("X");
        }
        else
        {
            x = origin.x + Mathf.Abs(absO.x - x);
            //Debug.Log("X2");
        }


        //Debug.Log($"z = {z} // absO.x = {absO.x} // x = {x} // {auxX} / {auxZ}");
        if (target.z < origin.z)
        {
            z = origin.z - z;
            //Debug.Log("Z");
        }
        else
        {
            z = origin.z + z;
            //Debug.Log("Z2");
        }
        //Debug.Log(new Vector3(x, y, z));
        return new Vector3(x, y, z);
    }

    //GetValues va millor
    /*public static (float maxTime, float grade) GetValues2(Vector3 origin, Vector3 target, float range, float maxAngle = 45)
    {
        Vector2 origin2D = new Vector2(origin.x, origin.z);
        Vector2 target2D = new Vector2(target.x, target.z);

        float distance = Vector2.Distance(origin2D, target2D);

        float angle = distance *maxAngle / range;

        return (0, angle);
    }*/

    public static Vector3 GetPosParabola2(Vector3 origin, Vector3 target, float power, float time, float grade)
    {
        //Debug.Log($"Variables: {grade} |||| Pos: {origin} |||| Power: {power} |||| Target: {target} |||| Time: {time}");
        Vector2 origin2D = new Vector2(origin.x, origin.z);
        Vector2 target2D = new Vector2(target.x, target.z);

        float distance = Vector2.Distance(origin2D, target2D);

        float x = 0 + power * time * Mathf.Cos(grade * Mathf.Deg2Rad);
        float y = origin.y + power * time * Mathf.Sin(grade * Mathf.Deg2Rad) - (modifiedGravity / 20) * time * time;

        float auxX = Mathf.Abs(origin.x - target.x);
        float auxZ = Mathf.Abs(origin.z - target.z);

        float percentage = x / distance;

        float finalX;
        float finalZ;
        float finalY = y;

        if (target.x < origin.x)
        {
            //Debug.Log($"X->{origin.x} , {auxX} , {percentage} , {distance} , {x}");
            finalX = origin.x - auxX * percentage;
        }
        else
        {
            finalX = origin.x + auxX * percentage;
        }

        if (target.z < origin.z)
        {
            finalZ = origin.z - auxZ * percentage;

        }
        else
        {
            finalZ = origin.z + auxZ * percentage;
        }
        //Debug.Log(new Vector3(finalX, finalY, finalZ));
        return new Vector3(finalX, finalY, finalZ);
    }
}

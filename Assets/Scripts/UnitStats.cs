using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/Stats")]

public class UnitStats : ScriptableObject
{
    public bool isEnemy;

    public enum unitType
    {
        Boat,
        Ship
    };
    [Space(15)]
    [Header("Unit Settings")]
    public unitType type;

    public new string name;

    public Sprite shipImage;

    public GameObject unitPrefab;

    [Space(15)]
    [Header("Unit Base Stats")]
    [Space(40)]
    public int cost;
    public int attack;
    public int health;
    public float windAngle;
    public float facingWindSpeed;
    public float nothingWindSpeed;
    public float favourWindSpeed;
    public float rotationSpeed;
    [Space(15)]
    public float cannonPower;
    public float maxAngleCannon;
    public float fireCannonRate;
    public float cannonVisionAngle;
    [Space(15)]
    public int maxCaptains;
    public int maxOfficers;
    public int maxCrewmembers;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/Stats")]

public class UnitStats : ScriptableObject
{
    //public bool isEnemy;

    public enum unitType
    {
        Boat,
        Ship
    };
    [Space(15)]
    [Header("Unit Settings")]
    public unitType type;

    public new string name;

    public GameObject unitPrefab;

    [Space(15)]
    [Header("Unit Base Stats")]
    [Space(40)]
    public int cost;
    public int attack;
    public int health;
    public float windAngle;
    public float facingWindSpeed;
    public float favourWindSpeed;
    public float nothingWindSpeed;
}

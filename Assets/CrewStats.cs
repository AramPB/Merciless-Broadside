using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "New Character/Stats")]

public class CrewStats : ScriptableObject
{
    public enum unitType
    {
        Captain,
        Gunner,
        Cannoneer
    };

    [Space(15)]
    [Header("Character Settings")]
    public unitType type;

    public new string name;

    public Sprite characterImage;

    [Space(15)]
    [Header("Unit Base Stats")]
    [Space(40)]
    public int cost;
    public int attack;
    public int health;

    [Space(15)]
    public float cannonPower;
    public float fireCannonRate;

}

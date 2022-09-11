using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUIManager : MonoBehaviour
{
    public static StatsUIManager _instance;

    public List<CrewStats> CharactersList = new List<CrewStats>();
    public List<UnitStats> ShipsList = new List<UnitStats>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public CrewStats GetCharacterStats(string nameType)
    {
        foreach (CrewStats character in CharactersList)
        {
            if (character.name == nameType)
            {
                return character;
            }
        }
        Debug.Log("Character not exists in CharacterList form StatsUIManager");
        return null;
    }

    public UnitStats GetShipStats(string nameType)
    {
        foreach (UnitStats ship in ShipsList)
        {
            if (ship.name == nameType)
            {
                return ship;
            }
        }
        Debug.Log("Character not exists in ShipList form StatsUIManager");
        return null;
    }
}

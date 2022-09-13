using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUIManager : MonoBehaviour
{
    public static StatsUIManager _instance;

    public List<CrewStats> NavyCharactersList = new List<CrewStats>();
    public List<CrewStats> PirateCharactersList = new List<CrewStats>();
    public List<UnitStats> NavyShipsList = new List<UnitStats>();
    public List<UnitStats> PirateShipsList = new List<UnitStats>();

    public int numFactions = 2;

    public int actualFaction = 0;

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

    public void GetListsStats(int faction, out List<CrewStats> crewStats, out List<UnitStats> shipStats)
    {
        switch (faction)
        {
            case Constants.NAVY_FACTION:
                crewStats = NavyCharactersList;
                shipStats = NavyShipsList;
                break;
            case Constants.PIRATE_FACTION:
                crewStats = PirateCharactersList;
                shipStats = PirateShipsList;
                break;
            default:
                crewStats = PirateCharactersList;
                shipStats = PirateShipsList;
                break;
        }
    }

    public CrewStats GetCharacterStats(string nameType)
    {
        if (actualFaction == Constants.NAVY_FACTION) {
            foreach (CrewStats character in NavyCharactersList)
            {
                if (character.name == nameType)
                {
                    return character;
                }
            }
        }
        if (actualFaction == Constants.PIRATE_FACTION)
        {
            foreach (CrewStats character in PirateCharactersList)
            {
                if (character.name == nameType)
                {
                    return character;
                }
            }
        }
        Debug.Log("Character not exists in CharacterList form StatsUIManager");
        return null;
    }

    public UnitStats GetShipStats(string nameType)
    {
        if (actualFaction == Constants.NAVY_FACTION)
        {
            foreach (UnitStats ship in NavyShipsList)
            {
                if (ship.name == nameType)
                {
                    return ship;
                }
            }
        }
        if (actualFaction == Constants.PIRATE_FACTION)
        {
            foreach (UnitStats ship in PirateShipsList)
            {
                if (ship.name == nameType)
                {
                    return ship;
                }
            }
        }
        Debug.Log("Character not exists in ShipList form StatsUIManager");
        return null;
    }
}

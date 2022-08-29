using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class UnitHandler : MonoBehaviour
{

    public static UnitHandler unitHandler;

    [SerializeField]
    private UnitStats Ship, Boat;

    // Start is called before the first frame update
    private void Start()
    {
        unitHandler = this;
    }

    public (int cost, int attack, int health, float windAngle, float facingWindSpeed, float favourWindSpeed, float nothingWindSpeed, float cannonPower, float maxAngle) GetUnitStats(string type)//STATS MOD
    {
        UnitStats stats;

        switch (type)
        {
            case "ship":
                stats = Ship;
                break;
            case "boat":
                stats = Boat;
                break;
            default:
                Debug.Log($"Unit type: {type} not found");
                return (0, 0, 0, 0, 0, 0, 0, 0, 0);
        }
        return (stats.cost, stats.attack, stats.health, stats.windAngle, stats.facingWindSpeed, stats.favourWindSpeed, stats.nothingWindSpeed, stats.cannonPower, stats.maxAngleCannon); //STATS MOD
    }

    public void SetUnitStats(Transform type)
    {
        foreach (Transform child in type)
        {
            foreach (Transform unit in child)
            {
                string unitName = child.name.Substring(0, child.name.Length - 1).ToLower();//remove s and capitals from herachy name

                var stats = GetUnitStats(unitName);

                ShipMovement ship;
                //STATS MOD
                if (type == GameManager._instance.playerUnits)
                {
                    ship = unit.GetComponent<ShipMovement>();

                    ship.isEnemy = false;

                    ship.cost = stats.cost;
                    ship.attack = stats.attack;
                    ship.health = stats.health;
                    ship.windAngle = stats.windAngle;
                    ship.windFaced = stats.facingWindSpeed;
                    ship.windFavour = stats.favourWindSpeed;
                    ship.windNothing = stats.nothingWindSpeed;
                    ship.cannonPower = stats.cannonPower;
                    ship.maxAngle = stats.maxAngle;
                }
                else if(type == GameManager._instance.enemyUnits)
                {
                    //set enemy stats
                    ship = unit.GetComponent<ShipMovement>();
                    ship.isEnemy = true;
                }

                //set Unit Stats


                //if we have upgrades or buffs add them here
            }
        }
    }
}

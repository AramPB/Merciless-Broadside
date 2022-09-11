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

    public (int cost, int attack, int health, float windAngle, float facingWindSpeed, float favourWindSpeed, float nothingWindSpeed, float cannonPower, float maxAngle, float fireCannonRate, float rotationSpeed, float cannonVisionAngle) GetUnitStats(string type)//STATS MOD
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
                return (0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }
        return (stats.cost, stats.attack, stats.health, stats.windAngle, stats.facingWindSpeed, stats.favourWindSpeed, stats.nothingWindSpeed, stats.cannonPower, stats.maxAngleCannon, stats.fireCannonRate, stats.rotationSpeed, stats.cannonVisionAngle); //STATS MOD
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
                    ship.fireCannonRate = stats.fireCannonRate;
                    ship.rotationSpeed = stats.rotationSpeed;
                    ship.cannonVisionAngle = stats.cannonVisionAngle;
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

    public void LoadUnits(Transform playerUnits, Vector3 sizeArea, Vector3 centerArea)
    {
        ShipMovement shipM;
        UnitStats stats;
        //bool isOK;
        //Vector3 tmpPosition = Vector3.zero;
        for (int i = 0; i < StoreInfoUnits.units.Count; i++) //STATS MOD
        {
            //create ShipUnit
           /* isOK = false;

            while (!isOK)
            {
                isOK = true;
                tmpPosition = GenerateRandomPosition(sizeArea, centerArea);
                foreach (Transform child in playerUnits)
                {
                    if (child.GetComponent<ShipMovement>())
                    {
                        ShipMovement sm = child.GetComponent<ShipMovement>();

                        if (Vector3.Distance(child.position, tmpPosition) < sm.GetMargins().x && Vector3.Distance(child.position, tmpPosition) < sm.GetMargins().z)
                        {
                            isOK = false;
                        }
                    }
                }
            }*/

            stats = StoreInfoUnits.units[i].unitStats;

            GameObject ship = Instantiate(stats.unitPrefab);
            ship.transform.SetParent(playerUnits);
            //ship.transform.position = tmpPosition;
            ship.transform.position = GenerateRandomPosition(sizeArea, centerArea, playerUnits);

            shipM = ship.GetComponent<ShipMovement>();

            shipM.isEnemy = false;

            shipM.cost = stats.cost;
            shipM.attack = stats.attack;
            shipM.health = stats.health;
            shipM.windAngle = stats.windAngle;
            shipM.windFaced = stats.facingWindSpeed;
            shipM.windFavour = stats.favourWindSpeed;
            shipM.windNothing = stats.nothingWindSpeed;
            shipM.cannonPower = stats.cannonPower;
            shipM.maxAngle = stats.maxAngleCannon;
            shipM.fireCannonRate = stats.fireCannonRate;
            shipM.rotationSpeed = stats.rotationSpeed;
            shipM.cannonVisionAngle = stats.cannonVisionAngle;



            Debug.Log("U:" + StoreInfoUnits.units[i].unitStats.name);

            for (int j = 0; j < StoreInfoUnits.units[i].crewMembers.Count; j++)
            {
                //Create crew
                Debug.Log("c->" + StoreInfoUnits.units[i].crewMembers[j].characterStats.name);
            }
        }
    }

    public void UpdateStartStats(Transform playerUnits)
    {
        foreach (Transform child in playerUnits)
        {
            if (child.GetComponent<ShipMovement>())
            {
                child.GetComponent<ShipMovement>().UpdateStartStats();
            }
        }
    }

    public Vector3 GenerateRandomPosition(Vector3 sizeArea, Vector3 centerArea, Transform playerUnits)
    {
        bool isOK;
        Vector3 tmpPosition = Vector3.zero;
        isOK = false;
        float x = 0;
        float z = 0;
        while (!isOK)
        {
            isOK = true;
            //tmpPosition = GenerateRandomPosition(sizeArea, centerArea);


            x = Random.Range(centerArea.x - (sizeArea.x / 2), centerArea.x + (sizeArea.x / 2));
            z = Random.Range(centerArea.z - (sizeArea.z / 2), centerArea.z + (sizeArea.z / 2));

            foreach (Transform child in playerUnits)
            {
                if (child.GetComponent<ShipMovement>())
                {
                    ShipMovement sm = child.GetComponent<ShipMovement>();

                    if (Vector3.Distance(child.position, tmpPosition) < sm.GetMargins().x && Vector3.Distance(child.position, tmpPosition) < sm.GetMargins().z)
                    {
                        isOK = false;
                    }
                }
            }
        }

        return new Vector3(x, 0, z);
    }
}

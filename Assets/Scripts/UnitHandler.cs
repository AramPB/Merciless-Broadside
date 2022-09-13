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

    public void LoadUnits(Transform playerUnits, Vector3 sizeArea, Vector3 centerArea, Transform playerUIPanel)
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

            shipM._name = stats.name;
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

            ship.GetComponent<UnitRTS>().AddUnit();

            //Debug.Log("U:" + StoreInfoUnits.units[i].unitStats.name);

            for (int j = 0; j < StoreInfoUnits.units[i].crewMembers.Count; j++)
            {
                //Create crew
                //Debug.Log("c->" + StoreInfoUnits.units[i].crewMembers[j].characterStats.name);
                shipM.health += StoreInfoUnits.units[i].crewMembers[j].characterStats.plusShipHealth * StoreInfoUnits.units[i].crewMembers[j].quantity;
                shipM.attack += StoreInfoUnits.units[i].crewMembers[j].characterStats.plusShipAttack * StoreInfoUnits.units[i].crewMembers[j].quantity;
                shipM.windFaced += StoreInfoUnits.units[i].crewMembers[j].characterStats.plusShipSpeed * StoreInfoUnits.units[i].crewMembers[j].quantity;
                shipM.windNothing += StoreInfoUnits.units[i].crewMembers[j].characterStats.plusShipSpeed * StoreInfoUnits.units[i].crewMembers[j].quantity;
                shipM.windFavour += StoreInfoUnits.units[i].crewMembers[j].characterStats.plusShipSpeed * StoreInfoUnits.units[i].crewMembers[j].quantity;
                shipM.fireCannonRate -= StoreInfoUnits.units[i].crewMembers[j].characterStats.plusShipFireRate * StoreInfoUnits.units[i].crewMembers[j].quantity;
                shipM.cannonPower += StoreInfoUnits.units[i].crewMembers[j].characterStats.plusShipCannonPower * StoreInfoUnits.units[i].crewMembers[j].quantity;
            }

            GameObject shipUI = Instantiate(GameManager._instance.playerPanelUI);
            shipUI.transform.SetParent(playerUIPanel);
            shipUI.GetComponent<UnitUIPanelController>().SetStats(shipM, stats.shipImage);
            shipM.gameObject.GetComponent<UnitRTS>().SetUnitUIPanel(shipUI.GetComponent<UnitUIPanelController>());
        }
    }

    private int RandomEnemyFaction(int playerFacion)
    {
        int enemyFaction;
        do
        {
            enemyFaction = Random.Range(0, StatsUIManager._instance.numFactions);
        } while (enemyFaction == playerFacion);
        return enemyFaction;
    }
    public bool ProbabilityCheck(float threshold)
    {
        if (threshold == 0)
            return false;
        float a = Random.Range(0, 100f);
        if (a <= threshold)
            return true;
        return false;
    }

    public void LoadEnemies(Transform enemyUnits, Vector3 sizeArea, Vector3 centerArea)
    {
        int maxCost = StoreInfoUnits.currentGameCost + (int)(StoreInfoUnits.currentGameCost * 0.1);
        int faction = StoreInfoUnits.playerFaction;
        StatsUIManager._instance.GetListsStats(RandomEnemyFaction(faction), out List<CrewStats> crewStats, out List<UnitStats> shipStats);

        int maxIters = shipStats.Count * 4;
        int totalCost = 0;
        int iter = 0;
        Debug.Log("MAX:" + maxCost);
        while (iter <= maxIters)
        {
            int pos = Random.Range(0, shipStats.Count);
            if (shipStats[pos].cost + totalCost <= maxCost)
            {
                iter = 0;
                totalCost += shipStats[pos].cost;
                //add ship`+ stats ...

                GameObject ship = Instantiate(shipStats[pos].unitPrefab);
                ship.AddComponent<EnemyIA>();
                ship.transform.SetParent(enemyUnits);
                Debug.Log("Ship->" + shipStats[pos].name);
                //ship.transform.position = tmpPosition;
                ship.transform.position = GenerateRandomPosition(sizeArea, centerArea, enemyUnits);
                ship.transform.rotation = Quaternion.Euler(0, 180, 0);
                ShipMovement shipM;

                shipM = ship.GetComponent<ShipMovement>();

                shipM.isEnemy = true;

                shipM._name = shipStats[pos].name;
                shipM.cost = shipStats[pos].cost;
                shipM.attack = shipStats[pos].attack;
                shipM.health = shipStats[pos].health;
                shipM.windAngle = shipStats[pos].windAngle;
                shipM.windFaced = shipStats[pos].facingWindSpeed;
                shipM.windFavour = shipStats[pos].favourWindSpeed;
                shipM.windNothing = shipStats[pos].nothingWindSpeed;
                shipM.cannonPower = shipStats[pos].cannonPower;
                shipM.maxAngle = shipStats[pos].maxAngleCannon;
                shipM.fireCannonRate = shipStats[pos].fireCannonRate;
                shipM.rotationSpeed = shipStats[pos].rotationSpeed;
                shipM.cannonVisionAngle = shipStats[pos].cannonVisionAngle;

                ship.GetComponent<UnitRTS>().AddUnit();
                ship.GetComponent<UnitRTS>().ChangeEnemyColorSelection();

                

                //crew
                for (int i = 0; i < crewStats.Count; i++)
                {
                    int maxCrew;
                    switch (crewStats[i].type)
                    {
                        case CrewStats.unitType.Captain:
                            maxCrew = shipStats[pos].maxCaptains;
                            break;
                        case CrewStats.unitType.Officer:
                            maxCrew = shipStats[pos].maxOfficers;
                            break;
                        case CrewStats.unitType.Crew:
                            maxCrew = shipStats[pos].maxCrewmembers;
                            break;
                        default:
                            maxCrew = 1;
                            break;
                    }
                    if (ProbabilityCheck(75))
                    {
                        int quantity;
                        int maxIters2 = crewStats[i].maxPerShip * 4;
                        int iters2 = 0;
                        do
                        {
                            quantity = Random.Range(1, maxCrew + 1);
                            iters2++;
                        } while (quantity * crewStats[i].cost + totalCost > maxCost && maxIters2 >= iters2);
                        if ((maxIters2 >= iters2))
                        {
                            totalCost += quantity * crewStats[i].cost;
                            Debug.Log("Crew->" + crewStats[i].name + "->" + quantity);
                            //add crew
                            shipM.health += crewStats[i].plusShipHealth * quantity;
                            shipM.attack += crewStats[i].plusShipAttack * quantity;
                            shipM.windFavour += crewStats[i].plusShipSpeed * quantity;
                            shipM.windNothing += crewStats[i].plusShipSpeed * quantity;
                            shipM.windFaced += crewStats[i].plusShipSpeed * quantity;
                            shipM.fireCannonRate -= crewStats[i].plusShipFireRate * quantity;
                            shipM.cannonPower += crewStats[i].plusShipCannonPower * quantity;
                        }
                    }
                }
                
            }
            iter++;
        }
        Debug.Log(totalCost);
    }

    public void UpdateUnitUI(Transform playerUnits)
    {
        foreach (Transform child in playerUnits)
        {
            if (child.GetComponent<ShipMovement>())
            {
                child.GetComponent<ShipMovement>().UpdateUnitUI();
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

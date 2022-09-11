using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FleetPanelController : MonoBehaviour
{

    private List<UnitClass> units = new List<UnitClass>();

    [SerializeField]
    private GameObject listParent;

    [SerializeField]
    private GameObject unitPanel, crewMemberPanel;

    [SerializeField]
    private TextMeshProUGUI textCost;

    private int maxCost, currentCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        units.Clear();
        foreach (Transform child in listParent.transform)
        {
            Destroy(child.gameObject);
        }
        currentCost = 0;
    }

    private bool CanAddCost(int cost, int quantity = -1)
    {
        if (quantity == -1)
        {
            if (currentCost + cost > maxCost)
            {
                Debug.Log("!" + currentCost + "/" + cost);
                return false;
            }
            else
            {
                currentCost += cost;
                textCost.text = "COST: " + currentCost + "/" + maxCost;
                return true;
            }
        }
        else
        {
            if (currentCost + cost * quantity > maxCost)
            {
                Debug.Log("!" + currentCost + "/" + cost + "/" + quantity);
                return false;
            }
            else
            {
                currentCost += cost * quantity;
                textCost.text = "COST: " + currentCost + "/" + maxCost;
                return true;
            }
        }
    }

    private void RemoveCost(int cost, int quantity = -1)
    {
        if (quantity == -1)
        {
            currentCost -= cost;
            textCost.text = "COST: " + currentCost + "/" + maxCost;
        }
        else
        {
            currentCost -= cost * quantity;
            textCost.text = "COST: " + currentCost + "/" + maxCost;
        }
    }

    public void AddShip(UnitStats stats)
    {
        if (CanAddCost(stats.cost)) {
            UnitClass unit = new UnitClass();
            unit.unitStats = stats;
            unit.crewMembers = new List<CrewClass>();
            unit.selected = false;

            units.Add(unit);

            GameObject newObject = Instantiate(unitPanel) as GameObject;

            int newPos = units.Count;
            for (int i = 0; i < units.Count; i++)
            {
                newPos += units[i].crewMembers.Count;
            }

            newObject.GetComponent<UnitPanelController>().SetPosition(newPos - 1);
            newObject.GetComponent<UnitPanelController>().SetStats(stats);
            newObject.transform.SetParent(listParent.transform);
        }
        else
        {
            Debug.Log("Cost Over!");
        }
    }

    public void RemoveShip(int position)
    {
        //Debug.Log("Rem:"+position);

        //int newPos = 0;

        GetUnitsInfo(position, out int unitsNewPos, out int numCrew);

        /*
        for (int j = 0; j < units.Count; j++)
        {
            if (newPos == position)
            {
                unitsNewPos = j;
                numCrew = units[j].crewMembers.Count;
            }
            newPos += 1;
            newPos += units[j].crewMembers.Count;
        }*/
        RemoveCost(units[unitsNewPos].unitStats.cost);
        for (int j = 0; j < numCrew; j++)
        {
            RemoveCost(units[unitsNewPos].crewMembers[j].characterStats.cost, units[unitsNewPos].crewMembers[j].quantity);
        }
        int i = 0;
        foreach (Transform child in listParent.transform)
        {
            if (i >= position && i <= position + numCrew)
            {
                Destroy(child.gameObject);
            }else if (i > position)
            {
                if (child.GetComponent<UnitPanelController>())
                {
                    child.GetComponent<UnitPanelController>().SetPosition(child.GetComponent<UnitPanelController>().GetPosition() - (numCrew + 1));
                }
                if (child.GetComponent<CrewMemberPanelController>())
                {
                    child.GetComponent<CrewMemberPanelController>().SetPosition(child.GetComponent<CrewMemberPanelController>().GetPosition() - (numCrew + 1));
                }
            }
            i++;
        }
        units.RemoveAt(unitsNewPos);
    }

    public void SelectUnit(int position)
    {
        int j = 0;
        foreach (Transform child in listParent.transform)
        {
            
            if (j != position)
            {
                if (child.GetComponent<UnitPanelController>())
                {
                    child.GetComponent<UnitPanelController>().DeSelectMe();
                }
            }
            j++;
        }

        GetUnitsInfo(position, out int unitsPos, out _);

        for (int i = 0; i < units.Count; i++)
        {
            units[i].selected = false;
            if (i == unitsPos)
            {
                units[i].selected = true;
            }
        }

    }

    public void UpdateCharactersToShip(bool add, int quantity, CrewStats stats)
    {
        
        int j = 0;
        int UIPos = -1;
        foreach (Transform child in listParent.transform)
        {
            if (child.GetComponent<UnitPanelController>())
            {
                if (child.GetComponent<UnitPanelController>().ImSelected())
                {
                    UIPos = j;
                }
            }
            j++;
        }

        int position = -1;
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].selected)
            {
                position = i;
            }
        }
        if (position != -1 && UIPos != -1)
        {

            int crewPos = -1;
            for (int i = 0; i < units[position].crewMembers.Count; i++)
            {
                if (units[position].crewMembers[i].characterStats.name == stats.name)
                {
                    crewPos = i;
                }
            }
            if (crewPos != -1) {
                if (add)
                {
                    if (CanAddCost(stats.cost, quantity)) {
                        units[position].crewMembers[crewPos].quantity += quantity;

                        Debug.Log("add" + quantity);
                    }
                    else
                    {
                        Debug.Log("Cost Over!");
                    }
                }
                else
                {
                    if (quantity > units[position].crewMembers[crewPos].quantity)
                    {
                        RemoveCost(stats.cost, units[position].crewMembers[crewPos].quantity);
                    }
                    else
                    {
                        RemoveCost(stats.cost, quantity);
                    }
                    
                    units[position].crewMembers[crewPos].quantity -= quantity;
                    Debug.Log(" no add" + quantity);
                }
                int k = 0;
                foreach (Transform child in listParent.transform)
                {
                    if (k >= UIPos && k <= UIPos + units[position].crewMembers.Count) {
                        if (child.GetComponent<CrewMemberPanelController>())
                        {
                            if (child.GetComponent<CrewMemberPanelController>().GetName() == stats.name)
                            {
                                if (units[position].crewMembers[crewPos].quantity <= 0)
                                {
                                    units[position].crewMembers.RemoveAt(crewPos);
                                    RemoveCharacterToShip(k, stats.name, false);
                                }
                                else
                                {
                                    child.GetComponent<CrewMemberPanelController>().EditQuantity(units[position].crewMembers[crewPos].quantity);
                                }
                            }
                        }
                    }
                    k++;
                }

            }
            else
            {
                //new crewMembers
                if (add)
                {
                    if (CanAddCost(stats.cost, quantity)) {
                        CrewClass crewMember = new CrewClass();
                        crewMember.quantity = quantity;
                        crewMember.characterStats = stats;
                        units[position].crewMembers.Add(crewMember);

                        GameObject newObject = Instantiate(crewMemberPanel) as GameObject;

                        int k = 0;
                        foreach (Transform child in listParent.transform)
                        {
                            if (k > UIPos)
                            {
                                if (child.GetComponent<CrewMemberPanelController>())
                                {
                                    child.GetComponent<CrewMemberPanelController>().SetPosition(child.GetComponent<CrewMemberPanelController>().GetPosition() + 1);
                                }
                                if (child.GetComponent<UnitPanelController>())
                                {
                                    child.GetComponent<UnitPanelController>().SetPosition(child.GetComponent<UnitPanelController>().GetPosition() + 1);
                                }
                            }
                            k++;
                        }
                        newObject.GetComponent<CrewMemberPanelController>().SetPosition(UIPos + 1);
                        newObject.GetComponent<CrewMemberPanelController>().SetStats(stats);
                        newObject.GetComponent<CrewMemberPanelController>().EditQuantity(quantity);
                        newObject.transform.SetParent(listParent.transform);
                        newObject.transform.SetSiblingIndex(UIPos + 1);
                    }
                    else
                    {
                        Debug.Log("Cost Over!");
                    }
                }
                else
                {
                    //trying to remove nothing
                }

            }

        }
        else
        {
            Debug.Log("NoOneSelected");
            //no selected
        }

    }

    public void RemoveCharacterToShip(int position, string name, bool removeCost)
    {
        Transform[] children = listParent.transform.GetComponentsInChildren<Transform>();
        int UIPos = 0;
        bool first = true;
        for (int i = position; i >= 0; i--)
        {
            if (children[i].transform.GetComponent<UnitPanelController>() && first)
            {
                UIPos = i;
                first = false;
            }
        }
        GetUnitsInfo(UIPos, out int unitPos, out _);

        for (int j = 0; j < units[unitPos].crewMembers.Count; j++)
        {
            if (units[unitPos].crewMembers[j].characterStats.name == name)
            {
                if (removeCost)
                {
                    RemoveCost(units[unitPos].crewMembers[j].characterStats.cost, units[unitPos].crewMembers[j].quantity);
                }
                units[unitPos].crewMembers.RemoveAt(j);
            }
        }

        int k = 0;
        foreach (Transform child in listParent.transform)
        {
            if (k == position)
            {
                Destroy(child.gameObject);
            }
            else if (k > position)
            {
                if (child.GetComponent<UnitPanelController>())
                {
                    child.GetComponent<UnitPanelController>().SetPosition(child.GetComponent<UnitPanelController>().GetPosition() - 1);
                }
                if (child.GetComponent<CrewMemberPanelController>())
                {
                    child.GetComponent<CrewMemberPanelController>().SetPosition(child.GetComponent<CrewMemberPanelController>().GetPosition() - 1);
                }
            }
            k++;
        }
    }

    private void GetUnitsInfo(int position, out int unitsPosition, out int numCrewMembers)
    {
        int newPos = 0;
        int unitsNewPos = 0;
        int numCrew = 0;
        for (int j = 0; j < units.Count; j++)
        {
            if (newPos == position)
            {
                unitsNewPos = j;
                numCrew = units[j].crewMembers.Count;
            }
            newPos += 1;
            newPos += units[j].crewMembers.Count;
        }
        unitsPosition = unitsNewPos;
        numCrewMembers = numCrew;
    }

    public void UploadInforToTheGame()
    {
        StoreInfoUnits.units = units;
    }

    public void UpdateMaxCost(int cost)
    {
        maxCost = cost;
        textCost.text = "COST: 0/" + cost.ToString();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsPanelController : MonoBehaviour
{

    [SerializeField]
    private GameObject _default, characters, ships, units;

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
        _default.SetActive(true);
        characters.SetActive(false);
        ships.SetActive(false);
        units.SetActive(false);
    }

    public void ChangeStats(CrewStats cS = null, UnitStats uS = null)
    {
        //CHARACTERS
        if (cS != null)
        {
            _default.SetActive(false);
            characters.SetActive(true);
            ships.SetActive(false);
            units.SetActive(false);

            foreach (Transform child in characters.transform)
            {
                if (child.name == "Name")
                {
                    child.GetComponent<TextMeshProUGUI>().text = cS.name;
                }
                if (child.name == "Cost")
                {
                    child.GetComponent<TextMeshProUGUI>().text = cS.cost.ToString();
                }
                if (child.name == "Health")
                {
                    child.GetComponent<TextMeshProUGUI>().text = "+" + cS.plusShipHealth.ToString();
                }
                if (child.name == "Attack")
                {
                    child.GetComponent<TextMeshProUGUI>().text = "+" + cS.plusShipAttack.ToString();
                }
                if (child.name == "Speed")
                {
                    child.GetComponent<TextMeshProUGUI>().text = "+" + cS.plusShipSpeed.ToString();
                }
                if (child.name == "FireRate")
                {
                    child.GetComponent<TextMeshProUGUI>().text = "-" + cS.plusShipFireRate.ToString();
                }
                if (child.name == "CannonPower")
                {
                    child.GetComponent<TextMeshProUGUI>().text = "+" + cS.plusShipCannonPower.ToString();
                }
                if (child.name == "Image")
                {
                    child.GetComponent<Image>().sprite = cS.characterImage;
                }
            }
        }
        else
        {
            //SHIPS
            if (uS != null)
            {
                _default.SetActive(false);
                characters.SetActive(false);
                ships.SetActive(true);
                units.SetActive(false);
                foreach (Transform child in ships.transform)
                {
                    if (child.name == "Name")
                    {
                        child.GetComponent<TextMeshProUGUI>().text = uS.name;
                    }
                    if (child.name == "Cost")
                    {
                        child.GetComponent<TextMeshProUGUI>().text = uS.cost.ToString();
                    }
                    if (child.name == "Health")
                    {
                        child.GetComponent<TextMeshProUGUI>().text = uS.health.ToString();
                    }
                    if (child.name == "Attack")
                    {
                        child.GetComponent<TextMeshProUGUI>().text = uS.attack.ToString();
                    }
                    if (child.name == "Speed")
                    {
                        child.GetComponent<TextMeshProUGUI>().text = uS.facingWindSpeed.ToString() + "//" + uS.nothingWindSpeed.ToString() + "//" + uS.favourWindSpeed.ToString();
                    }
                    if (child.name == "FireRate")
                    {
                        child.GetComponent<TextMeshProUGUI>().text = uS.fireCannonRate.ToString();
                    }
                    if (child.name == "CannonPower")
                    {
                        child.GetComponent<TextMeshProUGUI>().text = uS.cannonPower.ToString();
                    }
                    if (child.name == "Max Rank 1")
                    {
                        child.GetComponent<TextMeshProUGUI>().text = uS.maxCaptains.ToString();
                    }
                    if (child.name == "Max Rank 2")
                    {
                        child.GetComponent<TextMeshProUGUI>().text = uS.maxOfficers.ToString();
                    }
                    if (child.name == "Max Crew")
                    {
                        child.GetComponent<TextMeshProUGUI>().text = uS.maxCrewmembers.ToString();
                    }
                    if (child.name == "Image")
                    {
                        child.GetComponent<Image>().sprite = uS.shipImage;
                    }
                }
            }
            //DEFAULT
            else
            {
                _default.SetActive(true);
                characters.SetActive(false);
                ships.SetActive(false);
                units.SetActive(false);
            }
        }

    }
}

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
                if (child.name == "Type")
                {
                    child.GetComponent<TextMeshProUGUI>().text = cS.type.ToString();
                }
                if (child.name == "Attack")
                {
                    child.GetComponent<TextMeshProUGUI>().text = cS.attack.ToString();
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
                    if (child.name == "Type")
                    {
                        child.GetComponent<TextMeshProUGUI>().text = uS.type.ToString();
                    }
                    if (child.name == "Attack")
                    {
                        child.GetComponent<TextMeshProUGUI>().text = uS.attack.ToString();
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

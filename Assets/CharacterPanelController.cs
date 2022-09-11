using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterPanelController : MonoBehaviour
{

    private string nameType;
    
    private TextMeshProUGUI name;

    private CrewStats stats;

    [SerializeField]
    private Image image;

    private StatsPanelController statsPanelController;

    private FleetPanelController fleetPanelController;

    [SerializeField]
    private InputField input;

    // Start is called before the first frame update
    void Start()
    {
        InputField iField = gameObject.GetComponentInChildren<InputField>();
        iField.characterValidation = InputField.CharacterValidation.Integer;
        iField.characterLimit = 4;

        TextMeshProUGUI[] o = gameObject.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI t in o)
        {
            if (t.transform.name == "Name")
            {
                name = t;
            }
        }

        //name = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        nameType = gameObject.transform.name;

        stats = StatsUIManager._instance.GetCharacterStats(nameType);

        statsPanelController = FindObjectOfType<StatsPanelController>();
        fleetPanelController = FindObjectOfType<FleetPanelController>();

        if (stats != null)
        {
            name.text = stats.name;
            image.sprite = stats.characterImage;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendCharacetrStats()
    {
        statsPanelController.ChangeStats(stats);
    }

    public void UpdateCharactersToShip(bool add)
    {
        if (input.text == null || input.text == "")
        {
            //must add numbers
            Debug.Log("peto");
        }
        else
        {
            if (int.Parse(input.text) != 0) {
                fleetPanelController.UpdateCharactersToShip(add, int.Parse(input.text), stats);
            }
            else
            {
                //no 0
            }
        }
    }
}

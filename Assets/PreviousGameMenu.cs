﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreviousGameMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pirateShipContent, navyShipContent;
    [SerializeField]
    private GameObject pirateCrewContent, navyCrewContent;
    [SerializeField]
    private ScrollRect shipContent, crewContent;

    [SerializeField]
    private TMP_Dropdown factionDropdown;

    [SerializeField]
    private FleetPanelController fleetPanelController;

    private int actualFaction = 0;

    private enum Faction
    {
        Navy,
        Pirate
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeFaction(0);

        factionDropdown.options.Clear();
        factionDropdown.value = 0;



        for (int i = 0; i < System.Enum.GetValues(typeof(Faction)).Length; i++)
        {
            factionDropdown.options.Add(new TMP_Dropdown.OptionData() { text = ((Faction)i).ToString() });
        }
    }

    private void OnEnable()
    {
        ChangeFaction(0);
        factionDropdown.value = 0;
        actualFaction = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeFaction(int type)
    {
        actualFaction = type;
        StatsUIManager._instance.actualFaction = actualFaction;
        fleetPanelController.ClearInfo();
        switch ((Faction)type)
        {
            case Faction.Navy:
                pirateShipContent.SetActive(false);
                navyShipContent.SetActive(true);
                pirateCrewContent.SetActive(false);
                navyCrewContent.SetActive(true);

                shipContent.content = navyShipContent.GetComponent<RectTransform>();
                crewContent.content = navyCrewContent.GetComponent<RectTransform>();
                break;
            case Faction.Pirate:
                navyShipContent.SetActive(false);
                pirateShipContent.SetActive(true);
                navyCrewContent.SetActive(false);
                pirateCrewContent.SetActive(true);

                shipContent.content = pirateShipContent.GetComponent<RectTransform>();
                crewContent.content = pirateCrewContent.GetComponent<RectTransform>();
                break;
            default:
                break;
        }
    }

    public void UploadActualFaction()
    {
        StoreInfoUnits.playerFaction = actualFaction;
    }
}

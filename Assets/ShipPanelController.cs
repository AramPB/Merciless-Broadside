using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipPanelController : MonoBehaviour
{
    private string nameType;

    //private TextMeshProUGUI name;

    private UnitStats stats;

    [SerializeField]
    private Image image;

    private StatsPanelController statsPanelController;

    private FleetPanelController fleetPanelController;

    // Start is called before the first frame update
    void Start()
    {
        //name = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        nameType = gameObject.transform.name;

        stats = StatsUIManager._instance.GetShipStats(nameType);

        statsPanelController = FindObjectOfType<StatsPanelController>();
        fleetPanelController = FindObjectOfType<FleetPanelController>();

        if (stats != null)
        {
            //name.text = stats.name;
            image.sprite = stats.shipImage;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendShipStats()
    {
        statsPanelController.ChangeStats(null, stats);
    }

    public void AddShip()
    {
        fleetPanelController.AddShip(stats);
    }
}

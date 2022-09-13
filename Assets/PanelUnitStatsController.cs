using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelUnitStatsController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI 
        _name,
        health,
        attack,
        range,
        fireRate,
        speeds,
        cost;
    //[SerializeField]
    //private Image image;

    [SerializeField]
    private bool forEnemies;

    private ShipMovement sm;

    [SerializeField]
    private GameObject statsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (forEnemies)
        {
            if (SelectionManager.NumberSelectedsEnemies() == 1)
            {
                foreach (UnitRTS unit in SelectionManager.unitRTsEnemyList)
                {
                    if (unit.IsSelected())
                    {
                        sm = unit.gameObject.GetComponent<ShipMovement>();
                        UpdateStats();
                        ShowStats(true);
                    }
                }
            }
            else
            {
                //borra
                ShowStats(false);
            }
        }
        else
        {
            if (SelectionManager.NumberSelecteds() == 1)
            {
                foreach (UnitRTS unit in SelectionManager.unitRTsList)
                {
                    if (unit.IsSelected())
                    {
                        sm = unit.gameObject.GetComponent<ShipMovement>();
                        UpdateStats();
                        ShowStats(true);
                    }
                }
            }
            else
            {
                //borra
                ShowStats(false);
            }
        }
    }

    private void ShowStats(bool show)
    {
        statsPanel.SetActive(show);
    }

    private void UpdateStats()
    {
        _name.text = sm._name;
        health.text = sm.health.ToString();
        attack.text = sm.attack.ToString();
        range.text = MathParabola.MaxDistance(sm.cannonPower, sm.maxAngle).ToString();
        fireRate.text = sm.fireCannonRate.ToString();
        speeds.text = sm.windFaced.ToString() + "//" + sm.windNothing.ToString() + "//" + sm.windFavour.ToString();
        cost.text = sm.cost.ToString();
    }
}

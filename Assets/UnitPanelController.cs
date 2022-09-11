using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitPanelController : MonoBehaviour
{

    private FleetPanelController fleetPanelController;

    private int position;

    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Color selectedColor;

    private Color startColor;

    private bool selected = false;

    private void Start()
    {
        fleetPanelController = FindObjectOfType<FleetPanelController>();
        startColor = transform.GetComponent<Image>().color;
    }

    public void RemoveMe()
    {
        fleetPanelController.RemoveShip(position);
    }

    public void SetPosition(int pos)
    {
        position = pos;
    }

    public int GetPosition()
    {
        return position;
    }

    public void SetStats(UnitStats stats)
    {
        text.text = stats.name;
        image.sprite = stats.shipImage;
    }

    public void SelectMe()
    {
        selected = true;
        transform.GetComponent<Image>().color = selectedColor;
        fleetPanelController.SelectUnit(position);
    }

    public void DeSelectMe()
    {
        selected = false;
        transform.GetComponent<Image>().color = startColor;
    }

    public bool ImSelected()
    {
        return selected;
    }
}

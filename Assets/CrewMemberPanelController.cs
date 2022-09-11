using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrewMemberPanelController : MonoBehaviour
{
    private FleetPanelController fleetPanelController;

    private int position;

    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI nameMember, quantity;

    // Start is called before the first frame update
    void Start()
    {
        fleetPanelController = FindObjectOfType<FleetPanelController>();
    }

    public void SetPosition(int pos)
    {
        position = pos;
    }

    public int GetPosition()
    {
        return position;
    }

    public void EditQuantity(int quant)
    {
        quantity.text = "x " + quant.ToString();
    }
    
    public void SetStats(CrewStats stats)
    {
        image.sprite = stats.characterImage;
        nameMember.text = stats.name;
    }

    public string GetName()
    {
        return nameMember.text;
    }
    public void RemoveMe()
    {
        fleetPanelController.RemoveCharacterToShip(position, nameMember.text, true);
    }
}

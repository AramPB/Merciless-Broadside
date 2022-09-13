using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UnitUIPanelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _name;

    [SerializeField]
    private HealthBar healthBar;

    [SerializeField]
    private Image image;
    [SerializeField]
    private Image panelImage;

    [SerializeField]
    private GameObject panelDeath;

    private ShipMovement sm;

    private Color savedColor;

    private bool imSelected = false;

    public bool hasToChange = false;

    private void Start()
    {      
        savedColor = panelImage.color;
    }

    public void SetStats(ShipMovement sm, Sprite image)
    {
        this.sm = sm;
        _name.text = sm._name;
        healthBar.SetMaxHealth(sm.health);
        this.image.sprite = image;

    }

    public void Die()
    {
        panelDeath.SetActive(true);
    }

    public void SetHealth(int current)
    {
        healthBar.SetHealth(current);
    }

    public void SelectMe()
    {
        hasToChange = true;
        if (!imSelected)
        {
            panelImage.color = new Color32(11, 72, 0, 255);
            imSelected = true;
        }
        else
        {
            panelImage.color = savedColor;
            imSelected = false;
        }

    }

    public bool IsSelected()
    {
        return imSelected;
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            panelImage.color = new Color32(11, 72, 0, 255);
            imSelected = true;
        }
        else
        {
            panelImage.color = savedColor;
            imSelected = false;
        }

    }

}

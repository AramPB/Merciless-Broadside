using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitInfoUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private TextMeshProUGUI x;


    public void SetName(string name)
    {
        _name.text = name;
    }

    public void ActivateX(bool activate)
    {
        x.gameObject.SetActive(activate);
    }
}

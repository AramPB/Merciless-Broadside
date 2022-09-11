using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameSettingsMenuController : MonoBehaviour
{
    [SerializeField]
    private InputField input;

    [SerializeField]
    private FleetPanelController fleetPanelController;

    // Start is called before the first frame update
    void Start()
    {
        input.characterValidation = InputField.CharacterValidation.Integer;
        input.characterLimit = 7;
        input.text = "1000";
    }

    private void OnEnable()
    {
        input.text = "1000";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendCost()
    {
        fleetPanelController.UpdateMaxCost(int.Parse(input.text));
    }
}

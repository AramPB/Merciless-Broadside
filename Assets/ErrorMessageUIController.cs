using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorMessageUIController : MonoBehaviour
{
    private static TextMeshProUGUI text;
    private static float showStartTime;
    private static float showDuration = 3;
    private static bool count = false;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        showStartTime = 0;
    }
    private void Update()
    {
        if (count)
        {
            if (Time.time >= showStartTime + showDuration)
            {
                ClearText();
            }
        }
    }

    public static void ShowText(string newText)
    {
        text.text = newText;
        showStartTime = Time.time;
        count = true;
    }

    private static void ClearText()
    {
        text.text = "";
        count = false;
    }
}

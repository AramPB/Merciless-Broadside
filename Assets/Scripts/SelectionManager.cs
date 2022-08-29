using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    public static List<UnitRTS> unitRTsList = new List<UnitRTS>();
    public static List<UnitRTS> unitRTsEnemyList = new List<UnitRTS>();

    public static bool AreSelecteds()
    {
        foreach (UnitRTS unit in unitRTsList)
        {
            if (unit.IsSelected())
            {

                return true;
            }
        }
        return false;
    }
}

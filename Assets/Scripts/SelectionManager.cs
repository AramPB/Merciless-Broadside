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

    public static int NumberSelecteds()
    {
        int i = 0;
        foreach (UnitRTS unit in unitRTsList)
        {
            if (unit.IsSelected())
            {
                i++;
            }
        }
        return i;
    }

    public static int NumberSelectedsEnemies()
    {
        int i = 0;
        foreach (UnitRTS unit in unitRTsEnemyList)
        {
            if (unit.IsSelected())
            {
                i++;
            }
        }
        return i;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public enum ToolType
    {
        Planter,
        WateringCan,
        Axe,
        Seed
    }

    public GameObject plantType;

    public ToolType toolType = ToolType.Planter;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    Planter,
    WateringCan,
    Axe,
    Seed
}

public class Tool : MonoBehaviour
{
    public GameObject plantType;

    public ToolType toolType = ToolType.Planter;
}

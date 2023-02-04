using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public enum PlotState
    {
        Empty,
        Planter,
        Stage1,
        Stage2,
        Stage3,
        Finished
    }

    public PlotState myState = PlotState.Empty;

    public void PlayerInteract(Tool tool)
    {
        Debug.Log("Calling Player interact on plot: " + gameObject.name);
        Debug.Log("Using tool " + tool.toolType);

        switch (tool.toolType)
        {
            case Tool.ToolType.Planter:
                Debug.Log("Planter state is: " + myState);

                if (myState == PlotState.Empty)
                {
                    Debug.Log("Placed a planter");
                    myState = PlotState.Planter;
                } else
                {
                    Debug.Log("Could not place a planter");
                }
                break;
            case Tool.ToolType.WateringCan:
                break;
            case Tool.ToolType.Axe:
                break;
            case Tool.ToolType.Seed:
                break;
        }
    }
}

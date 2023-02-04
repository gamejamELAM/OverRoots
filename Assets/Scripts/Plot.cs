using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public enum PlotState
    {
        Empty,
        Planter,
        Growing,
        Finished
    }

    public PlotState myState = PlotState.Empty;

    public GameObject myPlanter;
    Plant myPlant;

    private void Update()
    {

    }

    public void PlayerInteract(Tool tool)
    {
        Debug.Log("Calling Player interact on plot: " + gameObject.name);
        Debug.Log("Using tool " + tool.toolType);

        switch (tool.toolType)
        {
            case Tool.ToolType.Planter:
                if (myState == PlotState.Empty)
                {
                    Debug.Log("I worked");
                    myState = PlotState.Planter;
                    myPlanter.SetActive(true);
                } 
                break;
            case Tool.ToolType.WateringCan:
                if (myState == PlotState.Growing)
                {
                    myPlant.WaterPlant();
                }
                break;
            case Tool.ToolType.Axe:
                break;
            case Tool.ToolType.Seed:
                if (myState == PlotState.Planter)
                {
                    myState = PlotState.Growing;
                    myPlant = Instantiate(tool.plantType, transform.position, Quaternion.identity).GetComponent<Plant>();
                }
                break;
        }
    }
}

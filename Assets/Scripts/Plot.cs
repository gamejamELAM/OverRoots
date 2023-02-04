using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlotState
{
    Empty,
    Planter,
    Growing,
    Finished
}

public class Plot : MonoBehaviour
{
    //Object references to the meshes attached to this Plot
    public GameObject planter;

    //Tracks the state of this plot
    PlotState myState = PlotState.Empty;

    //Tracks the plant being grown in this plot
    Plant myPlant;

    public void Start()
    {
        planter.SetActive(false);
    }

    public void PlayerInteract(GameObject seed, Tool seedBag, Player player)
    {
        if (myState == PlotState.Planter)
        {
            seedBag.Consume(player);

            //Create the appropriate plant and track it
            myPlant = Instantiate(seed, transform.position, Quaternion.Euler(0f, 180f, 0f)).GetComponent<Plant>();

            //Change to the growing state
            myState = PlotState.Growing;
        }
    }

    public void PlayerInteract(ToolType tool)
    {
        switch (tool)
        {
            case ToolType.Planter:
                if (myState == PlotState.Empty)
                {
                    // Turn on the planter mesh
                    planter.SetActive(true);

                    //Move to the next stage
                    myState = PlotState.Planter;
                }
                break;
            case ToolType.WateringCan:
                if (myState == PlotState.Growing)
                {
                    //Call the attached plats water method
                    myPlant.WaterPlant();
                }
                break;
            case ToolType.Axe:
                break;
        }
    }
}

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
    MeshRenderer[] myMeshes;

    //Tracks the state of this plot
    PlotState myState = PlotState.Empty;

    //Tracks the plant being grown in this plot
    Plant myPlant;

    public void Start()
    {
        //Find the planter mesh
        myMeshes = GetComponentsInChildren<MeshRenderer>();
        
        //Turn off the planter mesh
        foreach(MeshRenderer mesh in myMeshes)
        {
            mesh.enabled = false;
        }
    }

    public void PlayerInteract(Tool tool)
    {
        switch (tool.toolType)
        {
            case ToolType.Planter:
                if (myState == PlotState.Empty)
                {
                    // Turn on the planter mesh
                    foreach (MeshRenderer mesh in myMeshes)
                    {
                        mesh.enabled = true;
                    }

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
            case ToolType.Seed:
                if (myState == PlotState.Planter)
                {
                    //Create the appropriate plant and track it
                    myPlant = Instantiate(tool.plantType, transform.position, Quaternion.Euler(0f, 180f, 0f)).GetComponent<Plant>();

                    //Change to the growing state
                    myState = PlotState.Growing;
                }
                break;
        }
    }
}

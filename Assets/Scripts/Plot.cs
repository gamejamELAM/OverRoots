using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlotState
{
    Empty,
    Planter,
    Growing,
    Finished,
    Rooted
}

public class Plot : MonoBehaviour
{
    //Object references to the meshes attached to this Plot
    public GameObject planter;
    public GameObject myRoot;

    //Tracks the state of this plot
    public PlotState myState = PlotState.Empty;

    //Tracks the plant being grown in this plot
    Plant myPlant;

    public void Start()
    {
        planter.SetActive(false);
    }

    public void PlayerInteract(GameObject seed, Tool seedBag, Player player)
    {
        if ((myState == PlotState.Planter) && (seedBag.toolType == ToolType.Seed))
        {
            seedBag.Consume(player);

            //Create the appropriate plant and track it
            myPlant = Instantiate(seed, transform.position, Quaternion.Euler(0f, 180f, 0f)).GetComponent<Plant>();

            //Change to the growing state
            myState = PlotState.Growing;
        }
    }

    public void PlayerInteract(ToolType tool, Player player)
    {
        switch (tool)
        {
            case ToolType.Empty:
                break;
            case ToolType.Hoe:
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
                    if (player.UseWateringCan())
                    {
                        //Call the attached plats water method
                        myPlant.WaterPlant();
                    }
                }
                break;
            case ToolType.Axe:
                if (myPlant != null)
                {
                    Destroy(myPlant.gameObject);
                    myPlant = null;
                }

                planter.SetActive(false);

                if (myState == PlotState.Rooted)
                {
                    bool stillAlive = myRoot.GetComponent<Root>().TakeHit();

                    if (!stillAlive)
                    {
                        planter.SetActive(false);
                        myState = PlotState.Empty;
                    }
                }
                else
                {
                    myState = PlotState.Empty;
                }
                break;
            case ToolType.Scythe:
                if (myState == PlotState.Growing)
                {
                    if (myPlant != null)
                    {
                        if (myPlant.isGrown)
                        {
                            //Add to successful plant counter
                        }

                        Destroy(myPlant.gameObject);
                        myPlant = null;
                    }

                    myState = PlotState.Planter;
                }
                break;
            default:
                break;
        }
    }

    public void GetRooted()
    {
        if (myPlant != null)
        {
            Destroy(myPlant.gameObject);
            myPlant = null;
        }

        myRoot.SetActive(true);
        myState = PlotState.Rooted;
    }
}

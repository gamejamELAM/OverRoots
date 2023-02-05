using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public int myHealth = 3;

    float timeToPropagate = 1.0f;
    bool hasSpawned = false;

    bool fullyGrown = false;

    Plot[] plotsInScene;
    //Management variables
    int closestPlot = -1;

    Root rootThatISpawned = null;

    // Start is called before the first frame update
    public void Start()
    {
        plotsInScene = FindObjectsOfType<Plot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fullyGrown == false)
        {
            float currentScale = transform.localScale.y;
            currentScale *= 1.001f;

            if (currentScale > 1.0f)
            {
                fullyGrown = true;
            }
            else
            {
                transform.localScale = new Vector3(1f, currentScale, 1f);
            }
        }
        else
        {
            if (timeToPropagate > 0)
            {
                timeToPropagate -= Time.deltaTime;
            }
            else if (hasSpawned == false)
            {
                //Set our closest distance to an unrealistic level
                float closestDistance = 99999f;

                //Loop through the list of available plots and find the closest
                for (int i = 0; i < plotsInScene.Length; i++)
                {
                    if (plotsInScene[i].myState != PlotState.Rooted)
                    {
                        //Find the distances
                        Vector3 thisPos = plotsInScene[i].gameObject.transform.position;
                        float distance = (thisPos - transform.position).sqrMagnitude;

                        //If this one is the current closest update our trackers
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestPlot = i;
                        }
                    }
                }

                if (closestPlot != -1)
                {
                    plotsInScene[closestPlot].GetRooted();
                    rootThatISpawned = plotsInScene[closestPlot].myRoot.GetComponent<Root>();

                    hasSpawned = true;
                }
            } 
            else if (!rootThatISpawned.isActiveAndEnabled)
            {
                hasSpawned = false;
                timeToPropagate = 1.0f;
            }
        }
    }

    public bool TakeHit()
    {
        if (myHealth > 0)
        {
            myHealth--;
            return true;
        }
        else
        {
            Reset();
            return false;
        }
    }

    public void Reset()
    {
        timeToPropagate = 1.0f;
        hasSpawned = false;

        fullyGrown = false;

        transform.localScale = new Vector3(0f, 0.01f, 0f);

        myHealth = 3;

        gameObject.SetActive(false);
    }
}

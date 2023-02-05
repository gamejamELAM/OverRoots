using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject avatar;

    [Header("Player Control settings")]
    public float speed = 10.0f;
    public float acceleration = 3f;
    public float selectionDistance = 1f;
    public float toolSelectionDistance = 2f;

    [Header("Key Bindings for this player")]
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode interactKey = KeyCode.RightControl;
    public KeyCode unequipKey = KeyCode.RightShift;

    //Object references
    public GameObject mySelectionBox;
    public Transform equipmentPoint;

    Rigidbody playerBody;
    List<Plot> plotsInScene = new List<Plot>();
    List<Tool> toolsInScene = new List<Tool>();

    public Animator animator;

    //Management variables
    int closestPlot = -1;
    int closestTool = -1;
    bool atSeedCrate = false;
    bool atTool = false;
    bool atPool = false;

    //Tracks the tool the player is holding
    public Tool myTool = null;
    public GameObject mySeed;

    //Tracks the plot the player is standing in
    Plot currentPlot = null;
    Tool adjacentTool = null;

    public bool controlsDisabled = false;

    public int wateringCanCharges = 3;

    // Start is called before the first frame update
    void Start()
    {
        //Find various gameobject references
        playerBody = GetComponent<Rigidbody>();

        Plot[] plots = FindObjectsOfType<Plot>();
        foreach(Plot plot in plots)
        {
            plotsInScene.Add(plot);
        }

        Tool[] tools = FindObjectsOfType<Tool>();
        foreach (Tool tool in tools)
        {
            toolsInScene.Add(tool);
        }

        //Distances are returned squared, so we need to square our selection distance
        selectionDistance = selectionDistance * selectionDistance;
        toolSelectionDistance = toolSelectionDistance * toolSelectionDistance;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Tool tool in toolsInScene)
        {
            Debug.Log(tool.gameObject.name);
        }

        if (controlsDisabled == false)
        {
            //Get the amount we should be moving and apply it
            Vector2 movement = HandleInputAxes();

            animator.SetFloat("Movement", movement.SqrMagnitude());

            playerBody.velocity = new Vector3(movement.x * speed, playerBody.velocity.y, movement.y * speed);

            //Set our closest distance to an unrealistic level
            float closestPlotDistance = 99999f;

            //Loop through the list of available plots and find the closest
            for (int i = 0; i < plotsInScene.Count; i++)
            {
                //Find the distances
                Vector3 thisPos = plotsInScene[i].gameObject.transform.position;
                float distance = (thisPos - transform.position).sqrMagnitude;

                //If this one is the current closest update our trackers
                if (distance < closestPlotDistance)
                {
                    closestPlotDistance = distance;
                    closestPlot = i;
                }
            }

            float closestToolDistance = 99999f;

            for (int i = 0; i < toolsInScene.Count; i++)
            {
                if (toolsInScene[i] != myTool)
                {
                    //Find the distances
                    Vector3 thisPos = toolsInScene[i].gameObject.transform.position;
                    float distance = (thisPos - transform.position).sqrMagnitude;

                    //If this one is the current closest update our trackers
                    if (distance < closestToolDistance)
                    {
                        closestToolDistance = distance;
                        closestTool = i;
                    }
                }
            }

            if (closestToolDistance < toolSelectionDistance)
            {
                atTool = true;
                adjacentTool = toolsInScene[closestTool];
            } 
            else
            {
                atTool = false;
                adjacentTool = null;
            }

            Debug.Log(atTool);

            //If we are within range of a plot
            if (atTool)
            {
                mySelectionBox.SetActive(false);
                currentPlot = null;
                if (Input.GetKeyDown(interactKey))
                {
                    //Call the interact method for the plot, passing in the appropriate tool
                    adjacentTool.PlayerInteract(this);
                }
            }
            else if (atSeedCrate)
            {
                mySelectionBox.SetActive(false);
                currentPlot = null;

                if (Input.GetKeyDown(interactKey))
                {
                    FindObjectOfType<SeedCrate>().PlayerInteract(this);
                    controlsDisabled = true;
                    atSeedCrate = false;
                }
            }
            else if (atPool)
            {
                if (Input.GetKeyDown(interactKey))
                {
                    if (myTool != null)
                    {
                        if (myTool.toolType == ToolType.WateringCan)
                        {
                            RefillWateringCan();
                        }
                    }
                }
            }
            else if (closestPlotDistance < selectionDistance)
            {
                currentPlot = plotsInScene[closestPlot]; //Update our gameobject reference
                mySelectionBox.SetActive(true); //Turn selection box on
                mySelectionBox.transform.position = currentPlot.transform.position; //Move selection box to the plot position

                if (Input.GetKeyDown(interactKey) && myTool != null)
                {
                    if (myTool.toolType == ToolType.Seed)
                    {
                        currentPlot.PlayerInteract(mySeed, myTool, this);
                    }
                    else
                    {
                        currentPlot.PlayerInteract(myTool.toolType, this);
                    }
                }
            }
            else
            {
                mySelectionBox.SetActive(false);
                currentPlot = null;
            }

            if (Input.GetKeyDown(unequipKey))
            {
                if (myTool != null)
                {
                    myTool.Unequip(this);
                }
            }
        }
    }

    //Variables used for input
    float upAxis = 0f;
    float leftAxis = 0f;
    float downAxis = 0f;
    float rightAxis = 0f;
    float horizontalAxis = 0f;
    float verticalAxis = 0f;

    Vector2 HandleInputAxes()
    {
        //Handle input axis - Up
        if (Input.GetKey(upKey))
        {
            upAxis += Time.deltaTime * acceleration;
            avatar.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        } else
        {
            upAxis -= Time.deltaTime * acceleration;
        }

        upAxis = Mathf.Clamp(upAxis, 0f, 1f);

        //Handle input axis - Down
        if (Input.GetKey(downKey))
        {
            downAxis += Time.deltaTime * acceleration;
            avatar.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            downAxis -= Time.deltaTime * acceleration;
        }

        downAxis = Mathf.Clamp(downAxis, 0f, 1f);

        //Handle input axis - Left
        if (Input.GetKey(leftKey))
        {
            leftAxis += Time.deltaTime * acceleration;
            avatar.transform.localRotation = Quaternion.Euler(0f, 270f, 0f);
        }
        else
        {
            leftAxis -= Time.deltaTime * acceleration;
        }

        leftAxis = Mathf.Clamp(leftAxis, 0f, 1f);

        //Handle input axis - Right
        if (Input.GetKey(rightKey))
        {
            rightAxis += Time.deltaTime * acceleration;
            avatar.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else
        {
            rightAxis -= Time.deltaTime * acceleration;
        }

        rightAxis = Mathf.Clamp(rightAxis, 0f, 1f);

        horizontalAxis = rightAxis - leftAxis;
        verticalAxis = upAxis - downAxis;

        return new Vector2(horizontalAxis, verticalAxis);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SeedCrate")
        {
            atSeedCrate = true;
        } 
        else if (other.tag == "Pool")
        {
            atPool = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SeedCrate")
        {
            atSeedCrate = false;
        }
        else if (other.tag == "Pool")
        {
            atPool = false;
        }
    }

    public bool UseWateringCan()
    {
        if (wateringCanCharges > 0)
        {
            wateringCanCharges--;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RefillWateringCan()
    {
        wateringCanCharges = 3;
    }

    public void AddToToolList(Tool toAdd)
    {
        Debug.Log("Adding: " + toAdd);
        toolsInScene.Add(toAdd);
    }

    public void TakeFromToolList(Tool toTake)
    {
        toolsInScene.Remove(toTake);
    }
}

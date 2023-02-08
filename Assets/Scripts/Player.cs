using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject avatar;

    public GameObject hoeVisual;
    public GameObject axeVisual;
    public GameObject canVisual;
    public GameObject scythVisual;

    [Header("Player Control settings")]
    public float speed = 10.0f;
    public float acceleration = 3f;
    public float selectionDistance = 1f;
    public float toolSelectionDistance = 2f;

    public PlayerControls playerControls;

    //Object references
    public GameObject mySelectionBox;
    public Transform equipmentPoint;

    Rigidbody playerBody;
    List<Plot> plotsInScene = new List<Plot>();
    List<Tool> toolsInScene = new List<Tool>();

    public Animator animator;

    public Transform selectionPoint;

    public bool pause = false;

    //Management variables
    int closestPlot = -1;
    int closestTool = -1;
    bool atSeedCrate = false;
    bool atTool = false;
    bool atPool = false;
    bool atPlot = false;

    //Tracks the tool the player is holding
    public Tool myTool = null;
    public GameObject mySeed;

    //Tracks the plot the player is standing in
    Plot currentPlot = null;
    Tool adjacentTool = null;

    public bool pauseForAnim = false;
    public bool controlsDisabled = false;

    public int wateringCanCharges = 3;

    Vector2 movement;
    Vector2 rotation;
    float angle = 0;

    SeedCrate seedCrate;
    public Material hat;

    public void Move(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        seedCrate = FindObjectOfType<SeedCrate>();
        seedCrate.playersInScene.Add(this);

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

        if (FindObjectsOfType<Player>().Length > 1)
        {
            SkinnedMeshRenderer skm = GetComponentInChildren<SkinnedMeshRenderer>();
            skm.materials[0].color = new Color(Random.value, Random.value, Random.value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseForAnim == true)
        {
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                pauseForAnim = false;
            }
        }

        if ((controlsDisabled == false) && (pauseForAnim == false))
        {
            //Get the amount we should be moving and apply it

            animator.SetFloat("Movement", movement.SqrMagnitude());

            playerBody.velocity = new Vector3(movement.x * speed, playerBody.velocity.y, movement.y * speed);

            if (movement != Vector2.zero)
            {
                avatar.transform.forward = new Vector3(movement.x, 0f, movement.y);
            }

            //Set our closest distance to an unrealistic level
            float closestPlotDistance = 99999f;

            //Loop through the list of available plots and find the closest
            for (int i = 0; i < plotsInScene.Count; i++)
            {
                //Find the distances
                Vector3 thisPos = plotsInScene[i].gameObject.transform.position;
                float distance = (thisPos - selectionPoint.position).sqrMagnitude;

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
                    float distance = (thisPos - selectionPoint.position).sqrMagnitude;

                    //If this one is the current closest update our trackers
                    if (distance < closestToolDistance)
                    {
                        closestToolDistance = distance;
                        closestTool = i;
                    }

                    toolsInScene[i].myLight.color = Color.white;
                }
            }

            if (closestToolDistance < toolSelectionDistance)
            {
                atTool = true;
                adjacentTool = toolsInScene[closestTool];
                toolsInScene[closestTool].myLight.color = Color.red;
            }
            else
            {
                atTool = false;
                adjacentTool = null;
            }

            if ((closestPlotDistance < selectionDistance))
            {
                atPlot = true;
                mySelectionBox.SetActive(true);
                currentPlot = plotsInScene[closestPlot];
                mySelectionBox.transform.position = currentPlot.transform.position;
            }
        }
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true)
        {
            if (!pauseForAnim)
            {
                if (atTool)
                {
                    adjacentTool.PlayerInteract(this);
                    SetTool();
                }
                else if (atPool)
                {
                    if (myTool != null)
                    {
                        if (myTool.toolType == ToolType.WateringCan)
                        {
                            RefillWateringCan();
                        }
                    }
                } 
                else if (atSeedCrate)
                {
                    seedCrate.Interact(this);
                } 
                else if (atPool)
                {
                    RefillWateringCan();
                }
            }
        }
    }

    public void UseTool(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true)
        {
            if (myTool != null)
            {
                if ((!pauseForAnim) && (!controlsDisabled))
                {
                    if (atPlot)
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
            }
        }
    }

    public void Drop(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true)
        {
            if (seedCrate != null)
            {
                if (seedCrate.activePlayer == this)
                {
                    seedCrate.Cancel(this);
                }
            }
            if ((!pauseForAnim) && (!controlsDisabled))
            {
                if (myTool != null)
                {
                    myTool.Unequip(this);
                }

                SetTool();
            }
        }
    }

    public void MenuUp(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled == true)
        {
            if (seedCrate != null)
            {
                seedCrate.MenuUp(this);
            }
        }
    }

    public void MenuDown(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled == true)
        {
            if (seedCrate != null)
            {
                seedCrate.MenuDown(this);
            }
        }
    }

    void SetTool()
    {
        if (myTool != null)
        {
            if (myTool.toolType != ToolType.Seed)
            {
                switch (myTool.toolType)
                {
                    case ToolType.Hoe:
                        hoeVisual.SetActive(true);
                        axeVisual.SetActive(false);
                        canVisual.SetActive(false);
                        scythVisual.SetActive(false);
                        break;
                    case ToolType.Axe:
                        hoeVisual.SetActive(false);
                        axeVisual.SetActive(true);
                        canVisual.SetActive(false);
                        scythVisual.SetActive(false);
                        break;
                    case ToolType.Scythe:
                        hoeVisual.SetActive(false);
                        axeVisual.SetActive(false);
                        canVisual.SetActive(false);
                        scythVisual.SetActive(true);
                        break;
                    case ToolType.WateringCan:
                        hoeVisual.SetActive(false);
                        axeVisual.SetActive(false);
                        canVisual.SetActive(true);
                        scythVisual.SetActive(false);
                        break;
                }
            }
            else
            {
                hoeVisual.SetActive(false);
                axeVisual.SetActive(false);
                canVisual.SetActive(false);
                scythVisual.SetActive(false);
            }
        } 
        else
        {
            hoeVisual.SetActive(false);
            axeVisual.SetActive(false);
            canVisual.SetActive(false);
            scythVisual.SetActive(false);
        }
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
        toolsInScene.Add(toAdd);
    }

    public void TakeFromToolList(Tool toTake)
    {
        toolsInScene.Remove(toTake);
    }

    public void ActionPause()
    {
        playerBody.velocity = Vector3.zero;
        pauseForAnim = true;
    }
}

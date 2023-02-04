using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Control settings")]
    public float speed = 10.0f;
    public float acceleration = 3f;

    [Header("Key Bindings for this player")]
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode interactKey = KeyCode.RightControl;

    //Object references
    Rigidbody playerBody;

    [Header("List of available tools")]
    ////////////////////DEBUG
    //List of tools
    public Tool[] toolList;
    int toolSelection = 0;

    //Used to track the plot the player is standing in and the tool they are holding
    Plot currentPlot = null;
    public Tool myTool;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();

        ////////////////////DEBUG
        myTool = toolList[toolSelection];
    }

    // Update is called once per frame
    void Update()
    {
        //Get the amount we should be moving and apply it
        Vector2 movement = HandleInputAxes();
        playerBody.velocity = new Vector3(movement.x * speed, playerBody.velocity.y, movement.y * speed);

        //If the player is next to a plot, is holding a tool, and presses the interact key
        if (Input.GetKeyDown(interactKey) && currentPlot != null && myTool != null)
        {
            //Call the interact method for the plot, passing in the appropriate tool
            currentPlot.PlayerInteract(myTool);
        }

        ////////////////////DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            toolSelection++;

            if(toolSelection >= toolList.Length)
            {
                toolSelection = 0;
            }

            myTool = toolList[toolSelection];
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
        } else
        {
            upAxis -= Time.deltaTime * acceleration;
        }

        upAxis = Mathf.Clamp(upAxis, 0f, 1f);

        //Handle input axis - Down
        if (Input.GetKey(downKey))
        {
            downAxis += Time.deltaTime * acceleration;
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
        //When the player edits a plot, set it to their active plot
        if (other.tag == "Plot")
        {
            currentPlot = other.GetComponent<Plot>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //When the player exits a plot, set the plot to null
        if (other.tag == "Plot")
        {
            currentPlot = null;
        }
    }
}

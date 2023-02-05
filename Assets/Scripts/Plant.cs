using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [Header("Plant Gameplay settings")]
    public float thirstLevel = 30f; //How fast does the plant drink water?
    public float daysToGrow = 1; //How many days does the plant take to grow one stage?

    [Header("Required Objects")]
    public GameObject stage1; //Gameobject representing stage 1
    public GameObject stage2; //Gameobject representing stage 2
    public GameObject stage3; //Gameobject representing stage 3
    public GameObject stage4; //Gameobject representing stage 4

    public GameObject waterIcon; //Gameobject representing the icon for needing watered
    public GameObject grownIcon; //Gameobject representing the icon for being fully grown

    TimeHandler myTime;

    //Management variables
    float timeAlive = 0; //How long have we been planted for?
    float waterLevel = 100f; //How much water do we have?
    int stage = 1; //What stage of growth is the plant at?
    public bool isGrown = false; //Are we fully grown?
    float stageLength = 0f;

    private void Start()
    {
        myTime = FindObjectOfType<TimeHandler>(); //Get the Time Handler object

        //Calculate how many units of game time the plant takes to grow a stage
        stageLength = daysToGrow * myTime.dayLength;
    }

    // Update is called once per frame
    void Update()
    {
        //If we have water
        if (waterLevel > 0)
        {
            //Make sure the 'need water' icon isnt active
            waterIcon.SetActive(false);
            //Add to our 'time alive' counter to keep growing
            timeAlive = myTime.AddGameTime(timeAlive);
            //Subtract from our water level
            waterLevel -= (Time.deltaTime * thirstLevel);
        } 
        else 
        {
            //Show the player we need water by enabling the water timer
            waterIcon.SetActive(true);
        }

        //If we are fully grown
        if (isGrown == true)
        {
            //Make sure the water icon isnt enable, and enable the icon to show we're fully grown and ready for harvesting
            waterIcon.SetActive(false);
            grownIcon.SetActive(true);
        } else
        {
            //Make sure the 'fully grown' icon isnt active
            grownIcon.SetActive(false);
        }

        //If we have been alive for more than a day
        if (timeAlive > stageLength)
        {
            //If we arent fully grown
            if (stage < 4)
            {
                //Add to the current stage
                stage++;
                //Reset the time alive counter
                timeAlive = 0f;
            }
            else
            {
                isGrown = true;
            }
        }

        //Set the appropriate gameobjects active/inactive depending on the level of growth
        switch(stage){
            case 1:
                stage1.SetActive(true);
                stage2.SetActive(false);
                stage3.SetActive(false);
                stage4.SetActive(false);
                break;
            case 2:
                stage1.SetActive(false);
                stage2.SetActive(true);
                stage3.SetActive(false);
                stage4.SetActive(false);
                break;
            case 3:
                stage1.SetActive(false);
                stage2.SetActive(false);
                stage3.SetActive(true);
                stage4.SetActive(false);
                break;
            case 4:
                stage1.SetActive(false);
                stage2.SetActive(false);
                stage3.SetActive(false);
                stage4.SetActive(true);
                break;
        }
    }

    //Called by the player when they use a watering can
    public void WaterPlant()
    {
        //Reset the water level
        waterLevel = 100f;
    }
}

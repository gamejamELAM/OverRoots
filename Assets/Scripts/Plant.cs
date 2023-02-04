using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float thirstLevel = 1;
    public float waterLevel = 100f;

    bool isWatered = true;
    bool isGrown = false;

    TimeHandler myTime;
    float timeAlive = 0;

    public int startingDate;
    public int stage = 1;

    public GameObject stage1;
    public GameObject stage2;
    public GameObject stage3;
    public GameObject stage4;

    public GameObject waterIcon;
    public GameObject grownIcon;

    private void Start()
    {
        myTime = FindObjectOfType<TimeHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWatered == true)
        {
            waterIcon.SetActive(false);
            timeAlive = myTime.AddGameTime(timeAlive);
            waterLevel -= (Time.deltaTime * thirstLevel);

            if (waterLevel < 0)
            {
                isWatered = false;
            }

        } else 
        {
            if (isGrown == false)
            {
                waterIcon.SetActive(true);
            }
        }

        if (isGrown == true)
        {
            grownIcon.SetActive(true);
        } else
        {
            grownIcon.SetActive(false);
        }

        if (timeAlive > 100f)
        {
            stage++;
            timeAlive = 0f;

            if (stage == 4)
            {
                isGrown = true;
            }
        }

        stage = Mathf.Clamp(stage, 1, 4);

        switch(stage){
            case 1:
                stage1.SetActive(true);
                stage2.SetActive(false);
                stage3.SetActive(false);
                stage4.SetActive(false);
                break;
            case 2:
                stage1.SetActive(true);
                stage2.SetActive(true);
                stage3.SetActive(false);
                stage4.SetActive(false);
                break;
            case 3:
                stage1.SetActive(true);
                stage2.SetActive(true);
                stage3.SetActive(true);
                stage4.SetActive(false);
                break;
            case 4:
                stage1.SetActive(true);
                stage2.SetActive(true);
                stage3.SetActive(true);
                stage4.SetActive(true);
                break;
        }
    }

    public void WaterPlant()
    {
        isWatered = true;
        waterLevel = 100f;
    }
}

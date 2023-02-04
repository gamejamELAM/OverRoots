using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    public float timeSpeed = 1000f;

    public Text date;
    public Text time;

    int day = 1;
    int seasonNo = 0;
    int year = 0;

    float timeOfDay = 0f;
    string season = "Spring";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeOfDay = AddGameTime(timeOfDay);

        if (timeOfDay > 100f)
        {
            timeOfDay = 0f;
            day++;
        }

        if (day > 30)
        {
            day = 1;
            seasonNo++;
        }

        if (seasonNo > 3)
        {
            seasonNo = 0;
            year++;
        }

        date.text = GetSeason() + " " + GetDay();
        time.text = timeOfDay.ToString(".0");
    }

    string GetDay()
    {
        string dateToReturn = day.ToString();

        string digit = dateToReturn.Substring(dateToReturn.Length);

        if (digit == "1")
        {
            dateToReturn = dateToReturn + "st";
        }
        else if (digit == "2")
        {
            dateToReturn = dateToReturn + "nd";
        }
        else if (digit == "3")
        {
            dateToReturn = dateToReturn + "rd";
        }
        else
        {
            dateToReturn = dateToReturn + "th";
        }

        return dateToReturn;
    }

    string GetSeason()
    {
        switch (seasonNo)
        {
            case 0:
                return "Spring";
            case 1:
                return "Summer";
            case 2:
                return "Autumn";
            case 3:
                return "Winter";
        }

        return "Error";
    }

    public float AddGameTime(float input)
    {
        return input + Time.deltaTime * timeSpeed;
    }
}

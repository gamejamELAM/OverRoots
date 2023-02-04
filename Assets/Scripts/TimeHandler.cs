using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    [Header("Time Settings")]
    public float timeSpeed = 50f; //Multiply by deltaTime to add time to our counter
    public float dayLength = 100f; //How long a day lasts in terms of deltaTime multiplications

    [Header("Required Objects")]
    public Text date;
    public RectTransform clockHand;

    //Management variables
    int day = 1;
    int seasonNo = 0;
    float timeOfDay = 0f;
    string season = "Spring";

    float handrotation = 0f;

    void Update()
    {
        handrotation = (timeOfDay / dayLength) * 360f;

        clockHand.rotation = Quaternion.Euler(0f,0f, 90f - handrotation);

        //Add time to our counter
        timeOfDay = AddGameTime(timeOfDay);

        //If a day worth of time passes, add to day counter and reset
        if (timeOfDay > 100f)
        {
            timeOfDay = 0f;
            day++;
        }

        //If a season worth of time passes, add to season counter and reset
        if (day > 30)
        {
            day = 1;
            seasonNo++;
        }

        //If a year worth of seasons passes, reset
        if (seasonNo > 3)
        {
            seasonNo = 0;
        }

        //Update the UI elements
        date.text = GetSeason() + " " + GetDay();
    }

    //Gets a formatted version of the day counter
    string GetDay()
    {
        //Make day counter into string variable
        string dateToReturn = day.ToString();

        //Get the last digit of the string variable
        string digit = dateToReturn.Substring(dateToReturn.Length - 1);

        //Attach the appropriate suffix according to the last digit
        if (day >= 11 && day <= 13)
        {
            dateToReturn = dateToReturn + "th";
        }
        else if (digit == "1")
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

        //Return the string
        return dateToReturn;
    }

    //Returns a string representing the season number
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

        //This should never be reached as the season will not go above 3
        return "Error";
    }

    //Used by plants to make sure they are tracking time at the same speed as the calendar
    public float AddGameTime(float input)
    {
        return input + Time.deltaTime * timeSpeed;
    }
}

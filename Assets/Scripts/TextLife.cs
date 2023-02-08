using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLife : MonoBehaviour
{
    public Text text;

    // Update is called once per frame
    void Update()
    {
        text.color = new Color(text.color.r, text.color.b, text.color.g, Mathf.Abs(Mathf.Sin(Time.time)));
    }
}

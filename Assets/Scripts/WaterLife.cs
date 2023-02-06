using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLife : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(Mathf.Sin(2f + Time.time) * 0.5f, Mathf.Sin( 4f + Time.time) * 10f, Mathf.Sin(6f + Time.time) * 0.5f);
    }
}

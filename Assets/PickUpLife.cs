using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLife : MonoBehaviour
{
    float timeOffset;

    private void Start()
    {
        timeOffset = Random.Range(0f,100f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, (Mathf.Sin(timeOffset + (Time.time * 2f))) * 0.2f, transform.localPosition.z);
    }
}

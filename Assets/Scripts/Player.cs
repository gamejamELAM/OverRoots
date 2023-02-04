using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float keyPressSensitivity = 3f;
    public float speed = 10.0f;
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    
    float upAxis = 0f;
    float leftAxis = 0f;
    float downAxis = 0f;
    float rightAxis = 0f;

    float horizontalAxis = 0f;
    float verticalAxis = 0f;

    Rigidbody playerBody;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputAxes();
        playerBody.velocity = new Vector3(horizontalAxis * speed, playerBody.velocity.y, verticalAxis * speed);
    }

    void HandleInputAxes()
    {
         //Handle input axis - Up
        if(Input.GetKey(upKey))
        {
            upAxis += Time.deltaTime * keyPressSensitivity;
        } else
        {
            upAxis -= Time.deltaTime * keyPressSensitivity;
        }

        upAxis = Mathf.Clamp(upAxis, 0f, 1f);

        //Handle input axis - Down
        if (Input.GetKey(downKey))
        {
            downAxis += Time.deltaTime * keyPressSensitivity;
        }
        else
        {
            downAxis -= Time.deltaTime * keyPressSensitivity;
        }

        downAxis = Mathf.Clamp(downAxis, 0f, 1f);

        //Handle input axis - Left
        if (Input.GetKey(leftKey))
        {
            leftAxis += Time.deltaTime * keyPressSensitivity;
        }
        else
        {
            leftAxis -= Time.deltaTime * keyPressSensitivity;
        }

        leftAxis = Mathf.Clamp(leftAxis, 0f, 1f);

        //Handle input axis - Right
        if (Input.GetKey(rightKey))
        {
            rightAxis += Time.deltaTime * keyPressSensitivity;
        }
        else
        {
            rightAxis -= Time.deltaTime * keyPressSensitivity;
        }

        rightAxis = Mathf.Clamp(rightAxis, 0f, 1f);

        horizontalAxis = rightAxis - leftAxis;
        verticalAxis = upAxis - downAxis;

        Debug.Log("H: " + horizontalAxis);
        Debug.Log("V: " + verticalAxis);
    }
}

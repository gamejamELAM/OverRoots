using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    AudioSource audioSource;
    public AudioClip clip;

    private void Start()
    {
        audioSource = GameObject.Find("music").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.anyKey.isPressed)
        {
            Play();
        }

        foreach (Gamepad gamepad in Gamepad.all)
        {
            foreach (InputControl control in gamepad.allControls)
            {
                if (control.IsPressed())
                {
                    if (control.name != "down" && control.name != "up" && control.name != "right" && control.name != "left" && control.name != "x" && control.name != "y" && control.name != "right" && control.name != "rightStick" && control.name != "leftStick")
                    {
                        Play();
                    }
                }
            }
        }
    }

    void Play()
    {
        foreach(Root root in FindObjectsOfType<Root>())
        {
            root.pause = false;
        }

        audioSource.clip = clip;
        audioSource.Play();

        mainMenu.SetActive(false);
    }
}

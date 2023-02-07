using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SeedCrate : MonoBehaviour
{
    PlayerControls playerControls;

    public Transform creationPoint;
    public GameObject seedCratePanel;
    public RectTransform startPoint;
    public GameObject optionButton;
    public GameObject[] seeds;
    float spacing = -45f;

    Player activePlayer;
    bool active = false;

    List<Image> images = new List<Image>();
    int selection = 0;

    Player[] playersInScene;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Gameplay.Enable();

        //playerControls.Gameplay.Inte.performed += ctx => Interact();
        playerControls.Gameplay.MenuUp.performed += ctx => MenuUp();

        playerControls.Gameplay.MenuDown.performed += ctx => MenuDown();

        playerControls.Gameplay.Interact.performed += ctx => Interact();

        playerControls.Gameplay.Cancel.performed += ctx => Cancel();
    }

    // Start is called before the first frame update
    void Start()
    {
        playersInScene = FindObjectsOfType<Player>();

        for (int i = 0; i < seeds.Length; i++)
        {
            GameObject choice = Instantiate(optionButton, startPoint.position + new Vector3(0f, i * spacing, 0f), Quaternion.identity, seedCratePanel.transform);
            choice.GetComponentInChildren<Text>().text = seeds[i].GetComponent<Tool>().seedName;
            images.Add(choice.GetComponent<Image>());
        }

        seedCratePanel.SetActive(false);
    }

    void MenuUp()
    {
        if (active)
        {
            selection--;
            selection = Mathf.Clamp(selection, 0, images.Count - 1);
        }
    }

    void MenuDown()
    {
        if (active)
        {
            selection++;
            selection = Mathf.Clamp(selection, 0, images.Count - 1);
        }
    }

    void Interact()
    {
        if (activePlayer != null)
        {
            if (active)
            {
                GameObject seed = Instantiate(seeds[selection], creationPoint.position, Quaternion.identity);
                seed.GetComponent<Rigidbody>().velocity = new Vector3(0f, 3f, -3f);

                foreach (Player player in playersInScene)
                {
                    player.AddToToolList(seed.GetComponent<Tool>());
                }

                selection = 0;
                seedCratePanel.SetActive(false);
                activePlayer.controlsDisabled = false;
            }
            else
            {
                active = true;
                seedCratePanel.SetActive(true);
                activePlayer.controlsDisabled = true;
            }
        }
    }

    void Cancel()
    {
        Debug.Log("Cancel");
        selection = 0;
        seedCratePanel.SetActive(false);
        activePlayer.controlsDisabled = false;
    }

    private void Update()
    {
        if (active)
        {
            seedCratePanel.SetActive(true);

            for (int i = 0; i < images.Count; i++)
            {
                images[i].color = Color.white;
            }

            images[selection].color = Color.red;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Player")
        {
            activePlayer = collision.gameObject.GetComponent<Player>();
        }
    }
}

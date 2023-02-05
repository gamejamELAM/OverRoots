using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedCrate : MonoBehaviour
{
    public Transform creationPoint;
    public GameObject seedCratePanel;
    public RectTransform startPoint;
    public GameObject optionButton;
    public GameObject[] seeds;
    float spacing = -45f;

    Player activePlayer;

    List<Image> images = new List<Image>();
    int selection = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            GameObject choice = Instantiate(optionButton, startPoint.position + new Vector3(0f, i * spacing, 0f), Quaternion.identity, seedCratePanel.transform);
            choice.GetComponentInChildren<Text>().text = seeds[i].GetComponent<Tool>().seedName;
            images.Add(choice.GetComponent<Image>());
        }

        seedCratePanel.SetActive(false);
    }

    private void Update()
    {
        if (activePlayer != null)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].color = Color.white;
            }

            images[selection].color = Color.red;

            if (Input.GetKeyDown(activePlayer.downKey))
            {
                selection++;
            }

            if (Input.GetKeyDown(activePlayer.upKey))
            {
                selection--;
            }

            if (Input.GetKeyDown(activePlayer.interactKey))
            {
                seedCratePanel.SetActive(false);

                GameObject seed = Instantiate(seeds[selection], creationPoint.position, Quaternion.identity);
                seed.GetComponent<Rigidbody>().velocity = new Vector3(0f, 3f, -3f);

                activePlayer.controlsDisabled = false;
                activePlayer = null;
            }

            selection = Mathf.Clamp(selection, 0, images.Count - 1);
        }
    }

    public void PlayerInteract(Player player)
    {
        seedCratePanel.SetActive(true);
        activePlayer = player;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SeedCrate : MonoBehaviour
{
    public Transform creationPoint;
    public GameObject seedCratePanel;
    public RectTransform startPoint;
    public GameObject optionButton;
    public GameObject[] seeds;
    float spacing = -45f;

    bool active = false;

    List<Image> images = new List<Image>();
    int selection = 0;

    public List<Player> playersInScene;

    private void Awake()
    {
        playersInScene = new List<Player>();
    }

    public Player activePlayer;

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

    public void MenuUp(Player play)
    {
        if (activePlayer != null)
        {
            if (activePlayer == play)
            {
                selection--;
                selection = Mathf.Clamp(selection, 0, images.Count - 1);

                seedCratePanel.SetActive(true);

                for (int i = 0; i < images.Count; i++)
                {
                    images[i].color = Color.white;
                }

                images[selection].color = Color.red;
            }
        }
    }

    public void MenuDown(Player play)
    {
        if (activePlayer != null)
        {
            if (activePlayer == play)
            {
                selection++;
                selection = Mathf.Clamp(selection, 0, images.Count - 1);

                seedCratePanel.SetActive(true);

                for (int i = 0; i < images.Count; i++)
                {
                    images[i].color = Color.white;
                }

                images[selection].color = Color.red;
            }
        }
    }

    public void Interact(Player play)
    {
        if (activePlayer == null)
        {
            seedCratePanel.SetActive(true);
            activePlayer = play;
            play.controlsDisabled = true;
            selection = 0;

            for (int i = 0; i < images.Count; i++)
            {
                images[i].color = Color.white;
            }

            images[selection].color = Color.red;
        } 
        else if (activePlayer == play)
        {
            GameObject seed = Instantiate(seeds[selection], creationPoint.position, Quaternion.identity);
            seed.GetComponent<Rigidbody>().velocity = new Vector3(0f, 3f, -3f);

            foreach (Player player in playersInScene)
            {
                player.AddToToolList(seed.GetComponent<Tool>());
            }

            Cancel(play);
        }
    }

    public void Cancel(Player play)
    {
        if (activePlayer != null)
        {
            if (activePlayer == play)
            {
                active = false;
                seedCratePanel.SetActive(false);
                activePlayer = null;
                play.controlsDisabled = false;
            }
        }
    }
}

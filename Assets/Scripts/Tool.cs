using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ToolType
{
    Empty,
    Hoe,
    WateringCan,
    Axe,
    Seed,
    Scythe
}

public class Tool : MonoBehaviour
{
    public ToolType toolType = ToolType.WateringCan;
    public GameObject seed;
    public string seedName;

    bool isEquipped = false;
    BoxCollider[] myColliders;
    Rigidbody myRigidbody;

    Player equippedPlayer;

    Player[] playersInScene;

    public GameObject myVisuals;

    private void Start()
    {
        playersInScene = FindObjectsOfType<Player>();
        myRigidbody = GetComponent<Rigidbody>();
        myColliders = GetComponents<BoxCollider>();
    }

    public ToolType PlayerInteract(Player player)
    {
        Equip(player);

        return toolType;
    }

    public void Equip(Player player)
    {
        if (player.myTool != null)
        {
            player.myTool.Unequip(player);
        }

        foreach (BoxCollider collider in myColliders)
        {
            collider.enabled = false;
        }

        myVisuals.SetActive(false);

        player.myTool = this;
        player.mySeed = seed;

        equippedPlayer = player;
        isEquipped = true;
    }

    public void Unequip(Player player)
    {
        player.myTool = null;
        player.mySeed = null;

        myVisuals.SetActive(true);

        transform.position = player.transform.position + Vector3.up;
        myRigidbody.velocity = new Vector3(0f, 3f, 3f);

        foreach (BoxCollider collider in myColliders)
        {
            collider.enabled = true;
        }

        equippedPlayer = null;
        isEquipped = false;
    }

    public void Consume(Player player)
    {
        player.myTool = null;
        player.mySeed = null;

        foreach (Player play in playersInScene)
        {
            play.TakeFromToolList(this);
        }

        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    Planter,
    WateringCan,
    Axe,
    Seed
}

public class Tool : MonoBehaviour
{
    public ToolType toolType = ToolType.WateringCan;
    public GameObject seed;

    bool isEquipped = false;
    BoxCollider[] myColliders;
    Rigidbody myRigidbody;

    Player equippedPlayer;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myColliders = GetComponents<BoxCollider>();
    }

    private void Update()
    {
        if (isEquipped)
        {
            transform.position = equippedPlayer.equipmentPoint.position;
        }
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

        player.myTool = this;
        player.mySeed = seed;

        equippedPlayer = player;
        isEquipped = true;
    }

    public void Unequip(Player player)
    {
        player.myTool = null;
        player.mySeed = null;

        myRigidbody.AddForce(new Vector3(0f, 200f, 0f));

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

        Destroy(gameObject);
    }
}

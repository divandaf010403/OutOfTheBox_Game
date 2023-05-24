using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsTrigger : OnItems
{
    public DataItem getItems;
    public AudioClip takeSoundEffect;

    public override void OnInteract()
    {
        //AudioSource soundEffect = GameObject.Find("SoundEffect").GetComponent<AudioSource>();
        Inventory inventory = FindObjectOfType<Inventory>();

        if(inventory.items.Count >= inventory.maxItem)
        {
            Debug.Log("Inventory Full");
            return;
        }

        inventory.AddItem(getItems);
        //soundEffect.PlayOneShot(takeSoundEffect);

        Destroy(this.gameObject);
    }
}

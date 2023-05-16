using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsUI : MonoBehaviour
{
    public Image itemImage;
    public int m_index;

    public Inventory inventory;
    public bool isCanDrop;

    public void Drop()
    {
        if (isCanDrop == false) return;

        inventory.DropItem(m_index);
    }
}

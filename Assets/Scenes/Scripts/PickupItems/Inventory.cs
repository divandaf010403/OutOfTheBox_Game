using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<DataItem> items = new List<DataItem>();

    public int maxItem;
    public ItemsUI[] itemsUI;

    private void Update()
    {
        for(int i = 0; i < itemsUI.Length; i++)
        {
            if(i < items.Count)
            {
                itemsUI[i].itemImage.sprite = items[i].items.sprite;
                itemsUI[i].itemImage.enabled = true;
                itemsUI[i].m_index = i;
                itemsUI[i].inventory = this;
                itemsUI[i].isCanDrop = true;
            }
            else
            {
                itemsUI[i].isCanDrop = false;
                itemsUI[i].itemImage.enabled = false;
            }
        }
    }

    public void AddItem(DataItem new_item)
    {
        items.Add(new_item);
        Debug.Log("Item Ditambahkan: " + new_item.items.m_name);
    }

    public void RemoveItem(DataItem removed_item)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].items.m_name == removed_item.items.m_name)
            {
                Debug.Log("Item Dihapus: " + items[i].items.m_name);
                items.RemoveAt(i);
            }
        }
    }

    public DataItem searchItem(DataItem searchItem)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].items.m_name == searchItem.items.m_name)
            {
                return items[i];
            }
        }

        return null;
    }

    public void DropItem(int m_index)
    {
        Instantiate(Resources.Load("ItemObjects/" + items[m_index].items.m_name), transform.position, Quaternion.identity);
        Debug.Log("" + items[m_index].items.m_name);
        RemoveItem(items[m_index]);
    }
}

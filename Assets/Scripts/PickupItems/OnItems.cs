using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnItems : MonoBehaviour
{
    public DataItem requiereItem;
    public PlayerMove notification;
    public virtual void OnInteract() { }
    public virtual void OnUseItem() { }
    public void removeRequire()
    {
        Debug.Log("Hapus persyaratan : " + gameObject.name);
        requiereItem = null;
    }
    //public void removeInteract()
    //{
    //    notification.textNotification = null;
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data Item", menuName = "Inventory/Data Item", order = 1)]

public class DataItem : ScriptableObject
{
    public Items items;
}

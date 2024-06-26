using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary
{
    [SerializeField]
    public List<Vector3Int> placedObjectVector3Ints = new();

    [SerializeField]
    public List<PlacementData> placedObjectPlacementDatas = new();
}

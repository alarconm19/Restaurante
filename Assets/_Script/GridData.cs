using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GridData //: ISerializationCallbackReceiver
{
    [NonSerialized]
    public Dictionary<Vector3Int, PlacementData> PlacedObjects = new();

    [SerializeField]
    public SerializableDictionary SerializableDictionary { get; set; } = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex)
    {
        var positionToOccupy = CalculeOccupiedPositions(gridPosition, objectSize);
        PlacementData data = new(positionToOccupy, id, placedObjectIndex);

        foreach (var pos in positionToOccupy.Where(pos => !PlacedObjects.TryAdd(pos, data)))
        {
            throw new InvalidOperationException($"Position already occupied. {pos}");
        }
    }

    private List<Vector3Int> CalculeOccupiedPositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        for (var x = 0; x < objectSize.x; x++)
        {
            for (var y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        var positionToOccupy = CalculeOccupiedPositions(gridPosition, objectSize);

        return positionToOccupy.All(pos => !PlacedObjects.ContainsKey(pos));
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if(PlacedObjects.TryGetValue(gridPosition, out var o) == false)
            return -1;

        return o.PlacedObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in PlacedObjects[gridPosition].OccupiedPositions)
        {
            PlacedObjects.Remove(pos);
        }
    }

    public void SavePlacedObjects()
    {
        SerializableDictionary.placedObjectVector3Ints = PlacedObjects.Keys.ToList();
        SerializableDictionary.placedObjectPlacementDatas = PlacedObjects.Values.ToList();
    }

    public void LoadPlacedObjects()
    {
        PlacedObjects = new();

        for (int i = 0; i < SerializableDictionary.placedObjectVector3Ints.Count; i++)
        {
            PlacedObjects.Add(SerializableDictionary.placedObjectVector3Ints[i], SerializableDictionary.placedObjectPlacementDatas[i]);
        }
    }
}

[Serializable]
public class PlacementData
{
    public int Id, PlacedObjectIndex;
    public List<Vector3Int> OccupiedPositions;

    public PlacementData(List<Vector3Int> occupiedPositions, int id, int placedObjectIndex)
    {
        OccupiedPositions = occupiedPositions;
        Id = id;
        PlacedObjectIndex = placedObjectIndex;
    }

    public override string ToString()
    {
        return $"Id: {Id}, PlacedObjectIndex: {PlacedObjectIndex}, OccupiedPositions: {string.Join(", ", OccupiedPositions)}";
    }
}
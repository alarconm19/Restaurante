using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private GameObject gridVisualization;

    private GridData floorData, furnitureData;

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           furnitureData,
                                           objectPlacer);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
            return;

        Vector3 mouseposition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mouseposition);

       buildingState.OnAction(gridPosition);
    }

    private void StopPlacement()
    {
        if (buildingState == null)
            return;

        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;

        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;

        Vector3 mouseposition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mouseposition);

        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }

    public int GetTablesCount()
    {
        return objectPlacer.placedObjectDataList.Where(pGB => pGB.prefabName == "TableParent" || pGB.prefabName == "BigBedParent").Count();
    }

    public string ToJson()
    {
        furnitureData.SavePlacedObjects();
        floorData.SavePlacedObjects();

        var data = new PlacementSystemData
        {
            lastPosition = inputManager.GetLastPosition(),
            database = database.objectsData,
            floorData = floorData.SerializableDictionary,
            furnitureData = furnitureData.SerializableDictionary,
            objectPlacer = objectPlacer.placedObjectDataList,
        };

        foreach (var furniture in furnitureData.PlacedObjects)
        {
            Debug.Log(furniture.Value);
        }

        return JsonUtility.ToJson(data, true);
    }

    public void FromJson(string json)
    {
        var data = JsonUtility.FromJson<PlacementSystemData>(json);

        inputManager.SetLastPosition(data.lastPosition);
        database.objectsData = data.database;

        floorData.SerializableDictionary = data.floorData;
        floorData.LoadPlacedObjects();
        furnitureData.SerializableDictionary = data.furnitureData;
        furnitureData.LoadPlacedObjects();

        objectPlacer.placedObjectDataList = data.objectPlacer;
        objectPlacer.LoadPlacedObjects();
    }

    // Clase para contener los datos serializables de PlacementSystem
    [Serializable]
    private class PlacementSystemData
    {
        public Vector3 lastPosition;
        public List<ObjectData> database;
        public SerializableDictionary floorData, furnitureData;
        public List<PlacedObjectData> objectPlacer;

        public PlacementSystemData()
        {
            lastPosition = Vector3.zero;
            database = new();
            floorData = new();
            furnitureData = new();
            objectPlacer = new();
        }
    }
}
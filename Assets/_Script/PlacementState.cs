using UnityEngine;

public class PlacementState : IBuildingState
{
    private readonly int selectedObjectIndex = -1;
    readonly int ID;
    readonly Grid grid;
    readonly PreviewSystem previewSystem;
    readonly ObjectsDatabaseSO database;
    private readonly GridData floorData, furnitureData;
    readonly ObjectPlacer objectPlacer;

    public PlacementState(int ID, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        this.ID = ID;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        if (selectedObjectIndex > -1)
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab,
                                                       database.objectsData[selectedObjectIndex].Size);
        else
            throw new System.Exception("Object with ID " + ID + " not found in database. ");
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (!placementValidity)
            return;

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition,
                                 database.objectsData[selectedObjectIndex].Size,
                                 database.objectsData[selectedObjectIndex].ID,
                                 index);

        previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), placementValidity);

        // Initialize object state and color
        PlacementSystem placementSystem = GameObject.FindObjectOfType<PlacementSystem>();
        if (placementSystem != null)
        {
            placementSystem.ChangeObjectState(index);
        }
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}

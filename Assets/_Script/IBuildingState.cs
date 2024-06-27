using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnAction(Vector3Int gridPosition);
    void UpdateState(Vector3Int gridPosition);
    public enum ObjectState
    {
        Available,
        Occupied,
        Reserved
    }

}

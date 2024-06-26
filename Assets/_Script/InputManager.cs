using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastposition;

    [SerializeField]
    private LayerMask placementLayermask;

    public event Action OnClicked, OnExit;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, placementLayermask))
            lastposition = hit.point;

        return lastposition;
    }

    public Vector3 GetLastPosition()
    {
        return lastposition;
    }

    public void SetLastPosition(Vector3 position)
    {
        lastposition = position;
    }

}
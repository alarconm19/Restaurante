using System;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    //[SerializeField]
    //private float previewYOffset = 0.06f;

    public GameObject cellIndicator;
    public GameObject previewObject;

    public Material previewMaterialPrefab;
    public Material previewMaterialInstance;

    public Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renders = previewObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer render in renders)
        {
            Material[] materials = render.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            render.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);

        if (previewObject != null)
            Destroy(previewObject);
    }

    public void UpdatePreviewPosition(Vector3 position, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }

        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;

        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red;

        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + 0, position.z);
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);

    }
}

[Serializable]
public class PreviewSystemData
{
    public Vector3 cellIndicatorPosition;
    public Vector3 previewObjectPosition;
    public Color previewMaterialColor;
    public Color cellIndicatorColor;
}
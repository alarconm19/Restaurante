using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Canvas UIEditor, UIPreview;
    public GameObject gridVisualization, BuildingSystem;

    // Método para activar el Canvas
    public void CambiarModo()
    {
        if (UIEditor.gameObject.activeSelf)
        {
            UIEditor.gameObject.SetActive(false);
            UIPreview.gameObject.SetActive(true);
            BuildingSystem.SetActive(false);
            gridVisualization.SetActive(false);
        }
        else
        {
            UIEditor.gameObject.SetActive(true);
            UIPreview.gameObject.SetActive(false);
            BuildingSystem.SetActive(true);
            gridVisualization.SetActive(true);
        }
    }
}
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Canvas UIEditor, UIPreview, Menu;
    public GameObject gridVisualization, BuildingSystem;

    public void MenuAEdicion()
    {
        Menu.gameObject.SetActive(false);
        UIEditor.gameObject.SetActive(true);
        UIPreview.gameObject.SetActive(false);
        BuildingSystem.SetActive(true);
        gridVisualization.SetActive(true);
    }

    public void VolverMenu()
    {
        Menu.gameObject.SetActive(true);
        UIEditor.gameObject.SetActive(false);
        UIPreview.gameObject.SetActive(false);
        BuildingSystem.SetActive(false);
        gridVisualization.SetActive(false);
    }

    public void CambiarModo()
    {
        if (UIEditor.gameObject.activeSelf)
        {
            UIEditor.gameObject.SetActive(false);
            UIPreview.gameObject.SetActive(true);
            BuildingSystem.SetActive(false);
            BuildingSystem.GetComponentInChildren<PlacementSystem>().StopPlacement();
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

    public void Salir()
    {
        Application.Quit();
    }
}
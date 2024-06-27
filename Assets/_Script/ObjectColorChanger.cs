using UnityEngine;

public class ObjectColorChanger : MonoBehaviour
{
    public Canvas preview;
    private Renderer objectRenderer;
    private int clickCount = 0;

    private void Start()
    {
        objectRenderer = GetComponentInChildren<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("Renderer component not found on this game object.");
        }
    }

    private void OnMouseDown()
    {
        // Encuentra el GameObject por su nombre
        var canvasObject = GameObject.Find("UIPreview");

        // Verifica si se encontró el GameObject
        if (canvasObject != null)
        {
            preview = canvasObject.GetComponent<Canvas>();
        }
        if (preview == null || !preview.gameObject.activeSelf) return;

        clickCount++;
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (objectRenderer == null) return;

        switch (clickCount)
        {
            case 1:
                objectRenderer.material.color = Color.green;
                break;
            case 2:
                objectRenderer.material.color = Color.red;
                break;
            case 3:
                objectRenderer.material.color = Color.yellow;
                clickCount = 0; // Reiniciar el contador de clics
                break;

            // default:
            //     objectRenderer.material.color = Color.white; // Set a default color, such as white
            //     clickCount = 0; // Reiniciar el contador de clics
            //     break;
        }
    }
}
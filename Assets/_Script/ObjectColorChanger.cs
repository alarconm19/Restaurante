using UnityEngine;

public class ObjectColorChanger : MonoBehaviour
{
    private Renderer objectRenderer;
    private int clickCount = 0;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("Renderer component not found on this game object.");
        }
    }

    private void OnMouseDown()
    {
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
                break;
            default:
                objectRenderer.material.color = Color.white; // Color por defecto
                clickCount = 0; // Reiniciar el contador de clics
                break;
        }
    }
}
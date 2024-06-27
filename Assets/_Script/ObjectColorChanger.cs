using UnityEngine;

public class ObjectColorChanger : MonoBehaviour
{
    public Canvas preview;
    private Renderer objectRenderer;
    private int clickCount = 0;

    private void Start()
    {
        var planoTransform = transform.Find("Plane");
        if (planoTransform != null)
        {
            if (!TryGetComponent(out objectRenderer))
            {
                Debug.LogError("Renderer component not found on this game object.");
            }
        }
        else
        {
            Debug.LogError("GameObject 'plano' not found.");
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
        // Asegurar que el material soporte transparencia
        SetMaterialTransparency(objectRenderer);

        Color newColor;
        switch (clickCount)
        {
            case 1:
                newColor = new Color(0, 1, 0, 0.5f); // Verde con transparencia
                break;
            case 2:
                newColor = new Color(1, 0, 0, 0.5f); // Rojo con transparencia
                break;
            case 3:
                newColor = new Color(1, 1, 0, 0.5f); // Amarillo con transparencia
                clickCount = 0; // Reiniciar el contador de clics
                break;
            default:
                newColor = objectRenderer.material.color; // Mantener el color actual
                break;
        }
        objectRenderer.material.color = newColor;
    }

    private void SetMaterialTransparency(Renderer renderer)
    {
        // Configurar el material para usar transparencia
        renderer.material.SetOverrideTag("RenderType", "Transparent");
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        renderer.material.SetInt("_ZWrite", 0);
        renderer.material.DisableKeyword("_ALPHATEST_ON");
        renderer.material.EnableKeyword("_ALPHABLEND_ON");
        renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        renderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
}
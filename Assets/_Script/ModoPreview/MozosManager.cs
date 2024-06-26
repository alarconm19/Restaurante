using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MozosManager : MonoBehaviour
{
    public Canvas UIPreview;
    public GameObject mozosPanel, mozoPrefab;
    public Button addButton, deleteButton;
    public Transform mozoContent;
    public PlacementSystem placementSystem;

    private List<Mozo> mozosList = new();

    [SerializeField]
    private int mozoCount = 0;

    private void Start()
    {
        // Asignar eventos click a los botones
        addButton.onClick.AddListener(AgregarMozo);
        deleteButton.onClick.AddListener(EliminarUltimoMozo);
    }

    public void AgregarMozo()
    {
        // Crear un nuevo mozo
        Mozo newMozo = new("Mozo " + mozoCount);
        mozosList.Add(newMozo);

        // Instanciar el prefab y actualizar el texto
        GameObject newMozoGO = Instantiate(mozoPrefab, mozoContent);

        var nombre = newMozoGO.GetComponentInChildren<InputField>()
                              .GetComponentInChildren<TextMeshProUGUI>();
        nombre.text = newMozo.name;

         // Obtener el número total de mesas
        var mesasActuales = placementSystem.GetTablesCount();

        // Limpiar las asignaciones anteriores
        foreach (var mozo in mozosList)
        {
            mozo.mesasAsignadas.Clear();
        }

        // Reasignar mesas de manera equitativa
        for (int i = 0; i < mesasActuales; i++)
        {
            int mozoIndex = i % mozosList.Count;
            mozosList[mozoIndex].mesasAsignadas.Add(i + 1);
        }

        // Actualizar el texto de las mesas asignadas para cada mozo
        for (int i = 0; i < mozosList.Count; i++)
        {
            var mozoGO = mozoContent.GetChild(i).gameObject;
            var mesasAsignadas = mozoGO.GetComponentInChildren<TextMeshProUGUI>();

            string mesasTexto = "Mesas: " + string.Join(" ", mozosList[i].mesasAsignadas.Select(m => "mesa " + m));
            mesasAsignadas.text = mesasTexto;
        }

        mozoCount++;
    }

    public void EliminarUltimoMozo()
    {
        if (mozosList.Count > 0)
        {
            // Eliminar el último mozo de la lista
            mozosList.RemoveAt(mozosList.Count - 1);

            // Eliminar el último mozo del UI
            Transform lastMozo = mozoContent.GetChild(mozoContent.childCount - 1);
            Destroy(lastMozo.gameObject);

            mozoCount--;
        }
    }
}

public class Mozo
{
    public string name;
    public List<int> mesasAsignadas;

    public Mozo(string name)
    {
        this.name = name;
        mesasAsignadas = new List<int>();
    }
}
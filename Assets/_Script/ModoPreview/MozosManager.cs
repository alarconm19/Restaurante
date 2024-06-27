using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MozosManager : MonoBehaviour
{
    public GameObject mozoPrefab;
    public Button addButton, deleteButton;
    public Transform mozoContent;
    public PlacementSystem placementSystem;
    private List<Mozo> mozosList = new();
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
        Mozo newMozo = new("Mozo " + (mozoCount + 1));
        mozosList.Add(newMozo);

        // Instanciar el prefab y actualizar el texto
        _ = Instantiate(mozoPrefab, mozoContent);

        UpdateAsignacion();

        mozoCount++;
    }

    public void EliminarUltimoMozo()
    {
        if (mozosList.Count > 0)
        {
            // Eliminar el último mozo de la lista
            mozosList.RemoveAt(mozosList.Count - 1);
            mozoCount--;

            // Eliminar el último mozo del UI
            Transform lastMozo = mozoContent.GetChild(mozoContent.childCount - 1);
            Destroy(lastMozo.gameObject);

            UpdateAsignacion();
        }
    }

    public void UpdateAsignacion()
    {
        // Obtener el número total de mesas
        int mesasActuales = placementSystem.GetTablesCount();

        // Limpiar las asignaciones anteriores
        foreach (var mozo in mozosList)
        {
            mozo.mesasAsignadas.Clear();
        }

        if (mozosList.Count() == 0) return;
        // Reasignar mesas de manera equitativa
        for (int i = 0; i < mesasActuales; i++)
        {
            int mozoIndex = i % mozosList.Count();
            mozosList[mozoIndex].mesasAsignadas.Add(i);
        }

        // Actualizar el texto de las mesas asignadas para cada mozo
        for (int i = 0; i < mozosList.Count; i++)
        {
            var mozoGO = mozoContent.GetChild(i).gameObject;

            // Encontrar los componentes TextMeshProUGUI específicos
            var nombreMozoTransform = mozoGO.transform.Find("NombreMozo");
            var mesasAsignadasTransform = mozoGO.transform.Find("MesasAsignadas");

            if (nombreMozoTransform != null && mesasAsignadasTransform != null)
            {
                var nombreMozoText = nombreMozoTransform.GetComponentInChildren<TextMeshProUGUI>();
                var mesasAsignadasText = mesasAsignadasTransform.GetComponent<TextMeshProUGUI>();

                if (nombreMozoText != null)
                {
                    nombreMozoText.text = mozosList[i].name;
                }

                if (mesasAsignadasText != null)
                {
                    string mesasTexto = "Mesas: " + string.Join(" ", mozosList[i].mesasAsignadas.Select(m => "Mesa " + (m + 1)));
                    mesasAsignadasText.text = mesasTexto;
                }
            }
            else
            {
                Debug.LogError("TextMeshProUGUI no encontrado en el prefab de mozo.");
            }
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
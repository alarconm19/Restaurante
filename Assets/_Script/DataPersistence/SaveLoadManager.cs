using System;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public PlacementSystem placementSystem;

    private void Start()
    {
        // Asigna el placementSystem si no está asignado desde el editor
        if (placementSystem == null)
        {
            placementSystem = FindObjectOfType<PlacementSystem>();
            if (placementSystem == null)
            {
                Debug.LogError("placementSystem component not found in the scene.");
                return;
            }
        }
        // LoadGame();
    }

    public void SaveGame()
    {
        if (placementSystem == null)
        {
            Debug.LogError("PlacementSystem is not assigned.");
            return;
        }

        var fullPath = Path.Combine(Application.persistentDataPath, "data.json");

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string json = placementSystem.ToJson();

            using FileStream stream = new(fullPath, FileMode.Create);
            using StreamWriter writer = new(stream);

            writer.Write(json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred while saving data to the file: " + fullPath + "\n" + e);
        }
    }

    public void LoadGame()
    {
        if (placementSystem == null)
        {
            Debug.LogError("PlacementSystem is not assigned.");
            return;
        }

        var fullPath = Path.Combine(Application.persistentDataPath, "data.json");

        if (!File.Exists(fullPath)) return;
        try
        {
            using FileStream stream = new(fullPath, FileMode.Open);
            using StreamReader reader = new(stream);

            string json = reader.ReadToEnd();

            placementSystem.FromJson(json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred while loading data from the file: " + fullPath + "\n" + e);
        }
    }
}
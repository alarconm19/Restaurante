using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPlacer : MonoBehaviour
{
    [NonSerialized]
    public List<GameObject> placedGameObjects = new();

    // Lista de datos serializables
    public List<PlacedObjectData> placedObjectDataList = new();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;

        // Asegurarse de que el objeto tiene un Renderer
        Renderer renderer = newObject.GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = newObject.AddComponent<Renderer>();
        }

        // Agregar el componente ObjectColorChanger
        ObjectColorChanger colorChanger = newObject.GetComponent<ObjectColorChanger>();
        if (colorChanger == null)
        {
            newObject.AddComponent<ObjectColorChanger>();
        }

        placedGameObjects.Add(newObject);

        // Guardar los datos del objeto colocado
        placedObjectDataList.Add(new PlacedObjectData(prefab.name, position));

        return placedGameObjects.Count - 1;
    }

    public int PlaceObjectV1(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;

        // Asegurarse de que el objeto tiene un Renderer
        Renderer renderer = newObject.GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = newObject.AddComponent<Renderer>();
        }

        // Agregar el componente ObjectColorChanger
        ObjectColorChanger colorChanger = newObject.GetComponent<ObjectColorChanger>();
        if (colorChanger == null)
        {
            newObject.AddComponent<ObjectColorChanger>();
        }

        placedGameObjects.Add(newObject);

        return placedGameObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;

        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;

        // También remover los datos
        placedObjectDataList[gameObjectIndex] = null;
    }

    public void LoadPlacedObjects()
    {
        foreach (var placedObjectData in placedObjectDataList)
        {
            GameObject prefab = Resources.Load<GameObject>(placedObjectData.prefabName);
            if (prefab == null)
            {
                Debug.LogError("Prefab not found: " + placedObjectData.prefabName);
                continue;
            }

            PlaceObjectV1(prefab, placedObjectData.position);
        }
    }

    public List<PlacedObjectData> GetPlacedObjects()
    {
        return placedObjectDataList;
    }
}

// Clase para contener los datos serializables de los objetos colocados
[Serializable]
public class PlacedObjectData
{
    public string prefabName;
    public Vector3 position;

    public PlacedObjectData(string prefabName, Vector3 position)
    {
        this.prefabName = prefabName;
        this.position = position;
    }
}

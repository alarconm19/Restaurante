using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class ObjectPlacer : MonoBehaviour
{
    [NonSerialized]
    public List<GameObject> placedGameObjects = new();

    // Lista de datos serializables
    public List<PlacedObjectData> placedObjectDataList = new();

    int contMesas = 1;

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;

        placedGameObjects.Add(newObject);

        // Guardar los datos del objeto colocado
        placedObjectDataList.Add(new PlacedObjectData(prefab.name, position));

        if(prefab.name.Contains("TableRectangularParentV2") || prefab.name.Contains("RoundTableParentV2"))
        {
            // Buscar el objeto "plano" y obtener su Renderer
            var planoTransform = newObject.transform.Find("Plane");
            if (planoTransform != null)
            {
                // Asegurarse de que el objeto tiene un Renderer
                if (!newObject.TryGetComponent<Renderer>(out _))
                {
                    newObject.AddComponent<Renderer>();
                }

                // Agregar el componente ObjectColorChanger
                if (!newObject.TryGetComponent<ObjectColorChanger>(out _))
                {
                    newObject.AddComponent<ObjectColorChanger>();
                }

                //_ = newObject.AddComponent<ObjectColorChanger>();
            }
            else
            {
                Debug.LogError("GameObject 'plano' not found.");
            }


            // Buscar el objeto "numero" dentro del prefab instanciado
            var numeroTransform = newObject.transform.Find("Numero");
            if (numeroTransform != null)
            {
                if (numeroTransform.TryGetComponent<TextMeshPro>(out var numeroText))
                {
                    // Aquí puedes modificar el texto del TextMeshProUGUI
                    numeroText.text = contMesas.ToString();

                    contMesas++;
                }
                else
                {
                    Debug.LogWarning("TextMeshProUGUI no encontrado en el objeto 'numero'");
                }
            }
            else
            {
                Debug.LogWarning("Objeto 'numero' no encontrado en el prefab");
            }
        }

        return placedGameObjects.Count - 1;
    }

    public int PlaceObjectV1(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;

        placedGameObjects.Add(newObject);

        if(prefab.name.Contains("TableParentV2") || prefab.name.Contains("RoundTableParentV2"))
        {
            // Agregar el componente ObjectColorChanger y configurarlo
            _ = newObject.AddComponent<ObjectColorChanger>();

            // Buscar el objeto "numero" dentro del prefab instanciado
            var numeroTransform = newObject.transform.Find("Numero");
            if (numeroTransform != null)
            {
                if (numeroTransform.TryGetComponent<TextMeshPro>(out var numeroText))
                {
                    // Aquí puedes modificar el texto del TextMeshProUGUI
                    numeroText.text = contMesas.ToString();

                    contMesas++;
                }
                else
                {
                    Debug.LogWarning("TextMeshProUGUI no encontrado en el objeto 'numero'");
                }
            }
            else
            {
                Debug.LogWarning("Objeto 'numero' no encontrado en el prefab");
            }
        }

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

        contMesas--;
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

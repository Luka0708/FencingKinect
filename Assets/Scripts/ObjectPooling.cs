using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public Obstacle[] obstaclePrefabs;
    private GameObject[,] spawnedObjects;
    private int numberInstantiation = 5;    
    private int numberOfObstaclesTypes;

    private void Awake()
    {
        numberOfObstaclesTypes = obstaclePrefabs.Length;
        spawnedObjects = new GameObject[numberOfObstaclesTypes, numberInstantiation];
    }

    private void Start()
    {
        int index = 0;
        foreach (Obstacle obs in obstaclePrefabs)
        {
            for (int i = 0; i < numberInstantiation; i++)
            {
                GameObject o = Instantiate(obs.gameObject, this.transform);
                spawnedObjects[index, i] = o;
                o.SetActive(false);
            }
            index += 1;
        }
    }

    public int GetNumberOfObstaclesTypes()
    {
        return numberOfObstaclesTypes;
    }

    public GameObject getObstacle(int index)
    {
        for (int i = 0; i < numberInstantiation; i++)
        {
            if (!spawnedObjects[index, i].activeInHierarchy)
            {
                return spawnedObjects[index, i];
            }
        }
        return null;
    }
}

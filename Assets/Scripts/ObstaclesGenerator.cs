using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{
    [SerializeField] private ObjectPooling objectPooling;
    [SerializeField] private Transform[] positions;
    private float timePerGenerate;
    private float timer;
    private int numberOfObstaclesTypes;

    private void Start()
    {
        //Object Pooling
        numberOfObstaclesTypes = objectPooling.GetNumberOfObstaclesTypes();

        //Timer
        timer = 0;
        timePerGenerate = Random.Range(2f, 4f);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timePerGenerate)
        {
            timer = 0f;

            //obstacle variant
            int indexObject = Random.Range(0, numberOfObstaclesTypes);
            GameObject obstacle = objectPooling.getObstacle(indexObject);
            obstacle.SetActive(true);

            //position
            int indexPosition = Random.Range(0, positions.Length);
            obstacle.transform.position = positions[indexPosition].position;

            //rotation
            float rot = Random.Range(-40f, 40f);
            obstacle.transform.eulerAngles = new Vector3(0, 0, rot);
        }
    }
}

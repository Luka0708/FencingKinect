using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float timer = 0f;
    private float timeToDeactivate = 8f;
    private float fallSpeed = 0f;

    private void Start()
    {
        fallSpeed = Random.Range(1.5f, 4.0f);
    }

    private void Update()
    {
        transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);


        timer += Time.deltaTime;

        if (timer > timeToDeactivate)
        {
            timer = 0f;
            this.gameObject.SetActive(false);
        }
    }
}

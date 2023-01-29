using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SphereController : MonoBehaviour
{
    public float speed = 5f;
    public Transform target;
    //public Image fillImage;
    public Slider Slider;
    private bool isHovering;
    private float fillAmount;
    private void Start()
    {
        

    }

    void Update()
    {
        // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to world space
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Set the position of the sphere to the mouse position
        transform.position = new Vector3(worldMousePosition.x,worldMousePosition.y,0);

        // Check if the sphere is hovering over the target
        if (Vector2.Distance(transform.position, target.position) < 0.5f)
        {
            isHovering = true;
        }
        else
        {
            isHovering = false;
        }

        // Update the fill image
        if (isHovering)
        {
            Slider.value += Time.deltaTime / 2;

            if (Slider.value >= 1f)
            {
                Debug.Log("fill is one");
                //SceneManager.LoadScene("MainGame");
                SceneManager.LoadScene(1);
            }
        }
        else
        {
            Slider.value = 0;
        }
    }
}



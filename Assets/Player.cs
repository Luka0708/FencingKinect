using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0.5f * Time.deltaTime, 0, 0);
    }
}

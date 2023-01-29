using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] Image[] heartContainers;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] public int currentHealth;
    private int maxHealth;


    private void Start()
    {
        maxHealth = heartContainers.Length;

        PlayerHealth = maxHealth;

    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        PlayerHealth--;
    //    }
//
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        PlayerHealth++;
    //    }
    //}


    public int PlayerHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = Mathf.Clamp (value, 0, maxHealth);
            SetHealth(currentHealth); 
            if (currentHealth == 0)
            {
                Debug.Log("u are dead :))))))");
                //TODO: Add correct scene
                //SceneChanger.LoadScene(SceneChanger.SceneEnum.LoginMenu);
            }
        }
    }

    private void SetHealth(int health)
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (i < currentHealth)
            {
                //heartContainers[i].sprite = fullHeart;
                heartContainers[i].enabled = true;
            }
            else
            {
                //heartContainers[i].sprite = emptyHeart;
                heartContainers[i].enabled = false;


            }

            //if (i < maxHealth)
            //{
            //    heartContainers[i].enabled = true;
            //}
            //else
            //{
            //    heartContainers[i].enabled = false;
            //}
        }
    }

    


}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Update()
    {
        // Trouve PlayerController dans la scène
        PlayerController playerController = FindObjectOfType<PlayerController>();

        // Vérifie si l'instance a été trouvée
        if (playerController != null)
        {
            float floatValue = playerController.PlayerHealth;
           
            health =(int)Math.Ceiling(floatValue);

            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < health)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }

                if (i < numOfHearts)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }
            }
        }
        else
        {
            Debug.LogError("Script PlayerController non trouvé dans la scène.");
        }
    }
}
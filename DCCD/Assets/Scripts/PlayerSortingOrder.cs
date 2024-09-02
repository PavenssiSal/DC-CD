using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is powered by OpenAI's ChatGPT model...Aka I didn't write shit
public class PlayerSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Adjust sorting order based on Y position
        // The lower the Y value, the higher the sorting order
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}

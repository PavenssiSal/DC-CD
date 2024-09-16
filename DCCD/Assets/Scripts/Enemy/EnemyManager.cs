using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject reserve;


    // Start is called before the first frame update
    void Start()
    {
        reserve.SetActive(false);
    }
    private void Update()
    {
        if (PlayerController.enemyKillCount >= 3)
        {
            reserve.SetActive(true);
        }
        if (PlayerController.enemyKillCount < 3) 
        {
            reserve.SetActive(false);
        }
    }
}

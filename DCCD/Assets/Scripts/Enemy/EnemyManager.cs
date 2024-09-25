using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject reserve;
    public GameObject boss;
    public GameObject WinZone;


    // Start is called before the first frame update
    void Start()
    {
        reserve.SetActive(false);
        boss.SetActive(false);
        WinZone.SetActive(false);
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
        if (PlayerController.enemyKillCount >= 8) 
        {
            boss.SetActive(true);
        }
        if (PlayerController.enemyKillCount < 8) 
        {
            boss.SetActive(false);
        }
        if (PlayerController.enemyKillCount >= 9) 
        {
            WinZone.SetActive(true);
        }
    }
}

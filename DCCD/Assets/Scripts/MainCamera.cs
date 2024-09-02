using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime;
using System.Security.Cryptography;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    //Vasen reuna jonka yli ei saa menna
    private float tLX;
    //Vasen ylareuna jonka yli ei saa menna
    private float tLY;
    //Oikea reuna jonka yli ei saa menna
    private float bRX;
    //Oikea ylareuna jonka yli ei saa menna
    private float bRY;

    private Transform target;

    void Awake()
    {
        //Tehdaan pelaajasta kohde
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SetBound(GameObject map)
    {
        //Apumuuttujan kartta
        SuperTiled2Unity.SuperMap config = map.GetComponent<SuperTiled2Unity.SuperMap>();

        //Kameran koko
        float cameraSize = Camera.main.orthographicSize;
        //Kamera suhdeluku kerrottuna koolla
        float aspectRatio = Camera.main.aspect * cameraSize;

        //Lasketaan rajat
        tLX = map.transform.position.x + aspectRatio;
        tLY = map.transform.position.y - cameraSize;
        bRX = map.transform.position.x + config.m_Width - aspectRatio;
        bRY = map.transform.position.y - config.m_Height + cameraSize;
    }

    private void LateUpdate()
    {
        //Kameran seuraa eika mene kartan rajojen yli
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, tLX, bRX),
            Mathf.Clamp(target.position.y, bRY, tLY),
            transform.position.z
            );
    }
}

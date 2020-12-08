using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnim : MonoBehaviour
{
    [Header("Settings")] 
    public float perlinScale = 1;
    public float waveSpeed = 1;
    public float waveHeight = 1;

    public Transform InitPos;
    // Start is called before the first frame update
    void Start()
    {
        //InitPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        

        float pX = (transform.position.x * perlinScale) + (Time.timeSinceLevelLoad * waveSpeed);
        float pY = (transform.position.y * perlinScale) + (Time.timeSinceLevelLoad * waveSpeed);

        float perlinX = Mathf.PerlinNoise(pX, pY) * waveHeight;
        float perlinY = Mathf.PerlinNoise(pY, pX) * waveHeight;

        transform.position = new Vector3(InitPos.position.x + perlinX, InitPos.position.y + perlinY, InitPos.position.z);
    }
}

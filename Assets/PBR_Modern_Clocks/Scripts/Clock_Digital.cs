using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock_Digital : MonoBehaviour
{
    public static Clock_Digital instance;

    public float speed = 1.0f;
    public int minute;      // start Time
    public int hour;        // start Time

    public bool twelveHourTime = true;
    public bool pm = false;

    public Renderer ampmRenderer;

    Renderer objRenderer;

    float delay;
    float tickDelay;
    bool tick;

    int mainTexID;
    int emissionID;

    Vector2 vecOffset;

    void Start()
    {
        instance = this;
        objRenderer = GetComponent<Renderer>();
        mainTexID = Shader.PropertyToID("_MainTex");
        emissionID = Shader.PropertyToID("_EmissionMap");
    }

    private void SetAMPM()
    {
        if (ampmRenderer)
            ampmRenderer.enabled = pm;
    }

    void Update()
    {
        delay -= Time.deltaTime * speed;
        if (delay < 0.0f)
        {
            delay = 1.0f;
            minute++;
            if (minute >= 60)
            {
                minute = 0;
                hour++;
                if (hour >= 24)
                    hour = 0;
                if (twelveHourTime && hour > 12)
                {
                    hour = 1;
                    pm = !pm;
                    SetAMPM();
                }
            }
        }

        vecOffset.x = 0;

        //--------------------------------------------------------------------------------------------------------------------------
        // Minute 1er
        vecOffset.y = 0.0f - 0.1f * (float)(minute % 10);
        objRenderer.materials[4].SetTextureOffset(mainTexID, vecOffset);
        objRenderer.materials[4].SetTextureOffset(emissionID, vecOffset);

        // Minute 10er
        vecOffset.y = 0.0f - 0.1f * (float)((minute / 10) % 10);
        objRenderer.materials[3].SetTextureOffset(mainTexID, vecOffset);
        objRenderer.materials[3].SetTextureOffset(emissionID, vecOffset);

        // Hour 1er
        vecOffset.y = 0.0f - 0.1f * (float)(hour % 10);
        objRenderer.materials[1].SetTextureOffset(mainTexID, vecOffset);
        objRenderer.materials[1].SetTextureOffset(emissionID, vecOffset);

        // Hour 10er
        vecOffset.y = 0.0f - 0.1f * (float)((hour / 10) % 10);
        objRenderer.materials[2].SetTextureOffset(mainTexID, vecOffset);
        objRenderer.materials[2].SetTextureOffset(emissionID, vecOffset);

        //--------------------------------------------------------------------------------------------------------------------------
        tickDelay -= Time.deltaTime;
        if (tickDelay < 0.0f)
        {
            tickDelay += 0.5f;
            tick = !tick;
            if (tick)
            {
                vecOffset.y = 0.0f;
                objRenderer.materials[5].SetTextureOffset(mainTexID, vecOffset);
                objRenderer.materials[5].SetTextureOffset(emissionID, vecOffset);
            }
            else
            {
                vecOffset.y = 0.9f;
                objRenderer.materials[5].SetTextureOffset(mainTexID, vecOffset);
                objRenderer.materials[5].SetTextureOffset(emissionID, vecOffset);
            }
        }
    }
}
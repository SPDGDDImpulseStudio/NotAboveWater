using UnityEngine;
using System.Collections;

public class Underwater : MonoBehaviour
{

    //This script enables underwater effects. Attach to main camera.

    //Define variable
    public int underwaterLevel;

    //The scene's default fog settings
    private bool defaultFog;
    private Color defaultFogColor;
    private float defaultFogDensity;
    private Material defaultSkybox;
    private Material noSkybox;
    private Color aboveWaterColor;
    private bool isUnderwater;

    public Color underwaterColor;
    public float fogDensity;

    void Start()
    {
        defaultFog = RenderSettings.fog;
        defaultFogColor = RenderSettings.fogColor;
        defaultFogDensity = RenderSettings.fogDensity;
        defaultSkybox = RenderSettings.skybox;
        //underwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.5f);

        //Set the background color
        //GetComponent<Camera>().backgroundColor = new Color(0, 0.4f, 0.7f, 1);
    }

    void Update()
    {
        if ((transform.position.y < underwaterLevel) != isUnderwater)
        {
            isUnderwater = transform.position.y < underwaterLevel;
            if (isUnderwater)
                UnderwaterSettings();
            if (!isUnderwater)
                AboveWaterSettings();
        }

        //if (transform.position.y < underwaterLevel)
        //{   
        //    RenderSettings.fog = true;
        //    RenderSettings.fogColor = new Color(0, 0.4f, 0.7f, 0.6f);
        //    RenderSettings.fogDensity = 0.05f;
        //    RenderSettings.skybox = noSkybox;
        //} else
        //{
        //    RenderSettings.fog = defaultFog;
        //    RenderSettings.fogColor = defaultFogColor;
        //    RenderSettings.fogDensity = defaultFogDensity;
        //    RenderSettings.skybox = defaultSkybox;
        //}
    }

    void UnderwaterSettings()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = fogDensity;
    }

    void AboveWaterSettings()
    {
        RenderSettings.fog = defaultFog;
        RenderSettings.fogColor = defaultFogColor;
        RenderSettings.fogDensity = defaultFogDensity;
    }
}
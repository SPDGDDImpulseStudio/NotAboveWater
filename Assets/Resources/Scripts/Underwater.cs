using UnityEngine;
using System.Collections;
using UnityEngine.PostProcessing;
using UnityStandardAssets.ImageEffects;


[RequireComponent(typeof(PostProcessingBehaviour))]
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

    PostProcessingProfile pPProfile;
    SunShafts shaft;

    void Start()
    {
        //Debug.Log("AH");
        shaft = GetComponent<SunShafts>();
        //behaviour = GetComponent<PostProcessingBehaviour>();
        defaultFog = RenderSettings.fog;
        defaultFogColor = RenderSettings.fogColor;
        defaultFogDensity = RenderSettings.fogDensity;
        defaultSkybox = RenderSettings.skybox;
        //underwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.5f);

        //Set the background color
        //GetComponent<Camera>().backgroundColor = new Color(0, 0.4f, 0.7f, 1);
    }

    void OnEnable()
    {
        var behaviour = GetComponent<PostProcessingBehaviour>();

        if (behaviour.profile == null)
        {
            enabled = false;
            return;
        }

        pPProfile = Instantiate(behaviour.profile);
        behaviour.profile = pPProfile;
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
    }

    void UnderwaterSettings()
    {
        shaft.enabled = true;
        pPProfile.colorGrading.enabled = true;
        RenderSettings.fog = true;
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = fogDensity;
    }

    void AboveWaterSettings()
    {
        shaft.enabled = false;
        pPProfile.colorGrading.enabled = false;
        RenderSettings.fog = defaultFog;
        RenderSettings.fogColor = defaultFogColor;
        RenderSettings.fogDensity = defaultFogDensity;
    }
}
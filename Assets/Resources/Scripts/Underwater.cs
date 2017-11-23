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

    public bool TurnOn;
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
        EnableProf();
    }

    public void Init()
    {
        //Debug.Log("AH");
        shaft = GetComponent<SunShafts>();
        //behaviour = GetComponent<PostProcessingBehaviour>();
        defaultFog = RenderSettings.fog;
        defaultFogColor = RenderSettings.fogColor;
        defaultFogDensity = RenderSettings.fogDensity;
        defaultSkybox = RenderSettings.skybox;
        //underwaterColor = new Color(0.22f, 0.65f, 0.77f
    }

    public void EnableProf()
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
    public void UnderwaterSettings()
    {
        if (shaft == null) Init();
        shaft.enabled = true;
        if (pPProfile == null)
            EnableProf();
        pPProfile.colorGrading.enabled = true;
        RenderSettings.fog = true;
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = fogDensity;
    }

    public void AboveWaterSettings()
    {
        if (shaft == null) Init();
        shaft.enabled = false;
        if (pPProfile == null)
            EnableProf();                         //Disable Whole Sun Shaft
        pPProfile.colorGrading.enabled = false;             //Disables Color Grading Settings ONLY. Duplicate and change the settings you want accordingly
        RenderSettings.fog = defaultFog;
        RenderSettings.fogColor = defaultFogColor;
        RenderSettings.fogDensity = defaultFogDensity;
    }
}
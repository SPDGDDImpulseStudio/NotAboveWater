using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


public class SceneChanger : ISingleton<SceneChanger> {
 
    [Range(1f, 5f)]
    public float fadeSpeed;
    public Canvas sceneChangingCanvas;
    public Image image;
    public Text text;
    public bool transitting = false;
    private bool levelLoaded = false;

    Canvas currSceneCanvas;
    bool firstLoad = true;

    //First Time's Attributes
    bool haventRepeat = true;
    public Image instructionPanel;
    public Text instructionText;
    public Button button;
    public Text pressAny;
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }
    void Start()
    {
        //allSingletons = new List<GameObject>()
        //{
        //    AudioManager.Instance.gameObject,
        //    CircleManager.Instance.gameObject,
        //    CratesManager.Instance.gameObject,
        //    GameManager.Instance.gameObject,
        //    Player.Instance.gameObject,
        //    PoolManager.Instance.gameObject,
        //    Stats.Instance.gameObject,
        //};
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i] != sceneChangingCanvas)
            {
                currSceneCanvas = canvases[i];
                break;
            }
        }
    } 
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    public void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        levelLoaded = true;
        StartCoroutine(WhenFaderFades(scene.buildIndex));
        if (firstLoad) firstLoad = false;

        Canvas[] canvases = FindObjectsOfType<Canvas>();
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i] != sceneChangingCanvas)
            {
                currSceneCanvas = canvases[i];
                break;
            }
        }
        ToCallWhenSceneLoad();

    }
    List<GameObject> allSingletons;


    public System.Action ToCallWhenSceneLoad;
    void RefreshSingletons()
    {
        

    }

    IEnumerator WhenFaderFades(int levelIndex)
    {
        yield return new WaitUntil(() => !levelLoaded);
        
        switch (levelIndex)
        {
            case 0:
                break; //Main Menu
                           //Somewhere i reset the whole thing i needa turn all singletons except this off and on again.
                           //In a sense, this becomes the 'gameManager'
            case 1: break;
            case 2: break;
            case 3: break;
        }

        Debug.Log(levelIndex);
    }
    public void Fading(string toLoad)
    {
        switch (toLoad)
        {
            case "Credit":          StartFadeCoroutine(1);  break;
            case "New Game":        StartFadeCoroutine(2);  break;
            case "Instructions":    StartFadeCoroutine(3);  break;
            default:                StartFadeCoroutine(0); break;
        }
    }

    public void Fading(int toLoad)
    {
        StartFadeCoroutine(toLoad);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
    
    void StartFadeCoroutine(int toLoad)
    {
        if (!this.gameObject.activeInHierarchy)
            this.gameObject.SetActive(true);

        image.color = Color.clear;
        StartCoroutine(FadeIn(toLoad));
    }

    IEnumerator PopUpTutorial()
    {
        if (!instructionPanel.gameObject.activeInHierarchy)   instructionPanel.gameObject.SetActive(true);
        if (!instructionText.gameObject.activeInHierarchy) instructionText.gameObject.SetActive(true);
        while (instructionPanel.color != Color.white)
        {
            instructionPanel.color = Color.Lerp(instructionPanel.color, Color.white, Time.deltaTime * fadeSpeed);
            yield return null;
        }
        pressAny.gameObject.SetActive(true);
        if (!button.gameObject.activeInHierarchy) button.gameObject.SetActive(true);
        button.onClick.AddListener(Pressed);
    }
    void Pressed()
    {
        StartCoroutine(FadeOut());
        
    }
    IEnumerator FadeOut()
    {
        while (instructionPanel.color != Color.black)
        {
            instructionPanel.color = Color.Lerp(instructionPanel.color, Color.black, Time.deltaTime * fadeSpeed);
            yield return null;
        }
        instructionPanel.gameObject.SetActive(false); instructionText.gameObject.SetActive(false); button.gameObject.SetActive(false);
        haventRepeat = false;
    }


    IEnumerator FadeIn(int levelToLoad)
    {
        if (transitting) yield break;
        transitting = true;

        int temp = sceneChangingCanvas.sortingOrder;
        sceneChangingCanvas.sortingOrder = 1;
        currSceneCanvas.sortingOrder = 0;
        float tempSpd = fadeSpeed + Random.Range(0.6f, 1f);
        while (image.color != Color.black)
        {
            image.color = Color.Lerp(image.color, Color.black, Time.deltaTime * tempSpd);
            yield return null;
        }
        image.color = Color.black;


        if (haventRepeat) StartCoroutine(PopUpTutorial());

        yield return new WaitUntil(() => !haventRepeat);
        StartCoroutine(LoadText(3f, levelToLoad));

 
        yield return new WaitUntil(() => levelLoaded);
        //In Between should have a while, but the whole level is so fucking huge the game hangs liek crazy
        yield return new WaitUntil(() => loadText);
        loadText = false;

        //Load Text
        tempSpd = fadeSpeed + Random.Range(1.7f, 2.6f);

        while (image.color != Color.clear)
        {
            image.color = Color.Lerp(image.color, Color.clear, Time.deltaTime * tempSpd);
            yield return null;
        }
        sceneChangingCanvas.sortingOrder = temp;
        transitting = false;
        levelLoaded = false;
        gameObject.SetActive(false);

    }

    IEnumerator LoadText(float _seconds, int toLoad)
    {
        if (!text.gameObject.activeInHierarchy) text.gameObject.SetActive(true);
        text.enabled = true;
        int currInt = 0; 
        string[] loadingText = new string[4]
        {
            "Loading.",
            "Loading..",
            "Loading....",
            "Loading....."
        };

        float seconds = _seconds, nowTimer = 0;
        float perUpdate = 0.4f, updateTimer = 0;

        SceneManager.LoadScene(toLoad);
        while (true)
        {
            if (nowTimer > _seconds) break;

            if (updateTimer > perUpdate)
            {
                text.text = loadingText[currInt];
                updateTimer = 0;
            }

            if (currInt == loadingText.Length - 1) currInt = 0;
            else currInt++;

            nowTimer += Time.deltaTime;
            updateTimer += Time.deltaTime;
            yield return null;
        }
        loadText = true;
        text.enabled = false;
    }
    bool loadText = false;

}

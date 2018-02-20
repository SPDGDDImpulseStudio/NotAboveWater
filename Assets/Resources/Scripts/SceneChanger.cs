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

    public GameObject inputParent;
    public InputField saveName;

    public System.Action ToCallWhenSceneLoad;

    //First Time's Attributes]
    [Header("[FIRST TIMER ATTRIBUTES]")]
    public Image instructionPanel;
    public Text instructionText;
    public Button button;
    public Text pressAny;
    bool haventRepeat = true;
    #region Useful Functions

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }
    void Start()
    {
        ChangeOfCurrScene();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    public void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeOfCurrScene();
        if (currSceneCanvas)
            currSceneCanvas.sortingOrder = 0;
        levelLoaded = true;
        StartCoroutine(WhenFaderFades(scene.buildIndex));
        if (firstLoad) firstLoad = false;

        ToCallWhenSceneLoad();
    }

    void OnDestroy()
    {
        sceneChangingCanvas.sortingOrder = 0;
        transitting = false;
        GetComponent<SceneChanger>().image.color = Color.clear;
    }
    void ChangeOfCurrScene()
    {
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


    IEnumerator WhenFaderFades(int levelIndex)
    {
        yield return new WaitUntil(() => !levelLoaded);

        //switch (levelIndex)
        //{
        //    case 0:
        //        break; //Main Menu
        //               //Somewhere i reset the whole thing i needa turn all singletons except this off and on again.
        //               //In a sense, this becomes the 'gameManager'
        //    case 1: break;
        //    case 2: break;
        //    case 3: break;
        //}

        //Debug.Log(levelIndex);
    }
    public void Fading(string toLoad)
    {
        switch (toLoad)
        {
            case "Credit": Instance.StartFadeCoroutine(1); break;
            case "New Game": Instance.StartFadeCoroutine(2); break;
            case "Instructions": Instance.StartFadeCoroutine(3); break;
            default: Instance.StartFadeCoroutine(0); break;
        }
    }

    public void Fading(int toLoad)
    {
        Instance.StartFadeCoroutine(toLoad);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    #endregion

    void StartFadeCoroutine(int toLoad)
    {
        if (!Instance.gameObject.activeInHierarchy)
            Instance.gameObject.SetActive(true);
        if(currSceneCanvas == null)
        ChangeOfCurrScene();
        image.color = Color.clear;
        Instance.StartCoroutine(FadeIn(toLoad));
    }

    #region OneTimedTutorial

    IEnumerator PopUpTutorial()
    {
        if (!instructionPanel.gameObject.activeInHierarchy) instructionPanel.gameObject.SetActive(true);
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
        StartCoroutine(OneTimedUseFadeOut());
    }

    IEnumerator OneTimedUseFadeOut()
    {
        while (instructionPanel.color != Color.black)
        {
            instructionPanel.color = Color.Lerp(instructionPanel.color, Color.black, Time.deltaTime * fadeSpeed);
            yield return null;
        }
        instructionPanel.gameObject.SetActive(false); instructionText.gameObject.SetActive(false); button.gameObject.SetActive(false);
        haventRepeat = false;
    }

    #endregion

    IEnumerator FadeIn(int levelToLoad)
    {
        if (transitting) yield break;
        transitting = true;

        int temp = sceneChangingCanvas.sortingOrder;
        sceneChangingCanvas.sortingOrder = 2;
        currSceneCanvas.sortingOrder = 0;
        float tempSpd = fadeSpeed + Random.Range(0.6f, 1f);
        while (image.color != Color.black)
        {
            image.color = Color.Lerp(image.color, Color.black, Time.deltaTime * tempSpd);
            yield return null;
        }
        image.color = Color.black;

        if (levelToLoad == 0) {
            Player.Instance.playerCanvas.gameObject.SetActive(false);
            inputParent.SetActive(true);
            someRnd = true;
        }
        StartCoroutine(FadeOut(levelToLoad, temp));
    }
   
    public void PostTypingName(string _name)
    {
        Stats.Instance.SaveStats(_name);
        someRnd = false;
    }

    bool someRnd = false;
    IEnumerator FadeOut(int levelToLoad, int temp) {
        //if (haventRepeat) StartCoroutine(PopUpTutorial());

        //yield return new WaitUntil(() => !haventRepeat);
        yield return new WaitUntil(() => !someRnd);
        StartCoroutine(LoadText(3f, levelToLoad));
        
        yield return new WaitUntil(() => levelLoaded);
        //In Between should have a while, but the whole level is so fucking huge the game hangs liek crazy
        yield return new WaitUntil(() => loadText);
        loadText = false;

        //Load Text
        float tempSpd = fadeSpeed + Random.Range(1.7f, 2.6f);

        while (image.color != Color.clear)
        {
            image.color = Color.Lerp(image.color, Color.clear, Time.deltaTime * tempSpd);
            yield return null;
        }
        sceneChangingCanvas.sortingOrder = temp;
        currSceneCanvas.sortingOrder = 1;
        transitting = false;
        levelLoaded = false;
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

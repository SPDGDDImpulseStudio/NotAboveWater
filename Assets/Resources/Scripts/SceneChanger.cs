using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : ISingleton<SceneChanger> {
    
    [Range(1f, 5f)]
    public float fadeSpeed;
    public Canvas canvas;
    public Image image;
    public Text text;
    public bool transitting = false;
    private bool levelLoaded = false;

    bool firstLoad = true;
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
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

    IEnumerator FadeIn(int levelToLoad)
    {
        if (transitting) yield break;
        transitting = true;

        int temp = canvas.sortingOrder;
        canvas.sortingOrder = 1;

        float tempSpd = fadeSpeed + Random.Range(0.6f, 1f);
        while (image.color != Color.black)
        {
            image.color = Color.Lerp(image.color, Color.black, Time.deltaTime * tempSpd);
            yield return null;
        }
        image.color = Color.black;

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
        canvas.sortingOrder = temp;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Text.RegularExpressions;

#endif

[ExecuteInEditMode]
public class SceneCamera : MonoBehaviour {
    Camera cam;
    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }
#if UNITY_EDITOR 
    [MenuItem("Tools/CameraSnapToGO")]
    public static void CameraSnap()
    {
        if (Selection.activeGameObject)
        {           //SceneView.lastActiveSceneView.pivot = Selection.activeGameObject.transform.position;
            SceneView.lastActiveSceneView.pivot = Selection.activeGameObject.transform.localPosition;
            SceneView.lastActiveSceneView.rotation = Selection.activeGameObject.transform.localRotation;
            //SceneView.lastActiveSceneView.AlignViewToObject(Selection.activeGameObject.transform);

        }
    }

    [MenuItem("Tools/PlacePos")]
    public static void Placement()
    {
        if (Selection.gameObjects.Length == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                Debug.Log(Selection.gameObjects[i].name + " " + i);
            }
            Vector3 pos = Selection.gameObjects[2].transform.position - Selection.gameObjects[1].transform.position;
            Selection.gameObjects[0].transform.position = pos;
        }
    }
    [MenuItem("Tools/TurnOn")]
    public static void TurnOnProfile()
    {
        Underwater underWater = FindObjectOfType<Underwater>();

        if (underWater == null) return;
        underWater.UnderwaterSettings();
    }

    [MenuItem("Tools/TurnOff")]
    public static void TurnOffProfile()
    {
        Underwater underWater = FindObjectOfType<Underwater>();
        if (underWater == null) return;

        underWater.AboveWaterSettings();
    }

    //[MenuItem("Tools/Group")]
    //public static void GroupUp()
    //{
    //    List<string> names = new List<string>();

    //    List<GameObject> parentGO = new List<GameObject>(UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects());
    //    List<GameObject> assetGO = new List<GameObject>(
    //        Resources.LoadAll<GameObject>("Asset/Prefabs")

    //        );

    //    Debug.Log(assetGO.Count);
    //    for (int i = 0; i < assetGO.Count; i++)
    //    {
    //        //Debug.Log(assetGO[i].name);
    //        string charsOnly = Regex.Replace(assetGO[i].name, @"\W", "");

    //        List<GameObject> newArray = parentGO.FindAll(
    //            x => IsMatchOn2Characters(assetGO[i].name, x.name)

    //            );

    //        Debug.Log(charsOnly + " " + assetGO[i].name + " " + newArray.Count);
    //    }
    //}

    //public static bool IsMatchOn2Characters(string a, string b)
    //{
    //    string s1 = a.ToLowerInvariant();
    //    string s2 = b.ToLowerInvariant();

    //    for (int i = 0; i < s1.Length - 1; i++)
    //    {
    //        if (s2.IndexOf(s1.Substring(i, 2)) >= s1.Length)
    //        {
    //            //Debug.Log(s2.IndexOf(s1.Substring(i, 2)));
    //            return true; // match
    //        }
    //    }

    //    return false; // no match
    //}


    
#endif
}

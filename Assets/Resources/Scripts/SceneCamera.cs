using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 
public class SceneCamera : MonoBehaviour {
    Camera cam;
    // Use this for initialization
    void Start() {
       }

    // Update is called once per frame
    void Update() {
     
    }
#if UNITY_EDITOR 
    [ExecuteInEditMode]
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
#endif 
}

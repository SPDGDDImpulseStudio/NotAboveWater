using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class SceneOneCall : MonoBehaviour {

    public Camera currCamera;
    public PlayableDirector introPD;
    public PlayableDirector bossPD;
	// Use this for initialization
	void Start () {

        List<Camera> z = new List<Camera>(FindObjectsOfType<Camera>());
        Player.Instance.transform.SetParent(currCamera.transform);

        Camera n = z.Find(p => (!p.GetComponentInChildren<Player>() && p.name == "Main Camera"));

        Destroy(n.transform.root.gameObject);
        DontDestroyOnLoad(Player.Instance.transform.root.gameObject);

        Player.Instance.parentCam = currCamera.gameObject;  
        Player.Instance.CB = currCamera.gameObject.GetComponent<Cinemachine.CinemachineBrain>();
        Player.Instance.CallSceneOneFn(introPD, bossPD);
	}
	
	
}

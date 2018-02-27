using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCallerXD : MonoBehaviour {

    // Use this for initialization
    public GameObject playerPrefab;
    void Start()
    {
        if (!playerPrefab)
        {
            Debug.LogError("PlayerPrefab referencing is necessary");

            return;
        }
        if (!FindObjectOfType<Player>())
        {
            Debug.Log("False");
            GameObject x = Instantiate( playerPrefab);
            x.transform.SetParent(Camera.main.transform);
            DontDestroyOnLoad(Camera.main.gameObject);

            Player.Instance.parentCam = Camera.main.gameObject;
            //Player.Instance.transform.position = new Vector3(-90.3f, 2.973083f, -417.26f);
            //Player.Instance.transform.localEulerAngles = new Vector3(-18.945f, -2.145f, 0f);
            Player.Instance.RegisterSelf();
        }
        else
        {
            List<Camera> z = new List<Camera>(FindObjectsOfType<Camera>());
            Camera g = z.Find(p => (!p.GetComponentInChildren<Player>() && p.name == "Main Camera"));
            Player.Instance.transform.SetParent(g.transform);
            Camera n = z.Find(p => (!p.GetComponentInChildren<Player>() && p.name == "Main Camera"));

            Destroy(n.gameObject);

            DontDestroyOnLoad(g.gameObject);

            Player.Instance.parentCam = g.gameObject;
        }

        Player.Instance.CB = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        Player.Instance.blobs = blobsInOrder;
        Player.Instance.CallCircleBlobEvent();
        if (!FindObjectOfType<AudioManager>())
        {
            AudioManager.Instance.RegisterSelf();
        }

    }


    public List<GameObject> blobsInOrder = new List<GameObject>();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCallerXD : MonoBehaviour {

    // Use this for initialization
    public GameObject playerPrefab,
        grenadeGO, blockGO, sharkGO , boxGO;

    public Cinemachine.CinemachineVirtualCamera cam1, cam2, cam3;

    public List<GameObject> blobsInOrder = new List<GameObject>();

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
            GameObject x = Instantiate(playerPrefab);
            x.transform.SetParent(Camera.main.transform);
            DontDestroyOnLoad(Camera.main.transform.root.gameObject);

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

            DontDestroyOnLoad(g.transform.root.gameObject);

            Player.Instance.parentCam = g.gameObject;
        }

        #region CallWhenSceneZeroLoads

        if (!cam1) cam1 = GameObject.Find("CM_DollyCam_Gameplay_00").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        if (!cam2) cam2 = GameObject.Find("CM_AimGrenade_Gameplay_03").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        if (!cam3) cam3 = GameObject.Find("CM_AimBlock_Gameplay_04").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        if (!sharkGO) sharkGO = GameObject.Find("NewSharkPrefab");
        if (!grenadeGO) grenadeGO = GameObject.Find("Grenade");
        if (!blockGO) blockGO = GameObject.Find("Block");

        Player.Instance.CB = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        Player.Instance.blobs = blobsInOrder;
        Player.Instance.grenade = grenadeGO;
        Player.Instance.block = blockGO;
        Player.Instance.sharkEventGO = sharkGO;
        Player.Instance.boxSuitGO = boxGO;
        Player.Instance.cam01 = cam1;
        Player.Instance.cam02 = cam2;
        Player.Instance.cam03 = cam3;
        Player.Instance.CallCircleBlobEvent();


        #endregion

        if (!FindObjectOfType<AudioManager>()) AudioManager.Instance.RegisterSelf();


    }


}

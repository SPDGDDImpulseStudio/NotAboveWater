using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/EventPrebeta", fileName = "EventPrebeta", order = 8)]
public class Event_Prebeta : EventsInterface {
    public override void CurrEvent()
    {
        throw new NotImplementedException();
    }

    public override IEnumerator EnumEvent()
    {
        float maxDeltaDistTemp = 2.0f;
        Transform LookAtThis;
        Vector2 pos1 = new Vector2(3 * Screen.height, Screen.width ),// (int)UnityEngine.Random.Range(1, 7)),
                pos2 = new Vector2(3 * Screen.height, Screen.width ),// (int)UnityEngine.Random.Range(1, 7)),
                pos3 = new Vector2(3 * Screen.height, Screen.width ),// (int)UnityEngine.Random.Range(1, 7)),
                offSet1 = new Vector2(20, 40),
                offSet2 = new Vector2(-40, -20),
                offSet3 = new Vector2(40, -40);


        List<Vector2> vects = new List<Vector2>() {
         pos1,
         pos2,
         pos3,
        }, _offSets = new List<Vector2>()
        {
            offSet1,
            offSet2,
            offSet3
        };


        int currAI_WP = 0, currCam_WP = 0;
        LookAtThis = AI.Instance.waypoints[currAI_WP].transform;
        AI.Instance.maxDistDel = maxDeltaDistTemp;
        AI.Instance.target = LookAtThis;
        
        //AI.Instance.maxDistDel = maxDeltaDistTemp;
        while (true)
        {
            if(AI.Instance.target == null) {
                currAI_WP++;
                if (currAI_WP == 2) break;
                else if (currAI_WP == 1) LookAtThis = Player.Instance.transform;//AI.Instance.waypoints[currWaypoint].GetComponent<LookDirection>().LookAt;

                AI.Instance.target = AI.Instance.waypoints[currAI_WP].transform;

                CircleManager.Instance.SpawnButtons(3, _offSets, vects, AI.Instance.gameObject);

                }

            CameraManager.Instance.RotationCam(AI.Instance.transform, 20);
            AI.Instance.RotationCam(LookAtThis);
          
            yield return null;
        }
        Transform thisPos = GameManager.Instance.waypoints[currCam_WP].transform;
         CircleManager.Instance.ClearSet();
        CameraManager.Instance.target = thisPos;
        while (true)
        {
            if (thisPos.gameObject.activeInHierarchy)
            {
                Vector3 dir = (thisPos.position - CameraManager.Instance.transform.position).normalized;

                Quaternion rotation = Quaternion.LookRotation(dir);

                CameraManager.Instance.transform.localRotation = Quaternion.RotateTowards(CameraManager.Instance.transform.rotation, rotation,60* Time.deltaTime);
                //Debug.Log(angle);
                Debug.DrawRay(CameraManager.Instance.transform.position, thisPos.position - CameraManager.Instance.transform.position);
                //CameraManager.Instance.transform.position = Vector3.Lerp(CameraManager.Instance.transform.position, thisPos.position, speed * Time.deltaTime);
                //Debug.Log(Vector3.Distance(thisPos.position, CameraManager.Instance.transform.position));

            }
            else break;
            yield return null;
        }
        #region this can Be one function
        currCam_WP++;
        thisPos = GameManager.Instance.waypoints[currCam_WP].transform;
        CameraManager.Instance.target = thisPos;
        while (true)
        {
            if (thisPos.gameObject.activeInHierarchy)
            {
                Vector3 dir = (thisPos.position - CameraManager.Instance.transform.position).normalized;

                Quaternion rotation = Quaternion.LookRotation(dir);

                CameraManager.Instance.transform.localRotation = Quaternion.RotateTowards(CameraManager.Instance.transform.rotation, rotation, 60 * Time.deltaTime);
                //Debug.Log(angle);
                Debug.DrawRay(CameraManager.Instance.transform.position, thisPos.position - CameraManager.Instance.transform.position);
                //CameraManager.Instance.transform.position = Vector3.Lerp(CameraManager.Instance.transform.position, thisPos.position, speed * Time.deltaTime);
                //Debug.Log(Vector3.Distance(thisPos.position, CameraManager.Instance.transform.position));

            }
            else break;
            yield return null;
        }
        #endregion

        //Collider[] crates = Physics.OverlapSphere(Player.Instance.transform.position, 20);
        //crates_ = new List<GameObject>();
        GameObject ThatWall = GameObject.Find("Wall_Window2 (1)");
        offSet1 = new Vector2(40, 40);
        offSet2 = new Vector2(-40, -40);
        List<Vector2> offSett = new List<Vector2>() {

            offSet1,offSet2
        }
        
        
        ;
        CircleManager.Instance.SpawnButtons(1, offSett, offSett, ThatWall);
        //AI.Instance.target = AI.Instance.waypoints[currAI_WP].transform;
        
        yield return new WaitUntil(() => CircleManager.Instance.SetClear());

        Log("currCam_WP " + currCam_WP);

        CameraManager.Instance.maxDistDel = (int)SPEED.FAST;
        #region PlainMovementToNextWaypoint

        currCam_WP++;
        thisPos = GameManager.Instance.waypoints[currCam_WP].transform;
        CameraManager.Instance.target = thisPos;
        while (true)
        {
            if (thisPos.gameObject.activeInHierarchy)
            {
                Vector3 dir = (thisPos.position - CameraManager.Instance.transform.position).normalized;

                Quaternion rotation = Quaternion.LookRotation(dir);

                CameraManager.Instance.transform.localRotation = Quaternion.RotateTowards(CameraManager.Instance.transform.rotation, rotation, 60 * Time.deltaTime);
                //Debug.Log(angle);
                Debug.DrawRay(CameraManager.Instance.transform.position, thisPos.position - CameraManager.Instance.transform.position);
                //CameraManager.Instance.transform.position = Vector3.Lerp(CameraManager.Instance.transform.position, thisPos.position, speed * Time.deltaTime);
                //Debug.Log(Vector3.Distance(thisPos.position, CameraManager.Instance.transform.position));

            }
            else break;
            yield return null;
        }
        #endregion


        #region PlainMovementToNextWaypoint

        currCam_WP++;
        thisPos = GameManager.Instance.waypoints[currCam_WP].transform;
        CameraManager.Instance.target = thisPos;
        while (true)
        {
            if (thisPos.gameObject.activeInHierarchy)
            {
                Vector3 dir = (thisPos.position - CameraManager.Instance.transform.position).normalized;

                Quaternion rotation = Quaternion.LookRotation(dir);

                CameraManager.Instance.transform.localRotation = Quaternion.RotateTowards(CameraManager.Instance.transform.rotation, rotation, 60 * Time.deltaTime);
                //Debug.Log(angle);
                Debug.DrawRay(CameraManager.Instance.transform.position, thisPos.position - CameraManager.Instance.transform.position);
                //CameraManager.Instance.transform.position = Vector3.Lerp(CameraManager.Instance.transform.position, thisPos.position, speed * Time.deltaTime);
                //Debug.Log(Vector3.Distance(thisPos.position, CameraManager.Instance.transform.position));

            }
            else break;
            yield return null;
        }
        #endregion

        #region PlainMovementToNextWaypoint

        currCam_WP++;
        thisPos = GameManager.Instance.waypoints[currCam_WP].transform;
        CameraManager.Instance.target = thisPos;
        while (true)
        {
            if (thisPos.gameObject.activeInHierarchy)
            {
                Vector3 dir = (thisPos.position - CameraManager.Instance.transform.position).normalized;

                Quaternion rotation = Quaternion.LookRotation(dir);

                CameraManager.Instance.transform.localRotation = Quaternion.RotateTowards(CameraManager.Instance.transform.rotation, rotation, 60 * Time.deltaTime);
                //Debug.Log(angle);
                Debug.DrawRay(CameraManager.Instance.transform.position, thisPos.position - CameraManager.Instance.transform.position);
                //CameraManager.Instance.transform.position = Vector3.Lerp(CameraManager.Instance.transform.position, thisPos.position, speed * Time.deltaTime);
                //Debug.Log(Vector3.Distance(thisPos.position, CameraManager.Instance.transform.position));

            }
            else break;
            yield return null;
        }
        #endregion



        CameraManager.Instance.maxDistDel = (int)SPEED.MEDIUM;
        #region PlainMovementToNextWaypoint

        currCam_WP++;
        thisPos = GameManager.Instance.waypoints[currCam_WP].transform;
        CameraManager.Instance.target = thisPos;
        while (true)
        {
            if (thisPos.gameObject.activeInHierarchy)
            {
                Vector3 dir = (thisPos.position - CameraManager.Instance.transform.position).normalized;

                Quaternion rotation = Quaternion.LookRotation(dir);

                CameraManager.Instance.transform.localRotation = Quaternion.RotateTowards(CameraManager.Instance.transform.rotation, rotation, 60 * Time.deltaTime);
                //Debug.Log(angle);
                Debug.DrawRay(CameraManager.Instance.transform.position, thisPos.position - CameraManager.Instance.transform.position);
                //CameraManager.Instance.transform.position = Vector3.Lerp(CameraManager.Instance.transform.position, thisPos.position, speed * Time.deltaTime);
                //Debug.Log(Vector3.Distance(thisPos.position, CameraManager.Instance.transform.position));

            }
            else break;
            yield return null;
        }
        #endregion
        EndOfEvent();

    }

    enum SPEED
    {
        ZERO,
        FAST,
        MEDIUM,
        SLOW
    }
    void Log(object msg)
    {
        Debug.Log(msg);
    }

}

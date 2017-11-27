using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/EventE", fileName = "EventE_", order = 5)]
public class EventE : EventsInterface {
    public int waypointNumber;
    public float speedAIMoveForThisEvent = 12;
    public override void CurrEvent()
    {
        throw new NotImplementedException();
    }

    public override IEnumerator EnumEvent()
    {
        AI ai = AI.Instance;

        #region hide
                     /*  Player.Instance.allowToShoot = false;
         while (thisPos.gameObject.activeSelf)
        {
            CameraManager.Instance.transform.position = Vector3.LerpUnclamped(CameraManager.Instance.transform.position, thisPos.position, 4 * Time.deltaTime);

            yield return null;

        }
        Vector3 pos = ai.GetPos(0);
        //-Move AI to destinated place
        ai.nav.speed = 12;
        ai.nav.destination = pos;
        while (Vector3.Distance(ai.transform.position, ai.nav.destination) > 2)
        {
            yield return null;
        }
        float s = 0;
        ai.SliderTo(true);

        Player.Instance.allowToShoot = true;
        //Check if AI/Player dieded
        while (ai.currHealth > 0 && Player.Instance.currHealth > 0)
        {
            s += Time.deltaTime;
            if (s > 2)
            {
                AI.Instance.anim.Play("Bite", -1);
                s = 0;
                Player.Instance.currHealth -= AI.Instance.damage;
            }
            yield return null;
        }

        Player.Instance.allowToShoot = false;

        if (Player.Instance.currHealth <= 0)
        {//if player dies
            Debug.Log("Player Died");
            EndOfEvent();
            yield break;
        }
        else
        {//if AI dies
            ai.SliderTo(false);
            //ai.UpdateWaypoint();
            ai.nav.speed = 16;
        }

        pos = ai.GetPos(1);

        //Turn around
        while (ai.transform.rotation.y > 0 && ai.transform.rotation.y < 360)
        {
            ai.transform.Rotate(0, 10, 0, Space.Self);
            //Debug.Log(ai.transform.rotation.y);
            yield return null;
        }

        //To new Pos, exactly same shit but doesnt work
        ai.nav.destination = pos;
        while (Vector3.Distance(ai.transform.position, ai.nav.destination) > 2)
        {
            //ai.transform.position = Vector3.LerpUnclamped(ai.transform.position, pos, ai.speed * Time.deltaTime);
            //Debug.Log(ai.transform.rotation);
            yield return null;
        }

        thisPos = GameManager.Instance.waypoints[1].transform;

        //This condition should be checking if its y rotation is not around 180-ish
        //If it is then just get out
        //Debug.Log(ai.transform.rotation.y);

        while (ai.transform.rotation.y < 0.95)
        {
            ai.transform.Rotate(0, -10, 0, Space.Self);
            //Debug.Log(ai.transform.rotation.y);

            yield return null;
        }

        while (thisPos.gameObject.activeSelf)
        {
            CameraManager.Instance.transform.position = Vector3.LerpUnclamped(CameraManager.Instance.transform.position, thisPos.position, 3 * Time.deltaTime);

            yield return null;

        }
        Player.Instance.allowToShoot = true;

        ai.currHealth = ai.maxHealth;
        ai.SliderTo(true);
        while (ai.currHealth > 0 && Player.Instance.currHealth > 0)
        {
            s += Time.deltaTime;
            if (s > 2)
            {

                AI.Instance.anim.Play("Bite", -1);
                s = 0;
                Player.Instance.currHealth -= AI.Instance.damage;
            }
            yield return null;
        }

        Player.Instance.allowToShoot = false;

        while (ai.transform.rotation.y > 0 && ai.transform.rotation.y < 360)
        {
            ai.transform.Rotate(0, 10, 0, Space.Self);
            //Debug.Log(ai.transform.rotation.y);
            yield return null;
        }

        Debug.Log("AIWAYPOINTS.COUNT: " + GameManager.Instance.AIWaypoints.Count);
        //-Move AI to destinated place
        ai.nav.speed = 12;
        pos = ai.GetPos(ai.waypoints.Count - 1);
        Debug.Log(pos + " " + ai.GetPos(1) + " | " + ai.nav.destination);
        ai.nav.destination = pos;
        //Debug.Log()
        Debug.Log(Vector3.Distance(ai.transform.position, ai.nav.destination) + " | " + ai.transform.position + " | " + ai.nav.destination + " | " + pos);
        while (Vector3.Distance(ai.transform.position, ai.nav.destination) > 2)
        {
            //ai.transform.position = Vector3.LerpUnclamped(ai.transform.position, pos, ai.speed * Time.deltaTime);
            Debug.Log(ai.transform.rotation);
            yield return null;
        }
        thisPos = GameManager.Instance.waypoints[2].transform;
        while (ai.transform.rotation.y < 0.95)
        {
            ai.transform.Rotate(0, -10, 0, Space.Self);
            //Debug.Log(ai.transform.rotation.y);

            yield return null;
        }
        while (thisPos.gameObject.activeSelf)
        {
            CameraManager.Instance.transform.position = Vector3.LerpUnclamped(CameraManager.Instance.transform.position, thisPos.position, 3 * Time.deltaTime);

            yield return null;
        }

        Player.Instance.allowToShoot = true;

        ai.currHealth = ai.maxHealth;
        ai.SliderTo(true);
        while (ai.currHealth > 0 && Player.Instance.currHealth > 0)
        {
            s += Time.deltaTime;
            if (s > 2)
            {
                AI.Instance.anim.Play("Bite", -1);
                s = 0;
                Player.Instance.currHealth -= AI.Instance.damage;
            }
            yield return null;

            ai.SliderTo(false);
        }
           */
        #endregion
     
        ai.nav.speed = speedAIMoveForThisEvent;
        ai.nav.destination = GameManager.Instance.AIWaypoints[waypointNumber].transform.position;
     
        while (Vector3.Distance(ai.transform.position, ai.nav.destination) > 2)
        {
            if (debugThis) Debug.Log("Distance between AI and Dest is now :" + Vector3.Distance(ai.transform.position, ai.nav.destination));
            yield return null;
        }
        EndOfEvent();
    }

  
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/EventD", fileName = "EventD_", order = 4)]
public class EventD : EventsInterface {
    public override void CurrEvent()
    {
        throw new NotImplementedException();
    }

    

    public override IEnumerator EnumEvent()
    {
        Player.Instance.allowToShoot = true;
        AI ai = AI.Instance;
        //float s = ai.anim.GetFloat("biteTimer");
        Vector3 pos = ai.GetPos(0);
        //-Move AI to destinated place
        ai.nav.speed = 12;
        ai.nav.destination = pos;
        while (Vector3.Distance(ai.transform.position, ai.nav.destination) > 2)
        {
            //ai.transform.position = Vector3.LerpUnclamped(ai.transform.position, pos, ai.speed * Time.deltaTime);
            //Debug.Log(ai.transform.rotation);
            yield return null;
        }
        float s=0;
        ai.SliderTo(true);
        
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

        if(Player.Instance.currHealth <= 0)
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
        Debug.Log(pos);
        //Turn around
        while (ai.transform.rotation.y > 0 && ai.transform.rotation.y < 360)
        {
            ai.transform.Rotate(0, 10, 0, Space.Self);
            //Debug.Log(ai.transform.rotation.y);
            yield return null;
        }
        Debug.Log("SA");

        //To new Pos, exactly same shit but doesnt work
        ai.nav.destination = pos;
        while (Vector3.Distance(ai.transform.position, ai.nav.destination) > 2)
        {
            //ai.transform.position = Vector3.LerpUnclamped(ai.transform.position, pos, ai.speed * Time.deltaTime);
            //Debug.Log(ai.transform.rotation);
            yield return null;
        }
        Debug.Log("SA");

        Transform thisPos = GameObject.Find("Waypoints (1)").transform;
        Debug.Log(thisPos);
        //This condition should be checking if its y rotation is not around 180-ish
        //If it is then just get out
        Debug.Log(ai.transform.rotation.y);

        while (ai.transform.rotation.y < 0.95)
        {
            ai.transform.Rotate(0, -10, 0, Space.Self);
            //Debug.Log(ai.transform.rotation.y);

            yield return null;
        }

        while (Vector3.Distance(CameraManager.Instance.transform.position, thisPos.position) > 0.5)
        {
            CameraManager.Instance.transform.position = Vector3.LerpUnclamped(CameraManager.Instance.transform.position, thisPos.position, ai.speed/2 * Time.deltaTime);

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

        if (ai.currHealth <= 0)
        {
            Vector3 x = new Vector3(ai.transform.position.x - UnityEngine.Random.Range(-10, 10)
                , ai.transform.position.y - UnityEngine.Random.Range(-10, 10)
                , ai.transform.position.z - UnityEngine.Random.Range(-10, 10));
            float timer = 0;
            //Quaternion b = new Quaternion (ai.transform.rotation.x - UnityEngine.Random.Range(-10, 10)
            //    , ai.transform.rotation.y + UnityEngine.Random.Range(-10, 10)
            //    , ai.transform.rotation.z - UnityEngine.Random.Range(-10, 10)
            //    , ai.transform.rotation.w + UnityEngine.Random.Range(-10, 10));
            while (timer < 5)
            {
                ai.transform.Rotate(x, 10);
                //ai.transform.rotation = Quaternion.Lerp(ai.transform.rotation, b, 2*Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        /*
         
         
        AI swims to Camera
	    Starts Attacking AI + AI starts attackign camera
		Check for health then
	    Swims away 
	    Wait till swam away then camera move
	    repeat
	
         
         */
        EndOfEvent();
    }

  
}

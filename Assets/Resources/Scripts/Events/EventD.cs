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
        AI ai = FindObjectOfType<AI>();
        while(ai.currHealth > 0 && Player.Instance.currHealth > 0)
        {
            yield return null;
        }
        Player.Instance.allowToShoot = false;
        EndOfEvent();
    }

  
}

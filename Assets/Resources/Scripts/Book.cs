using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSlot {

    public GameObject GO;

    public Vector3 bookSlotPos;

    public int uniqueID;

    public bool gotBook = false;

    public BookSlot(GameObject _GO, Vector3 _bookSlot,  int ID)
    {
        bookSlotPos = _bookSlot;
        GO = _GO;
        uniqueID = ID;
    }
}

public class BookInfo :MonoBehaviour
{

}

public class Book :MonoBehaviour{

    public BookSlot bookSlotInfo;
    public BookShelf parent;

    public BookSlot ReturnSlot(Vector3 pos)
    {
        float worldPosX = (pos.x + parent.sizeOfShelf.x / 2) / parent.sizeOfShelf.x;
        float worldPosY = (pos.x + parent.sizeOfShelf.y / 2) / parent.sizeOfShelf.y;

        //worldPosX = Mathf.Clamp01(worldPosX);
        //worldPosY = Mathf.Clamp01(worldPosY);

        int x = Mathf.RoundToInt((parent.bookX) * worldPosX);
        int y = Mathf.RoundToInt((parent.bookY) * worldPosY);

        return parent.bookShelf[x, y];
    }
}



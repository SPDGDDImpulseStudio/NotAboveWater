using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelf : MonoBehaviour {


    public int bookX = 5, bookY = 3;
    public BookSlot[,] bookShelf ;

    public GameObject bookPrefab;

    public Vector2 bookShelfPos;

    public Vector2 sizeOfShelf;

    public Vector2 gapBetweenBook;

    public int _bookY = 4, _bookX = 2 ;
    public Camera cam;


    void Start()
    {

        cam = Camera.main;

        /*
        There's a cube's vector3,
        first return the x axis of the cube, divide by the number of books we putting in for  x
        starts from the bottom left, we have the x 
        does the same for y
         


         
         */
        sizeOfShelf = new Vector2(this.gameObject.transform.localScale.x, gameObject.transform.localScale.y);
        GenerateBook();
    }

    void Update()
    {
  
    }

    public BookSlot ReturnSlot(Vector3 pos)
    {
        float worldPosX = (pos.x + sizeOfShelf.x / 2) / sizeOfShelf.x;
        float worldPosY = (pos.x + sizeOfShelf.y / 2) / sizeOfShelf.y;

        worldPosX = Mathf.Clamp01(worldPosX);
        worldPosY = Mathf.Clamp01(worldPosY);

        int x = Mathf.RoundToInt((bookX) * worldPosX);
        int y = Mathf.RoundToInt((bookY) * worldPosY);

        return bookShelf[x, y];
    }
    void GenerateBook()
    {
        bookShelf = new BookSlot[bookX, bookY];
        //Vector3 leftMostBottom = startOfBook.transform.position - transform.right * i - transform.up * j;

        Vector3 worldBottomLeft = transform.position - (Vector3.right * sizeOfShelf.x / 2) - (Vector3.up * sizeOfShelf.y / 2);
        gapBetweenBook= new Vector2((sizeOfShelf.x / bookX), (sizeOfShelf.y / bookY));
        int num = 0;
        for (int i = 0; i < bookX; i++)
        {
            for (int j = 0; j < bookY; j++)
            {
                
                Vector3 newPos = worldBottomLeft + Vector3.right *(i * gapBetweenBook.x) + Vector3.up * (j * gapBetweenBook.y);

                bookShelf[i, j] = new BookSlot(bookPrefab, newPos, num);
                
                num++;
                GameObject slot = Instantiate(bookShelf[i, j].GO, bookShelf[i, j].bookSlotPos, Quaternion.identity);
                slot.AddComponent<Book>().bookSlotInfo = bookShelf[i, j];
                slot.GetComponent<Book>().parent = this;                
                slot.name = "Slot" + i + "_" + j;
                slot.transform.SetParent(this.transform);
            }
        }
        
    }

    public BookSlot GetSpecifyBook(int x)
    {
        for (int i = 0; i < bookX; i++)
        {
           for(int j = 0; j < bookY; j++)
            {
                if (x == bookShelf[i, j].uniqueID)
                    return bookShelf[i, j];
            }
        }
        return null;
    }
    


    void OnDrawGizmos()
    {

        
        
        //Vector3 worldBottomLeft = transform.position - (Vector3.right * sizeOfShelf.x / 2) - (Vector3.up * sizeOfShelf.y / 2);

        //Vector3 worldTopRight = transform.position + (Vector3.right * sizeOfShelf.x / 2) + (Vector3.up * sizeOfShelf.y / 2);
        Gizmos.color = Color.black;
        //Gizmos.DrawLine(worldBottomLeft, worldTopRight);
        Gizmos.DrawWireCube(transform.position,new Vector3(sizeOfShelf.x, sizeOfShelf.y,0 ));
        Gizmos.color = Color.blue;

        //if (bookShelf != null)
        //{
        //    foreach (BookSlot x in bookShelf)
        //    {
        //        Gizmos.DrawCube(x.bookSlotPos, Vector3.one *(gapBetweenBook.x -.1f));
        //    }
        //}
    }

}

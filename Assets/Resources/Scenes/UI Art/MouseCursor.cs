using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour
{

    public Texture2D defaultAimTexture;
    public CursorMode curMode = CursorMode.Auto;
    public Vector2 hotspot = Vector2.zero;

    void Start()
    {
        
        Cursor.SetCursor(defaultAimTexture, hotspot, curMode);
    }

}
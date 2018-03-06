using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour
{

    public Texture2D defaultAimTexture;
    public Texture2D targetTexture;
    public CursorMode curMode = CursorMode.Auto;
    public Vector2 hotspot = Vector2.zero;

    void Start()
    {
        Cursor.SetCursor(defaultAimTexture, hotspot, curMode);
    }

    public void CursorOnTarget(bool x)
    {
        if (x)
            Cursor.SetCursor(targetTexture, hotspot, curMode);
        else
            Cursor.SetCursor(defaultAimTexture, hotspot, curMode);
    }

    public void Update()
    {
        if(Input.GetMouseButton(0))
            Cursor.SetCursor(targetTexture, hotspot, curMode);
        else
        Cursor.SetCursor(defaultAimTexture, hotspot, curMode);

    }

}
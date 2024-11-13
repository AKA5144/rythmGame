using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CursorScript : MonoBehaviour
{
    [SerializeField] Texture2D cursorSkin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector2 hotspot = new Vector2(cursorSkin.width / 2, cursorSkin.height / 2);
        Cursor.SetCursor(cursorSkin, hotspot, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {


    }
}

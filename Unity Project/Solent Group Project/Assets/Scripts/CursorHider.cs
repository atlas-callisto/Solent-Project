using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHider : MonoBehaviour
{
    // Use this to disable the cursor in the scene this is attached to
    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSettings : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Optional: prevents the cursor from locking to the center
    }
}

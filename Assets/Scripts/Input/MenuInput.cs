using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    public bool WasBackPressed { get; private set; } = false;

    private void OnDisable()
    {
        WasBackPressed = false;
    }

    private void Update()
    {
        WasBackPressed = Input.GetKeyDown(KeyCode.Escape);
    }
}

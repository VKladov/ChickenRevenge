using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public bool WasShootPressed { get; private set; } = false;
    public bool WasShootReleased { get; private set; } = false;
    public bool IsJumpPressed { get; private set; } = false;
    public Vector2 Movement { get; private set; } = Vector2.zero;
    public bool WasPausePressed { get; private set; } = false;

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        WasShootPressed = false;
        WasShootReleased = true;
        Movement = Vector2.zero;
        IsJumpPressed = false;
        WasPausePressed = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        WasShootPressed = Input.GetMouseButtonDown(0);
        WasShootReleased = Input.GetMouseButtonUp(0);
        Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        IsJumpPressed = Input.GetKey(KeyCode.Space);
        WasPausePressed = Input.GetKeyDown(KeyCode.Escape);
    }
}

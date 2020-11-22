using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
    [SerializeField] float _turnSpeed = 15;
    [SerializeField] Camera _camera;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float yCamaraRotation = _camera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yCamaraRotation, 0), _turnSpeed * Time.fixedDeltaTime);
    }
}

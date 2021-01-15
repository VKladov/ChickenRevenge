using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
    [SerializeField] float _turnSpeed = 15;
    [SerializeField] Camera _camera;
    [SerializeField] Gun _gun;
    [SerializeField] LayerMask _shootableMask;

    private Quaternion Rotation => Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0);
    private Quaternion GunRotation
    {
        get
        {
            Vector3 shotPoint = _gun.transform.position;
            Vector3 direction = _camera.transform.forward;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _shootableMask))
                direction = (hit.point - shotPoint).normalized;

            return Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, _turnSpeed * Time.deltaTime);
        //_gun.transform.rotation = Quaternion.Slerp(transform.rotation, GunRotation, _turnSpeed * Time.deltaTime);
    }
}

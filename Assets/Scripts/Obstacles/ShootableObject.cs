using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

[RequireComponent(typeof(MeshFilter))]
public class ShootableObject : MonoBehaviour
{
    private GameObject _substructObject;
    private MeshFilter _meshFilter;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();

        _substructObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        _substructObject.transform.localScale = new Vector3(0.1f, 10, 0.1f);
        _substructObject.GetComponent<CapsuleCollider>().enabled = false;
    }

    public void TakeShoot(Vector3 shotPoint, Vector3 shotDirection)
    {
        _substructObject.transform.position = shotPoint;
        _substructObject.transform.rotation = Quaternion.LookRotation(shotDirection, Vector3.up) * Quaternion.Euler(new Vector3(90, 0, 0));

        _meshFilter.sharedMesh = Boolean.Subtract(gameObject, _substructObject).mesh;
    }
}

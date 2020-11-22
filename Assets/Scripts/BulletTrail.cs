using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class BulletTrail : MonoBehaviour
{
    public void Init(Vector3 target)
    {
        StartCoroutine(Move(target));
    }

    private IEnumerator Move(Vector3 target)
    {
        yield return null;
        transform.position = target;
    }
}

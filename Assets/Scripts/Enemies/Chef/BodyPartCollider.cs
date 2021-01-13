using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BodyPartCollider : DamageReceiver
{
    public UnityAction<Vector3, Vector3> GotShot;
    public bool Reflective = false;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StonePiece : MonoBehaviour
{
    public UnityAction<StonePiece> Destroyed;

    private void OnDestroy() => Destroyed?.Invoke(this);
}

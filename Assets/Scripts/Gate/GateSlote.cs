using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSlote : MonoBehaviour
{
    public bool IsEmpty { get; private set; } = true;
    private Character _character;

    public void MakeBusy(Character character)
    {
        _character = character;
        IsEmpty = false;
        character.Destroyed += OnOwnerDestroyed;
    }

    public void MakeEmpty()
    {
        IsEmpty = true;
        _character.Destroyed -= OnOwnerDestroyed;
    }

    private void OnOwnerDestroyed(DamageReceiver receiver)
    {
        IsEmpty = true;
    }
}

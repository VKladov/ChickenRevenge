using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PapuanMeetCollider : MonoBehaviour
{
    public UnityAction<Papuan> MetPapuan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Papuan papuan))
            MetPapuan?.Invoke(papuan);
    }
}

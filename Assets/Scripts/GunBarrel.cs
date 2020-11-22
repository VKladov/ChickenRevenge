using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GunBarrel : MonoBehaviour
{
    private AudioSource _audio;

    private void Awake() => _audio = GetComponent<AudioSource>();

    public void PlaySound() => _audio.Play();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]private AudioClip turretRotation;
    [SerializeField]private AudioClip engineStart;
    [SerializeField]private AudioClip engineMove;
    [SerializeField]private AudioClip engineEnd;
    [SerializeField] private AudioClip shoot;

    public AudioClip TurretRotation => turretRotation;

    public AudioClip EngineStart => engineStart;

    public AudioClip EngineMove => engineMove;

    public AudioClip EngineEnd => engineEnd;

    public AudioClip Shoot => shoot
    ;
    public void PlayClip(AudioSource source, AudioClip clip)
    {
        if (source.isPlaying)
        {
            source.Stop();
        }

        source.clip = clip;
        source.Play();
    }
}

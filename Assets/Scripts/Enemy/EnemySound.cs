using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemy;
using Unity.VisualScripting;
using UnityEngine;
using static Assets.Scripts.Enemy.EnemyChasingController;

public class EnemySound : MonoBehaviour
{

    private float TimeSinceLastScream = 0f;
    private float RandomScreamTime = 0f;
    public AudioSource enemyAudio;
    private Step Chasing;
    public AudioClip[] enemyScreamAudioClips;
    public AudioClip[] enemyRandomAudioClips;
    public AudioClip[] enemyLookingAroundAudioClips;
    private bool alreadyTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        Chasing = this.GetComponentInParent<EnemyChasingController>()._step;
        RandomScreamTime = UnityEngine.Random.Range(12f, 50f);
    }

    // Update is called once per frame
    void Update()
    {   
        Chasing = this.GetComponentInParent<EnemyChasingController>()._step;
        if (Chasing == Step.Screaming && enemyScreamAudioClips.Length > 0)
        {
            Debug.Log("Scream");
            enemyAudio.PlayOneShot(enemyScreamAudioClips[0]);
        }
        else if (Chasing == Step.Disabled &&  TimeSinceLastScream > RandomScreamTime && enemyRandomAudioClips.Length > 0 && !enemyAudio.isPlaying)
        {
            if (alreadyTrigger)
            {
                Debug.Log("Already");
                alreadyTrigger = false;
            }
            Debug.Log("Random");
            RandomScreamTime = UnityEngine.Random.Range(50f, 75f);
            enemyAudio.PlayOneShot(enemyRandomAudioClips[UnityEngine.Random.Range(0, enemyRandomAudioClips.Length)]);
        }
        else if (Chasing == Step.LookingAround && !alreadyTrigger && enemyLookingAroundAudioClips.Length > 0)
        {
            Debug.Log("Looking");
            enemyAudio.PlayOneShot(enemyLookingAroundAudioClips[UnityEngine.Random.Range(0, enemyLookingAroundAudioClips.Length)]);
            alreadyTrigger = true;
        }

        TimeSinceLastScream += Time.deltaTime;
    }
}

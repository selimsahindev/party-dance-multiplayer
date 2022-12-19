using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private KeySequencer keySequencer;

    private void Start()
    {
        //float actualKeySpeed = keySequencer.KeyMoveSpeed *  Screen.width / 1000f;
        //float distanceBetweenKeySpawnPositionAndSuccessArea = keySequencer.GetComponent<RectTransform>().sizeDelta.x / 2f;
        //float delay = distanceBetweenKeySpawnPositionAndSuccessArea / actualKeySpeed;
        //Debug.Log(delay);
        audioSource.PlayDelayed(delay);
    }
}

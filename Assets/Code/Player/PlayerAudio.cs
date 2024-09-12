using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource source;

    [SerializeField]
    private AudioClip[] _footstepSounds;
    [SerializeField]
    private float _secondsBetweenPlay;

    private int _currentIndex = 0;
    private float _currentPlayTimer = 0;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        float h = Core.InputManager.GetAxis(InputType.MoveHorizontal);
        float v = Core.InputManager.GetAxis(InputType.MoveVertical);
        bool isMoving = h != 0 || v != 0;

        // If not moving, reset the timer
        if (!isMoving)
        {
            _currentPlayTimer = _secondsBetweenPlay / 2;
            return;
        }

        // Increase timer
        _currentPlayTimer += Time.deltaTime;

        // If reached the end of the timer, change index and play sfx
        if (_currentPlayTimer >= _secondsBetweenPlay)
        {
            source.clip = _footstepSounds[_currentIndex];
            source.Play();

            _currentPlayTimer = 0;
            if (++_currentIndex >= _footstepSounds.Length)
                _currentIndex = 0;
        }
    }
}

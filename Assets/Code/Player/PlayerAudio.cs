using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource source;
    private readonly Collider2D[] _floorColliders = new Collider2D[1];

    [SerializeField]
    private AudioClip[] _footstepSounds;
    [SerializeField]
    private float _secondsBetweenPlay;
    [SerializeField]
    private LayerMask _floorLayer;

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

        // Check the current floor type
        if (Physics2D.OverlapCircleNonAlloc(transform.position, 0.2f, _floorColliders, _floorLayer) > 0)
        {
            Debug.Log("Walking on " + _floorColliders[0].gameObject.name);
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

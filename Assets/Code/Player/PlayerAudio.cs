using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource source;
    private readonly Collider2D[] _floorColliders = new Collider2D[1];

    [SerializeField]
    private float _secondsBetweenPlay;
    [SerializeField]
    private LayerMask _floorLayer;

    private FloorInfo _currentFloor;
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

        // If not on a floor, reset everything
        if (Physics2D.OverlapCircleNonAlloc(transform.position, 0.2f, _floorColliders, _floorLayer) == 0)
        {
            _currentFloor = null;
            _currentIndex = 0;
            _currentPlayTimer = 0;
            return;
        }

        // If on a different floor, update info
        if (_floorColliders[0].gameObject != _currentFloor?.gameObject)
        {
            Debug.Log($"Changing floor to {_floorColliders[0].gameObject.name}");
            _currentFloor = _floorColliders[0].GetComponent<FloorInfo>();
            _currentIndex = 0;
            _currentPlayTimer = 0;
            return;
        }

        // Increase timer
        _currentPlayTimer += Time.deltaTime;

        // If reached the end of the timer, change index and play sfx
        if (_currentPlayTimer >= _secondsBetweenPlay)
        {
            source.clip = _currentFloor.FootstepSounds[_currentIndex];
            source.Play();

            _currentPlayTimer = 0;
            if (++_currentIndex >= _currentFloor.FootstepSounds.Length)
                _currentIndex = 0;
        }
    }
}

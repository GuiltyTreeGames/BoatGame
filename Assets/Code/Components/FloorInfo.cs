using UnityEngine;

public class FloorInfo : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _footstepSounds;
    [SerializeField]
    [Range(0f, 1f)]
    private float _speedPercent = 1;

    public AudioClip[] FootstepSounds => _footstepSounds;
    public float speedPercent => _speedPercent;
}

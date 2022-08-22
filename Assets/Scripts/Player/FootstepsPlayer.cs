using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class FootstepsPlayer : MonoBehaviour
{
    public AudioSource _footstepsAudioSource;

    [SerializeField] private AudioClip _footstepsAudioClipWalk;

    private StarterAssetsInputs _inputs;
    private bool isOnCooldown;

    private float _footstepCooldown = 17f;


    private void Start()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
    }
    private void Update()
    {
        HandleFootstepSound();
    }


    private void HandleFootstepSound()
    {
        
        _footstepCooldown += Time.deltaTime;
        if (_footstepCooldown >= 17f)
        {
            _footstepCooldown = 17f;
            isOnCooldown = false;
        }
        if (_footstepCooldown == 17f)
        {
            if (_inputs.move != Vector2.zero && !isOnCooldown)
            {
                _footstepCooldown = 0;
                isOnCooldown = true;
            }
            
            if (_footstepCooldown == 0 && _inputs.move != Vector2.zero)
            {
                if (!_inputs.sprint)
                    _footstepsAudioSource.PlayOneShot(_footstepsAudioClipWalk);
            }

        }
            if (_inputs.move == Vector2.zero)
            {
                _footstepsAudioSource.Stop();
                isOnCooldown = false;
            _footstepCooldown = 17f;
            }

    }


}

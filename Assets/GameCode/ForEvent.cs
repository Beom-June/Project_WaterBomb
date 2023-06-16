using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForEvent : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particlePrefab;
    [SerializeField] private float _destroyTime = 3.0f;
    private void Start()
    {
        Destroy(gameObject, _destroyTime);
    }

    // private void OnDestroy()
    // {
    //     if (_spawnedParticle != null)
    //     {
    //         if (_spawnedParticle.transform.childCount >= 3)
    //         {
    //             Transform thirdChild = _spawnedParticle.transform.GetChild(2);
    //             if (thirdChild != null)
    //             {
    //                 thirdChild.gameObject.SetActive(true);
    //             }
    //         }
    //     }
    // }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            _particlePrefab.Play();
            Debug.Log("dsf");
        }
    }
}

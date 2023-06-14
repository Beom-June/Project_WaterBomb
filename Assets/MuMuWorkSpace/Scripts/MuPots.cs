using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuPots : MonoBehaviour
{
    [SerializeField] GameObject _plantGob = null;
    [SerializeField] GameObject _targetWtMnGob = null;

    private float _hitCount = 0;
    [SerializeField] GameObject _hitParticleGob = null;


    public void Hit()
    {
        _hitCount++;
        if (_hitCount == 1)
        {
            Instantiate(_hitParticleGob, transform.position, Quaternion.identity);
            _plantGob.SetActive(true);
        }
        else if(_hitCount == 2)
        {
            Instantiate(_hitParticleGob, transform.position, Quaternion.identity);
            _plantGob.SetActive(false);
            _targetWtMnGob.transform.localPosition = Vector3.zero;
        }
    }





}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MuTargetMain : MonoBehaviour
{
    [Header("Main Objs")]
    [SerializeField] protected GameObject _hitParticleGob = null;
    [SerializeField] protected Material _hitMat = null;
    
    public abstract void Hit(Vector3 pos);
}

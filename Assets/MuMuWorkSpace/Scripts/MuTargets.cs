using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuTargets : MuTargetMain
{
    [Space(6)]
    [Header("Normal Targets")]
    [SerializeField] private GameObject _crossImg = null;
    private MeshRenderer _ms = null;
    private void Awake() 
    {
        _ms = GetComponentInChildren<MeshRenderer>();
    }
    public override void Hit(Vector3 pos) // 피격
    {
        _ms.material = _hitMat;
        _crossImg.SetActive(true);
        Instantiate(_hitParticleGob, pos, transform.rotation);
        MuGameManager.Targets.Remove(this.gameObject);
        gameObject.layer = 1;        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuTargets : MonoBehaviour
{
    [SerializeField] private Material _orgMat = null;
    [SerializeField] private GameObject _crossImg = null;
    [SerializeField] private GameObject _hitParticleGob = null;
    private MeshRenderer _ms = null;
    private void Awake() 
    {
        _ms = GetComponentInChildren<MeshRenderer>();
    }
    public void Hit(Vector3 pos)
    {
        // 사망 효과 추가
        _ms.material = _orgMat;
        Instantiate(_hitParticleGob, pos, transform.rotation);
        _crossImg.SetActive(true);
        MuGameManager.Targets.Remove(this.gameObject);
        gameObject.layer = 1;        
    }

}

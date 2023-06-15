using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuTargets : MuTargetMain
{
    [Space(6)]
    [Header("Normal Targets")]
    [SerializeField] private GameObject _crossImg = null;
    private MeshRenderer _ms = null;
    [Header("OutLine")]
    [SerializeField] private float _outLineStartTime = 10;
    [SerializeField] private float _outLineColorChangeTime = 1f;
    private Outline _outLine = null;
    private void Awake() 
    {
        _ms = GetComponentInChildren<MeshRenderer>();
        _outLine = GetComponent<Outline>();
    }
    private void Update() 
    {
        _outLineStartTime -= Time.deltaTime;
        if (_outLineStartTime <= 0 && !_outLine.enabled)
        {
            _outLine.enabled = true;
            StartCoroutine(OutLineColorChanger());
        }    
    }
    public override void Hit(Vector3 pos) // 피격
    {
        _outLine.OutlineWidth = 0;
        _ms.material = _hitMat;
        _crossImg.SetActive(true);
        Instantiate(_hitParticleGob, pos, transform.rotation);
        MuGameManager.Targets.Remove(this.gameObject);
        gameObject.layer = 1;        
    }


    private IEnumerator OutLineColorChanger()
    {
        yield return new WaitForSeconds(_outLineColorChangeTime);
        _outLine.OutlineColor = Color.white;
        yield return new WaitForSeconds(_outLineColorChangeTime);
        _outLine.OutlineColor = Color.black;
        yield return StartCoroutine(OutLineColorChanger());

    }


















}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum BonusTargetType
{
    Jumper, Runner
}
public class MuBonusTargets : MuTargetMain
{
    private BoxCollider _col = null;
    private Animator _ani = null;
    [Space(6)]
    [Header("Bonus Targets")]
    [Header("Hit")]
    [SerializeField] private float _pushPower = 100f;
    [SerializeField] private Rigidbody _rb = null;
    [SerializeField] private SkinnedMeshRenderer[] _ms = new SkinnedMeshRenderer[2];

    [Header("TargetType")]
    [SerializeField] private BonusTargetType _targetType = BonusTargetType.Jumper;
    [Header("Runner")]
    [SerializeField] private Rigidbody _runnerRb = null;
    [SerializeField] private Transform[] _runPointsTrs = null;
    private Vector3 _curRunPoint = Vector3.zero;
    private int _runCount = 0;
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _runWaitTime = 1.0f;
    
    [Header("Gold")]
    [SerializeField] private float _hitGold = 30;
    private UnityEngine.UI.Text _goldTxt = null;
    private void Awake() 
    {
        _col = GetComponent<BoxCollider>();
        _ani = GetComponentInParent<Animator>();    
    }
    private void Start() 
    {
        if (_targetType == BonusTargetType.Runner)
            StartCoroutine(RunnerRun());    
        
        if (GameObject.FindGameObjectWithTag("GoldText") != null)
        {
            _goldTxt = GameObject.FindGameObjectWithTag("GoldText").GetComponent<UnityEngine.UI.Text>();
        }
    }
    public override void Hit(Vector3 pos)
    {
        if (_targetType == BonusTargetType.Runner)
        {
            _runnerRb.isKinematic = true;
            _rb.isKinematic = false;
            StopCoroutine(RunnerRun());
        }

        Instantiate(_hitParticleGob, pos, transform.rotation);
        MuGameManager.Targets.Remove(this.gameObject);
        gameObject.layer = 1;        
        Vector3 dir = transform.position - pos;
        _col.enabled = false;
        _ani.enabled = false;
        for (int i=0; i<_ms.Length; i++)
        {
            _ms[i].material = _hitMat;
        }
        _rb.AddForce(dir * _pushPower, ForceMode.Impulse);
        if (_goldTxt != null)
            _goldTxt.text = (Convert.ToInt32(_goldTxt.text) + _hitGold).ToString();
    }


    private IEnumerator RunnerRun()
    {
        _curRunPoint = _runPointsTrs[_runCount].position;
        _runCount = _runCount + 1 >= _runPointsTrs.Length ? 0 : _runCount + 1;
        Vector3 dir = (_curRunPoint - transform.position).normalized;
        _runnerRb.transform.LookAt(_curRunPoint);
        while (Vector3.Distance(transform.position, _curRunPoint) > 2)
        {
            _runnerRb.velocity = (dir * _runSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        _runnerRb.velocity = Vector3.zero;
        yield return new WaitForSeconds(_runWaitTime);
        yield return StartCoroutine(RunnerRun());
        yield break;
    }


}

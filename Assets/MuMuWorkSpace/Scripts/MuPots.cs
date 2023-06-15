using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MuPots : MonoBehaviour
{
    [SerializeField] private GameObject _plantGob = null;
    [SerializeField] private GameObject _targetWtMnGob = null;
    private float _hitCount = 0;
    [SerializeField] private GameObject _hitParticleGob = null;
    [Header("Imoticon")]
    [SerializeField] private GameObject[] _disGobs = new GameObject[2];
    [SerializeField] private GameObject _enaGobs = null;

    [Header("OutLine")]
    [SerializeField] private float _outLineStartTime = 10;
    [SerializeField] private float _outLineColorChangeTime = 1f;
    private Outline _outLine = null;
    private void Awake() 
    {
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
            _outLine.OutlineWidth = 0;
            Instantiate(_hitParticleGob, transform.position, Quaternion.identity);
            _plantGob.SetActive(false);
            _targetWtMnGob.transform.localPosition = Vector3.zero;
            _disGobs[0].SetActive(false);
            _disGobs[1].SetActive(false);
            _enaGobs.SetActive(true);

        }
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

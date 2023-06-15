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
    [SerializeField] private Image _imoImg = null;

    [SerializeField] private Sprite _smileSpr = null;


    private void Awake() 
    {
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
            Instantiate(_hitParticleGob, transform.position, Quaternion.identity);
            _plantGob.SetActive(false);
            _targetWtMnGob.transform.localPosition = Vector3.zero;
            _imoImg.sprite = _smileSpr;
            _imoImg.GetComponent<RectTransform>().sizeDelta = new Vector2(8,8);
        }
    }





}

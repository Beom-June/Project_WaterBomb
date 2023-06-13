using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuPlayerController : MonoBehaviour
{
    private Camera _cam = null;
    
    [Header("Zoom")]
    [SerializeField] private Vector3 _maxRot = Vector3.zero;
    [SerializeField] private Vector3 _startRot = Vector3.zero;
    [SerializeField] private float _rotSpeed = 2f;
    [SerializeField] private float _zoomTime = 0.5f;
    [SerializeField] private int _zoomSize = 35;
    [SerializeField] private GameObject[] _zoomDisGobs = null;
    [SerializeField] private GameObject[] _zoomEnaGobs = null;
    [SerializeField] private float _reboundTime = 0.15f;
    private bool _isZomming = false;

    [Header("Hit")]
    [SerializeField] private LayerMask _hitLayers = 1;
    [SerializeField] private GameObject _hitParticleGob = null;

    [Header("Bullet")]
    [SerializeField] private GameObject _bullet = null;
    [SerializeField] private Transform _bulletStartTrs = null;
    private bool _lastHit = false;

    [Header("End")]
    [SerializeField] private Transform _endStartTrs = null;
    [SerializeField] private Transform _endTrs = null;
    [SerializeField] private Vector3 _endOffSet = Vector3.zero;
    [SerializeField] private GameObject[] _endDisGobs = null;

    [SerializeField] private float _endTime = 2.0f;
    [SerializeField] private float _endRotTime = 1.2f;
    [SerializeField] private float _waitTime = 0.3f;
    [SerializeField] private float _endingUITime = 0.5f;
    [SerializeField] private float _endingUIZ = -6;
    private bool _isEnding = false; // 엔딩 연출중인지

    [Header("WaterGun")]
    [SerializeField] private Animator _gunAni = null;
    private void Awake() 
    {
        _cam = GetComponent<Camera>();
        _startRot = transform.rotation.eulerAngles;    
    }
    private void Update() 
    {
        if (MuGameManager.GameState == MuGameState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_isZomming)
                {
                    StartCoroutine(ZoomIn());
                }
            }
            else if (Input.GetMouseButton(0))
            {
                float yRot = Input.GetAxis("Mouse X");
                float xRot = -Input.GetAxis("Mouse Y");
                transform.eulerAngles += new Vector3(xRot, yRot, 0) * _rotSpeed;

            }
            else if (Input.GetMouseButtonUp(0)) // 발사
            {

                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1000, _hitLayers))
                {
                    if (hit.collider != null)
                    {

                        if (hit.collider.gameObject.layer == 10) // 타겟이 맞았을 경우
                        {
                            if (MuGameManager.Targets.Count > 1)
                            {
                                hit.collider.GetComponent<MuTargets>().Hit(hit.point);
                                Instantiate(_hitParticleGob, hit.point, Quaternion.identity);
                            }
                            else // 마지막 타겟이 맞았을 경우
                            {
                                _lastHit = true;
                                _endTrs.position = hit.point + _endOffSet;
                                GameObject bullet = Instantiate(_bullet, _bulletStartTrs.position, Quaternion.identity);
                                //StartCoroutine(bullet.GetComponent<MuBullets>().Shot(hit.point, _endTime + _waitTime));
                                bullet.GetComponent<MuBullets>().Shoot(hit.point, _endTime + _waitTime);
                                MuGameManager.GameState = MuGameState.End;
                            }
                        }
                        else
                        {
                            Instantiate(_hitParticleGob, hit.point, Quaternion.identity);
                        }

                    }
                }
                if (_isZomming)
                {
                    StartCoroutine(Rebound());
                }
            }
        }
        else if (MuGameManager.GameState == MuGameState.End)
        {
            if (!_isEnding)
                StartCoroutine(Ending());

        }
        else if (MuGameManager.GameState == MuGameState.EndUI)
        {
            if (_isEnding)
                StartCoroutine(EndingUI());
        }
        
    }

    private IEnumerator ZoomIn()
    {
        _isZomming = true;
        float curTime = 0f;

        for (int i=0; i<_zoomDisGobs.Length; i++)
        {
            _zoomDisGobs[i].SetActive(false);
        }
        for (int i=0; i<_zoomEnaGobs.Length; i++)
        {
            _zoomEnaGobs[i].SetActive(true);
        }

        while (curTime < _zoomTime)
        {
            curTime += Time.deltaTime;
            _cam.fieldOfView = Mathf.Lerp(60, _zoomSize, curTime/_zoomTime);
            yield return null;
        }
        yield break;
    }
    private IEnumerator Rebound()
    {
        _isZomming = false;
        if (!_lastHit)
        {
            float curTime = 0;
            Vector3 rot = transform.eulerAngles;
            while (curTime < _reboundTime)
            {
                curTime += Time.deltaTime;
                transform.eulerAngles = Vector3.Lerp(rot, rot - new Vector3(2.5f, 0, 0), curTime / _reboundTime);
                yield return null;
            }
            while (curTime > 0)
            {
                curTime -= Time.deltaTime;
                transform.eulerAngles = Vector3.Lerp(rot - new Vector3(2.5f, 0, 0), rot, (_reboundTime - curTime) / _reboundTime);
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return StartCoroutine(ZoomOut());
    }

    private IEnumerator ZoomOut()
    {
        for (int i = 0; i < _zoomDisGobs.Length; i++)
        {
            _zoomDisGobs[i].SetActive(true);
        }
        for (int i = 0; i < _zoomEnaGobs.Length; i++)
        {
            _zoomEnaGobs[i].SetActive(false);
        }
        transform.eulerAngles = _startRot;
        _gunAni.SetTrigger("Reload");
        float curTime = 0f;

        while (curTime < _zoomTime)
        {
            curTime += Time.deltaTime;
            _cam.fieldOfView = Mathf.Lerp(_zoomSize, 60, curTime/_zoomTime);
            yield return null;
        }
        _gunAni.ResetTrigger("Reload");
        yield break;
    }

    private IEnumerator Ending() // 엔딩 움직임
    {
        _isEnding = true;
        for (int i=0; i<_zoomDisGobs.Length; i++)
        {
            _zoomDisGobs[i].SetActive(false);
        }
        for (int i=0; i<_endDisGobs.Length; i++)
        {
            _endDisGobs[i].SetActive(false);
        }
        
        transform.position = _endStartTrs.position;
        transform.rotation = _endStartTrs.rotation;
        yield return new WaitForSeconds(_waitTime);
        StartCoroutine(EndingRot());
        float curTIme = 0;
        while (curTIme < _endTime)
        {
            curTIme += Time.deltaTime;
            transform.position = Vector3.Lerp(_endStartTrs.position, _endTrs.position, curTIme/_endTime);
            yield return null;
        }
        MuGameManager.GameState = MuGameState.EndUI;
        yield break;
    }
    private IEnumerator EndingRot() // 엔딩 회전값
    {
        yield return new WaitForSeconds(_endTime - _endRotTime);
        float curTIme = 0;
        while (curTIme < _endRotTime)
        {
            curTIme += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(_endStartTrs.rotation, _endTrs.rotation, curTIme / _endTime);
            yield return null;
        }
        yield break;
    }


    private IEnumerator EndingUI()
    {
        _isEnding = false;
        yield return new WaitForSeconds(0.6f);
        float curTime = 0;
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        while (curTime < _endingUITime)
        {
            curTime += Time.deltaTime;
            transform.position = Vector3.Lerp(pos, new Vector3(0, pos.y, pos.z + _endingUIZ), curTime / _endingUITime);
            transform.rotation = Quaternion.Lerp(rot, Quaternion.identity, curTime / _endingUITime);
            yield return null;
        }
    }

}
   
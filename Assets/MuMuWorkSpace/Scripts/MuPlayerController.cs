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
    private bool _isZomming = false;
    [SerializeField] private GameObject[] _zoomDisGobs = null;
    [SerializeField] private GameObject[] _zoomEnaGobs = null;

    [Header("Shoot")]
    [SerializeField] private LayerMask _hitLayers = 1;
    [SerializeField] private GameObject _hitParticleGob = null;
    [Header("Bullet")]
    [SerializeField] private GameObject _bullet = null;
    [SerializeField] private Transform _bulletStartTrs = null;

    [Header("End")]
    [SerializeField] private Transform _endStartTrs = null;
    [SerializeField] private Transform _endTrs = null;
    [SerializeField] private Vector3 _endOffSet = Vector3.zero;

    [SerializeField] private float _endTime = 2.0f;
    private bool _isEnding = false; // 엔딩 연출중인지
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
                        Instantiate(_hitParticleGob, hit.point, Quaternion.identity); //  파티클 삽입

                        if (hit.collider.gameObject.layer == 10) // Target이 맞았을 경우
                        {
                            if (MuGameManager.Targets.Count > 1)
                            {
                                hit.collider.GetComponent<MuTargets>().Hit();
                            }
                            else
                            {
                                _endTrs.position = hit.point + _endOffSet;
                                GameObject bullet = Instantiate(_bullet, _bulletStartTrs.position, Quaternion.identity);
                                StartCoroutine(bullet.GetComponent<MuBullets>().Shot(hit.point, _endTime));
                                MuGameManager.GameState = MuGameState.End;
                            }
                        }

                    }
                }
                transform.eulerAngles = _startRot;
                if (_isZomming)
                {
                    StartCoroutine(ZoomOut());
                }
            }
        }
        else
        {
            if (!_isEnding)
                StartCoroutine(Ending());

            return;
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
    private IEnumerator ZoomOut()
    {
        _isZomming = false;
        float curTime = 0f;

        for (int i=0; i<_zoomDisGobs.Length; i++)
        {
            _zoomDisGobs[i].SetActive(true);
        }
        for (int i=0; i<_zoomEnaGobs.Length; i++)
        {
            _zoomEnaGobs[i].SetActive(false);
        }

        while (curTime < _zoomTime)
        {
            curTime += Time.deltaTime;
            _cam.fieldOfView = Mathf.Lerp(_zoomSize, 60, curTime/_zoomTime);
            yield return null;
        }
        yield break;
    }

    private IEnumerator Ending()
    {
        _isEnding = true;

        float curTIme = 0;
        transform.position = _endStartTrs.position;
        transform.rotation = _endStartTrs.rotation;
        while (curTIme < _endTime)
        {
            curTIme += Time.deltaTime;
            transform.position = Vector3.Lerp(_endStartTrs.position, _endTrs.position, curTIme/_endTime);
            transform.rotation = Quaternion.Slerp(_endStartTrs.rotation, _endTrs.rotation, curTIme/_endTime);
            yield return null;
        }
        MuGameManager.GameState = MuGameState.EndUI;
        yield break;
    }

}
   
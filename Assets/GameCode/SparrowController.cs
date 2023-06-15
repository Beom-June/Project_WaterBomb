using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparrowController : MonoBehaviour
{
    [SerializeField] private List<Transform> _wayPoints;                    //  이동 경로 웨이포인트
    private Transform _currentWaypoint;                    //  현재 웨이포인트
    [SerializeField] private float _speed = 5f;                             //  이동 속도
    [SerializeField] private bool _isMoving;                                //  이동 중인지 여부

    [Header("Event Settings")]
    [SerializeField] [Range(0f, 1f)] private float _spawnProbability = 0.3f; // 생성 확률
    [SerializeField] private GameObject _pooPrefab;                         //  생성할 프리팹
    [SerializeField] private bool _isInstantiate;                           //  true 누른 곳만 생성 되게 제한

    void Start()
    {
        // 초기 웨이포인트 설정
        if (_wayPoints.Count > 0)
            _currentWaypoint = _wayPoints[Random.Range(0, _wayPoints.Count)];

        // 이동 방향으로 쳐다보도록 설정
        if (_currentWaypoint != null)
            transform.LookAt(_currentWaypoint.position);
    }

    void Update()
    {
        SparrowMove();
    }

    private void SparrowMove()
    {
        if (_isMoving)
        {
            // 현재 웨이포인트를 향해 이동
            transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint.position, _speed * Time.deltaTime);

            // 현재 웨이포인트에 도착한 경우 다음 웨이포인트로 설정
            if (transform.position == _currentWaypoint.position)
            {
                _currentWaypoint = _wayPoints[Random.Range(0, _wayPoints.Count)];
                transform.LookAt(_currentWaypoint.position);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("WayPoint"))
        {
            if (_isInstantiate && Random.value <= _spawnProbability)
            {
                Instantiate(_pooPrefab, transform.position, transform.rotation);
            }
        }
    }
}

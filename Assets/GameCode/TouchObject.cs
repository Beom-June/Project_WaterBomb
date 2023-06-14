using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchObject : MonoBehaviour
{
    private RaycastHit _rayHit, _hitWall;                                       //  레이 캐스트 선언
    private GameObject _touchObject;                                            //  스크립트를 갖고 있는 해당 오브젝트
    private Vector3 _mainCamPosition;                                           //  카메라 벡터 값 받아옴
    [SerializeField] private bool _isRotating = false;                          //  마우스 우 클릭시 회전을 하기 위한 bool 값
    [SerializeField] private bool _isExit = false;                              //  한 번더 터치시 로테이션 빠져나오기 위한 bool 값
    [SerializeField] private float _rotSpeed = 200.0f;                           //  오브젝트 로테이션 속도

    private void Start()
    {
        _mainCamPosition = Camera.main.transform.position;
    }


    private void FixedUpdate()
    {
        if (_isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X") * _rotSpeed * Time.deltaTime;
            Vector3 rotation = new Vector3(0, -mouseX, 0);
            transform.Rotate(rotation);
        }
    }

    // 마우스를 떼면
    private void OnMouseUp()
    {
        //  부모 해제
        transform.SetParent(null);
        Destroy(_touchObject);
    }

    // 마우스(좌클릭)을 누르면
    private void OnMouseDown()
    {
        //  마우스 좌 클릭시 레이 
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _rayHit))
        {
            // 해당 오브젝트는 마우스 포인트의 자식으로 보냄
            int desiredLayer = LayerMask.NameToLayer("TouchObject");
            if (_rayHit.transform.gameObject.layer == desiredLayer)
            {
                _touchObject = new GameObject(_rayHit.transform.gameObject.name);
                _touchObject.transform.position = _rayHit.point;
                transform.SetParent(_touchObject.transform);

            }
        }


        if (!_isExit)
        {
            _isRotating = true;
            _isExit = true;
        }
        else
        {
            _isRotating = false;
            _isExit = false;
        }
    }

    //  마우스를 드래그하는 동안
    private void OnMouseDrag()
    {
        MoveObejcet();
    }
    void OnMouseExit()
    {
        _isRotating = false;
    }

    //  오브젝트 드래그 메소드
    private void MoveObejcet()
    {
        // 드래그 이동 로직
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hitWall, Mathf.Infinity, LayerMask.GetMask("Wall")))
        {
            // 계산에 기준이 되는 y좌표인 height
            float _height = _touchObject.transform.position.y;

            // 카메라 -> 바닥 방향 이동 벡터를 구하고 이동할 다음 위치를 초기화
            Vector3 _camToWall = _hitWall.point - Camera.main.transform.position;
            Vector3 _nextPosition = Vector3.zero;

            // 비율을 대상으로 삼분 탐색 수행. iteration이 40번을 넘기면 안정적인 값이 나옴
            float lo = 0.0f, hi = 1.0f;
            for (int i = 0; i < 38; i++)
            {
                float diff = hi - lo;
                float p1 = lo + diff / 3;
                float p2 = hi - diff / 3;

                var v1 = _mainCamPosition + _camToWall * p1;
                var v2 = _mainCamPosition + _camToWall * p2;
                if (Mathf.Abs(v1.y - _height) > Mathf.Abs(v2.y - _height))
                {
                    _nextPosition = v2;
                    lo = p1;
                }
                else
                {
                    _nextPosition = v1;
                    hi = p2;
                }
            }
            _touchObject.transform.position = _nextPosition;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> _createObject;                                    //  생성할 오브젝트를 담는 리스트
    [SerializeField] private List<bool> _canCreate;                                            //  오브젝트당 한 번만 생성되게 막는 bool 값

    private void Start()
    {
        _canCreate = new List<bool>(new bool[_createObject.Count]); // 생성 가능 여부 리스트 초기화

        for (int i = 0; i < _canCreate.Count; i++)
        {
            _canCreate[i] = true; // 모든 요소를 true로 설정
        }
    }

    public void CreateObjectAtIndex(int _idx)
    {
        if (_idx >= 0 && _idx < _createObject.Count && _canCreate[_idx])
        {
            Vector3 spawnPosition = transform.position + Camera.main.transform.forward;
            Instantiate(_createObject[_idx], spawnPosition, transform.rotation);

            Debug.Log(_idx);
            // 생성되면 false로 변경
            _canCreate[_idx] = false;
            
        }
    }
    #region  생성할 오브젝트 담는 메소드
    public void CreateChair()
    {
        int index = 0; // 생성할 오브젝트의 인덱스
        CreateObjectAtIndex(index);
    }

    public void CreateBed()
    {
        int index = 1; // 생성할 오브젝트의 인덱스
        CreateObjectAtIndex(index);
    }

    public void CreateLamp()
    {
        int index = 2; // 생성할 오브젝트의 인덱스
        CreateObjectAtIndex(index);
    }
    public void CreateNightStand()
    {
        int index = 3; // 생성할 오브젝트의 인덱스
        CreateObjectAtIndex(index);
    }
    public void CreateWardrobe()
    {
        int index = 4; // 생성할 오브젝트의 인덱스
        CreateObjectAtIndex(index);
    }

    #endregion
}

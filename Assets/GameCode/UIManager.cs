using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas _uiCanvas;                                  //  UI Canvas
    [SerializeField] private List<RawImage> _targetImage;                       //  타겟 연동할 이미지 리스트
    [SerializeField] private GameObject _hitImage;                              //  Target hit시 띄워주는 Image
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas _uiCanvas;                                  //  UI Canvas
    [SerializeField] private List<RawImage> _targetImage;                       //  타겟 연동할 이미지 리스트
    [SerializeField] private GameObject _hitImage;                              //  Target hit시 띄워주는 Image

    [SerializeField] private Button _getButton;                                  //  다음 스테이지로 넘어가는 버튼
    [SerializeField] private Button _noThanks;                                  //  다음 스테이지로 넘어가는 버튼

    [Header("Bonus Arrow")]
    [SerializeField] private Image _bonusArrow;                                //  화살표 ui
    [SerializeField] private float _movementDistance = 100f;  // 이동 거리
    [SerializeField] private float _movementSpeed = 2f;      // 이동 속도

    private bool _isMovingRight = true;  // 현재 이동 방향

    [Header("Reward Item Fill")]
    [SerializeField] private Text _textPercent;                                 // 퍼센트  텍스트
    [SerializeField] private Image _imageFill;                                  //  채우는 이미지 (노란색)
    [SerializeField] private float _totalFill = 100.0f;
    [SerializeField] private float _currentFill;                                //  현재 채워진 양
    [SerializeField] private float _fillSpeed = 30.0f;                                   //  채워지는 속도
    private bool isEnded = true;                                                //  채우는 것이 끝났는지 확인하는 bool 값

    void Update()
    {
        if (isEnded)
        {
            // 키보드 1을 눌렀을 때 스킬 실행
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Trigger_Fill();
            }
        }
        else
        {
            Check_FillTime();
        }
    }

    void Start()
    {
        //   NextScene();  
        StartCoroutine(MoveArrow());
    }

    private IEnumerator MoveArrow()
    {
        while (true)
        {
            // 현재 위치와 목표 위치 계산
            float startX = _isMovingRight ? -_movementDistance : _movementDistance;
            float targetX = _isMovingRight ? _movementDistance : -_movementDistance;

            // 이동 애니메이션
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * _movementSpeed;
                float newX = Mathf.Lerp(startX, targetX, t);
                Vector3 newPosition = _bonusArrow.rectTransform.localPosition;
                newPosition.x = newX;
                _bonusArrow.rectTransform.localPosition = newPosition;
                yield return null;
            }

            // 이동 방향 변경
            _isMovingRight = !_isMovingRight;
        }
    }

    #region  Reward Fill 관련 함수
    void Check_FillTime()
    {
        _currentFill += Time.deltaTime * _fillSpeed;
        if (_currentFill < _totalFill)
        {
            Set_FillAmount(_currentFill);
        }
        else if (!isEnded)
        {
            End_FillTime();
        }
    }

    void End_FillTime()
    {
        Set_FillAmount(_totalFill);
        isEnded = true;
        // _textPercent.gameObject.SetActive(false);
    }

    // 호출시 차오르기 시작함
    public void Trigger_Fill()
    {
        if (!isEnded)
        {
            return;
        }
        else
        {
            Debug.Log("Fill Start");
            Reset_FillTime();
        }
    }

    void Reset_FillTime()
    {
        _textPercent.gameObject.SetActive(true);
        _currentFill = 0;
        Set_FillAmount(0);
        isEnded = false;
    }

    void Set_FillAmount(float value)
    {
        _imageFill.fillAmount = value / _totalFill;
        _textPercent.text = string.Format(" {0} %", value.ToString("0"));
    }


    #endregion

    #region Scene 이동 관련 함수


    public void NextScene()
    {
        // 다음 씬으로 넘어가는 로직을 작성
        SceneManager.LoadScene("Scene_Room");
    }

    private void QuitGame()
    {
        // 게임을 종료하는 로직
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}

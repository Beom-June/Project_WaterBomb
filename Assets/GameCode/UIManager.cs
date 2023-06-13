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

    [Header("Reward Item Fill")]
    [SerializeField] private Text _textPercent;                                 // 퍼센트  텍스트
    [SerializeField] private Image _imageFill;                                  //  채우는 이미지 (노란색)
    [SerializeField] private float _totalFill = 100.0f;
    [SerializeField] private float _currentFill;                                //  현재 채워진 양
    [SerializeField] private float _fillSpeed = 20.0f;                                   //  채워지는 속도
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
        _textPercent.gameObject.SetActive(false);
    }

    void Trigger_Fill()
    {
        if (!isEnded) return;
        Reset_FillTime();
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

    public void StartFillAnimation()
    {
        _imageFill.type = Image.Type.Filled;
        _imageFill.fillMethod = Image.FillMethod.Vertical;
        _imageFill.fillOrigin = 1;

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

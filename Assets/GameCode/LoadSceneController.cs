using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneController : MonoBehaviour
{
    [SerializeField] private Button _btnLoadScene;
    [SerializeField] private string desiredSceneName; // 로드하고자 하는 씬의 이름
    private void Start()
    {
        _btnLoadScene.onClick.AddListener(ChangeToDesiredScene);
    }

    private void ChangeToDesiredScene()
    {
        SceneManager.LoadScene(desiredSceneName);
    }
}

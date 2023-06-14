using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public enum MuGameState
{
    Playing, End, EndUI
}
public class MuGameManager : MonoBehaviour
{
    public static List<GameObject> Targets = new List<GameObject>();
    public static MuGameState GameState = MuGameState.Playing;
    private UIManager _uiManager = null;
    [SerializeField] private RawImage _endBackImg = null;
    [SerializeField] private GameObject _endCanvas = null;
    private bool _isEndChanging = false;

    public static int PlayerGold = 0;
    [Header("Bonus Stage")]
    [SerializeField] private GameObject[] _bulletImgs = null;
    private void Awake() 
    {
        GameState = MuGameState.Playing;    
    }
    private void Start() 
    {
        _uiManager = GameObject.FindObjectOfType<UIManager>();
        Targets = GameObject.FindGameObjectsWithTag("Target").ToList();
    }

    private void Update() 
    {
        if (GameState == MuGameState.EndUI)
        {
            if (!_isEndChanging)
                StartCoroutine(EndingUI());
        }
    }

    private IEnumerator EndingUI()
    {
        _isEndChanging = true;
        yield return new WaitForSeconds(0.2f);
        float curTime = 0;
        float targetTime = 0.4f;
        Color curColor = _endBackImg.color;
        Color targetColor = new Color(curColor.r, curColor.g, curColor.b, 0.5f);
        while (curTime < targetTime)
        {
            curTime += Time.deltaTime;
            _endBackImg.color = Color.Lerp(curColor, targetColor, curTime/targetTime);
            yield return null;
        }
        _endCanvas.SetActive(true);
        _uiManager.Trigger_Fill();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuTargets : MonoBehaviour
{
    [SerializeField] private GameObject _crossImg = null;
    public void Hit()
    {
        // 사망 효과 추가
        _crossImg.SetActive(true);
        MuGameManager.Targets.Remove(this.gameObject);
    }

}

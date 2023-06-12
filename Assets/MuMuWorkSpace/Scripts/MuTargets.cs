using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuTargets : MonoBehaviour
{
    public void Hit()
    {
        // 사망 효과 추가
        MuGameManager.Targets.Remove(this.gameObject);
    }

}

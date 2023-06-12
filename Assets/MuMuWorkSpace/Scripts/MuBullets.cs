using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuBullets : MonoBehaviour
{
    public IEnumerator Shot(Vector3 pos, float targetTime)
    {
        float curTime = 0;
        Vector3 curPos = transform.position;
        while (curTime < targetTime)
        {
            curTime += Time.deltaTime;
            transform.position = Vector3.Lerp(curPos, pos, curTime/targetTime);
            yield return null;
        }
        yield break;
    }

    private void OnCollisionEnter(Collision other) 
    {
        MuGameManager.Targets.Remove(this.gameObject);    
    }
}

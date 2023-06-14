using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuBullets : MonoBehaviour
{
    [SerializeField] private GameObject _hitParticleGob = null;
    [SerializeField] private GameObject _endFireworkGob = null;
    [SerializeField] private Vector3 _fireworkOffSet = Vector3.zero;
    private IEnumerator _shotCoroutine = null;

    public void Shoot(Vector3 pos, float targetTime) // Destroy하기 때문에 코루틴 인자를 받아 호출
    {
        _shotCoroutine = Shot(pos,targetTime);
        StartCoroutine(_shotCoroutine);
    }
    private IEnumerator Shot(Vector3 pos, float targetTime)// 물총 발사
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

    private void OnCollisionEnter(Collision other) // Target에만 충돌 Layer설정
    {
        StopCoroutine(_shotCoroutine);
        other.gameObject.GetComponent<MuTargetMain>().Hit(other.contacts[0].point);
        Instantiate(_hitParticleGob, transform.position, transform.rotation);
        Instantiate(_endFireworkGob, transform.position + _fireworkOffSet, Quaternion.identity);
        Instantiate(_endFireworkGob, transform.position + new Vector3(_fireworkOffSet.x, _fireworkOffSet.y, -_fireworkOffSet.z), Quaternion.identity);
        MuGameManager.GameState = MuGameState.EndUI;
        Destroy(this.gameObject);
    }
}

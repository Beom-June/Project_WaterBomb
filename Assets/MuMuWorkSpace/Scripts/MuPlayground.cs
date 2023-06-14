using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RideType
{
    Rotate, Swing, Siso, None
}
public class MuPlayground : MonoBehaviour
{
    [SerializeField] private RideType _type;

    [Header("Rotate")]
    [SerializeField] private float _rotateSpeed = 1;

    [Header("Swing")]
    [SerializeField] private float _swingSpeed = 1; 

    [Header("Siso")]
    [SerializeField] private float _sisoSpeed = 1; 

    private void Awake() 
    {
        if (_type == RideType.Swing)
        {
            StartCoroutine(SwingDown());
        }
        else if (_type == RideType.Siso)
        {
            StartCoroutine(SisoDown());
        }
    }
    private void Update() 
    {
        switch (_type)
        {
            case RideType.Rotate:
                transform.Rotate(new Vector3(0, _rotateSpeed * Time.deltaTime, 0));
                break;
        }    
    }
    private IEnumerator SwingDown()
    {
        float curTime = 0;
        float targetTime = 2 / _swingSpeed;
        float curX = transform.eulerAngles.x;
        while (curTime < targetTime)
        {
            curTime += Time.deltaTime;
            curX = Mathf.Lerp(-50, -120, curTime/targetTime);
            transform.eulerAngles = new Vector3(curX, 90, -90);
            yield return null;
        }
        yield return StartCoroutine(SwingUp());
    }
    private IEnumerator SwingUp()
    {

        float curTime = 0;
        float targetTime = 2 / _swingSpeed;
        float curX = transform.eulerAngles.x;
        while (curTime < targetTime)
        {
            curTime += Time.deltaTime;
            curX = Mathf.Lerp(-120, -50, curTime/targetTime);
            transform.eulerAngles = new Vector3(curX, 90, -90);
            yield return null;
        }
        yield return StartCoroutine(SwingDown());
    }
    private IEnumerator SisoDown()
    {
        float curTime = 0;
        float targetTime = 2 / _sisoSpeed;
        float curZ = transform.eulerAngles.x;
        while (curTime < targetTime)
        {
            curTime += Time.deltaTime;
            curZ = Mathf.Lerp(13, -13, curTime / targetTime);
            transform.eulerAngles = new Vector3(0, 0, curZ);
            yield return null;
        }
        yield return StartCoroutine(SisoUp());
    }
    private IEnumerator SisoUp()
    {

        float curTime = 0;
        float targetTime = 2 / _sisoSpeed;
        float curZ = transform.eulerAngles.x;
        while (curTime < targetTime)
        {
            curTime += Time.deltaTime;
            curZ = Mathf.Lerp(-13, 13, curTime / targetTime);
            transform.eulerAngles = new Vector3(0, 0, curZ);
            yield return null;
        }
        yield return StartCoroutine(SisoDown());
    }

}

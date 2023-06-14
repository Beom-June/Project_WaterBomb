using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoom : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private float _rotationSpeed = 5f;                             //  로테이션 속도
    [SerializeField] private float _maxRotationY = 60f;                             //  최대 Y축 한계
    [SerializeField] private float _minRotationY = -60f;                            //  최소 Y축 한계
    private float _rotationY = 0f;

    void Start()
    {
        if (_cam == null)
            _cam = GetComponent<Camera>();
    }

    void Update()
    {
         if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * _rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * _rotationSpeed;

            _rotationY += mouseX;
            _rotationY = Mathf.Clamp(_rotationY, _minRotationY, _maxRotationY);

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _rotationY, 0f);
        }
    }
}

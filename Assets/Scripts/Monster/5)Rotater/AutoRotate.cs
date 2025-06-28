using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [Tooltip("�ʴ� ȸ�� �ӵ� (�� ����)")]
    public Vector3 rotationSpeed = new Vector3(0f, 0f, 200f); // Z�� ȸ��

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
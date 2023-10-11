using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    private float _activeTime = 5f;
    private Rigidbody _rigidbody;

    void Awake()
    {
        // _rigidbody = GetComponent<Rigidbody>();
        // Destroy(gameObject, _activeTime);
    }

    private void Update()
    {
        _activeTime -= Time.deltaTime;
        if (_activeTime < 0)
        {
            BulletPoolM.instance.RecoverBullet(gameObject);
        }
    }

    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnEnable()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        _activeTime = 3;
    }
}
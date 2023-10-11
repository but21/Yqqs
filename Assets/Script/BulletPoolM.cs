using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolM : MonoBehaviour
{
    public static BulletPoolM instance;
    
    public GameObject bullet_prefab;

    private Queue<GameObject> _bulletPool;

    private int _poolInitNum = 20;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _bulletPool = new Queue<GameObject>();
        GameObject bullet = null;
        for (int i = 0; i < _poolInitNum; i++)
        {
            bullet = Instantiate(bullet_prefab);
            bullet.SetActive(false);
            _bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if (_bulletPool.Count > 0)
        {
            GameObject bullet = _bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            return Instantiate(bullet_prefab);
        }
    }

    public void RecoverBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        _bulletPool.Enqueue(bullet);
    }
}
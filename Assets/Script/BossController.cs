using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable All

public class BossController : MonoBehaviour
{
    public float HP = 100;
    private float _attackTime;
    private float _attackLastTime = 3.5f;

    public enum BossState
    {
        巡逻,
        攻击,
        死亡,
    }


    public BossState bossState;
    public Transform target;
    public Animator animator;

    public SkinnedMeshRenderer render;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        checkDie();
        switch (bossState)
        {
            case BossState.巡逻:
                FindTarget();
                break;
            case BossState.攻击:
                Attack();
                break;
            case BossState.死亡:
                Die();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bossState == BossState.死亡)
        {
            return;
        }

        HP--;
        StartCoroutine(changeColor());
    }

    void FindTarget()
    {
        // 控制boss旋转
        transform.Rotate(Vector3.up, Time.deltaTime * 30);

        Vector3 pos = target.position;
        pos.y = transform.position.y;

        // boss正前方与角色位置的夹角
        float angle = Vector3.Angle(transform.forward, transform.position - pos);

        if (angle < 15)
        {
            //时间戳
            _attackTime = Time.time;
            bossState = BossState.攻击;
        }
    }

    void Attack()
    {
        animator.SetBool("攻击", true);
        if (Time.time - _attackTime > _attackLastTime)
        {
            animator.SetBool("攻击", false);
            bossState = BossState.巡逻;
        }
    }

    void Die()
    {
        animator.SetBool("死亡", true);
    }

    void checkDie()
    {
        if (HP <= 0)
        {
            bossState = BossState.死亡;
        }
    }

    IEnumerator changeColor()
    {
        render.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        render.material.color = Color.white;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniEvent : MonoBehaviour
{
    public GameObject bullet_prefab;
    public string bulletLayer;
    public Transform[] shootPos;
    public float speed =500;

    public void shoot()
    {
        for (int i = 0; i < shootPos.Length; i++)
        {
            // GameObject bullet = Instantiate(bullet_prefab);
            GameObject bullet = BulletPoolM.instance.GetBullet();
            bullet.transform.position = shootPos[i].position;
            //transform.position + transform.forward + Vector3.up * 1.2f;
            bullet.GetComponent<Rigidbody>().AddForce(shootPos[i].forward * speed);
            bullet.layer = LayerMask.NameToLayer(bulletLayer);
        }
    }
}
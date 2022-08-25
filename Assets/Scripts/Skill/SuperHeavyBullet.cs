using Mirror;
using UnityEngine;

public class SuperHeavyBullet : Bullet
{
    protected override void Start()
    {
        m_BulletForce = 17;
        m_BulletDamagee = 50;

        base.Start();
    }

    [ServerCallback]
    public override void OnCollisionEnter(Collision collision)
    {
        if ((m_LayerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            print("HIT");
            //Damageable damageable = collision.transform.gameObject.GetComponent<Damageable>();
            //ApplyDamage(damageable);
        }

        Return();
    }

    [ServerCallback]
    public override void OnTriggerEnter(Collider other)
    {

    }
}
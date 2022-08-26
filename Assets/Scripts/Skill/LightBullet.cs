using Mirror;
using UnityEngine;

public class LightBullet : Bullet
{
    protected override void Start()
    {
        m_BulletForce = 17;
        m_BulletDamagee = 10;

        base.Start();
    }

    [ServerCallback]
    public override void OnCollisionEnter(Collision collision)
    {
        if ((m_LayerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            //Damageable damageable = collision.transform.gameObject.GetComponent<Damageable>();
            //ApplyDamage(damageable);
            Character character = collision.transform.gameObject.GetComponentInParent<Character>();

            if(character != null)
                character.OnHit(m_BulletDamagee);
        }

        Return();
    }

    [ServerCallback]
    public override void OnTriggerEnter(Collider other)
    {

    }
}
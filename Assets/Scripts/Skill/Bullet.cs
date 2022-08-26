using Mirror;
using System;
using UnityEngine;

public abstract class Bullet : NetworkBehaviour
{
    [SerializeField] protected LayerMask m_LayerMask;

    [Header("Data")]
    protected Rigidbody m_Rigidbody;
    [SyncVar]
    protected Vector3 m_LastVelocity;
    [SyncVar]
    [SerializeField] protected float m_BulletForce;
    [SyncVar]
    [SerializeField] protected int m_BulletDamagee;
    [SyncVar]
    [SerializeField] protected Vector3 _Point;
    [SyncVar]
    [SerializeField] protected Vector3 _Angle;

    protected virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.AddForce(_Point * m_BulletForce, ForceMode.VelocityChange);
    }

    public void Apply(Transform point, float bulletForce, int bulletDamage)
    {
        m_BulletForce = bulletForce;
        m_BulletDamagee = bulletDamage;
        _Point = point.forward;
        _Angle = point.eulerAngles;
    }

    public virtual void Update()
    {
        m_LastVelocity = m_Rigidbody.velocity;
    }

    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnCollisionEnter(Collision collision);

    //public void ApplyDamage(Damageable damageable)
    //{
    //    DamageMessage damageMessage = new DamageMessage()
    //    {
    //        Damage = m_BulletDamagee
    //    };

    //    damageable.ApplyDamage(damageMessage);
    //}

    public void Return()
    {
        NetworkServer.Destroy(gameObject);
    }
}
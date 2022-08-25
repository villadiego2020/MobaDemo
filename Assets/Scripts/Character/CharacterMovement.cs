using Mirror;
using System.Collections;
using UnityEngine;

public class CharacterMovement : NetworkBehaviour
{
    [SerializeField] private Vector3 _TargetPosition;
    [SerializeField] private float _Speed = 2;
    [SerializeField] private GameObject _Curror;

    private void Start()
    {
        _TargetPosition = transform.position;
    }

    void Update()
    {
        // always update health bar.
        // (SyncVar hook would only update on clients, not on server)
        //healthBar.text = new string('-', health);

        // movement for local player
        if (isLocalPlayer)
        {

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    _TargetPosition = new Vector3(hit.point.x, 0, hit.point.z);

                    StartCoroutine(DoWaitForDestroy());
                    IEnumerator DoWaitForDestroy()
                    {
                        GameObject go = Instantiate(_Curror);
                        go.transform.position = _TargetPosition;
                        yield return new WaitForSeconds(0.15f);

                        Destroy(go);

                        //};
                    }
                }

                //// rotate
                //float horizontal = Input.GetAxis("Horizontal");
                //transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);

                //// move
                //float vertical = Input.GetAxis("Vertical");
                //Vector3 forward = transform.TransformDirection(Vector3.forward);
                //agent.velocity = forward * Mathf.Max(vertical, 0) * agent.speed;
                //animator.SetBool("Moving", agent.velocity != Vector3.zero);

                //// shoot
                //if (Input.GetKeyDown(shootKey))
                //{
                //    CmdFire();
                //}
            }

            MoveObject();
        }
    }

    void MoveObject()
    {
        transform.LookAt(_TargetPosition);
        transform.position = Vector3.MoveTowards(transform.position, _TargetPosition, _Speed * Time.deltaTime);

        Debug.DrawLine(transform.position, _TargetPosition, Color.red);
    }

    //// this is called on the server
    //[Command]
    //void CmdFire()
    //{
    //    //GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, projectileMount.rotation);
    //    //NetworkServer.Spawn(projectile);
    //    //RpcOnFire();
    //}

    //// this is called on the tank that fired for all observers
    //[ClientRpc]
    //void RpcOnFire()
    //{
    //    //animator.SetTrigger("Shoot");
    //}

    //[ServerCallback]
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<Projectile>() != null)
    //    {
    //        --health;
    //        if (health == 0)
    //            NetworkServer.Destroy(gameObject);
    //    }
    //}
}

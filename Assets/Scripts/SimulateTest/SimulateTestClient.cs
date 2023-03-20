using Mirror;
using UnityEngine;

namespace Assets.SimulateTest
{
    public class SimulateTestClient : NetworkBehaviour
    {
        private GameObject _Follower;

        [SyncVar]
        public int Speed = 50;

        void Update()
        {
            if(isLocalPlayer)
            {
                float xAxisValue = Input.GetAxis("Horizontal") * Speed;
                float zAxisValue = Input.GetAxis("Vertical") * Speed;

                transform.position = new Vector3(transform.position.x + xAxisValue, transform.position.y, transform.position.z + zAxisValue);
            }
        }

        public void Setup()
        {
            _Follower = gameObject.transform.Find("Main Camera").gameObject;
            _Follower.SetActive(true);
        }
    }
}
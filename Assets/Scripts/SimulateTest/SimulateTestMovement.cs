using Mirror;
using UnityEngine;

namespace Assets.SimulateTest
{
    public enum MoveState
    {
        FindRoute,
        Moving,
        Stop,
    }

    public class SimulateTestMovement : NetworkBehaviour
    {
        [SerializeField] private Vector3 _TargetPosition;
        [SerializeField] private float _Speed = 2;
        [SerializeField] private MoveState m_MoveState;

        [SerializeField] private float m_Timer;

        private void Start()
        {
            _TargetPosition = transform.position;

            m_Timer = Random.Range(0f,5f);
            m_MoveState = MoveState.Stop;
        }

        void Update()
        {
            switch (m_MoveState)
            {
                case MoveState.FindRoute:
                    FindRoute();
                    break;
                case MoveState.Moving:
                    Moving();
                    break;
                case MoveState.Stop:
                    Stop();
                    break;
            }
        }

        private void FindRoute()
        {
            _TargetPosition = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
            m_MoveState = MoveState.Moving;
        }

        private void Moving()
        {
            if(Vector3.Distance(transform.position, _TargetPosition) <= 1f)
            {
                m_MoveState = MoveState.Stop;
            }

            transform.LookAt(_TargetPosition);
            transform.position = Vector3.MoveTowards(transform.position, _TargetPosition, _Speed * Time.deltaTime);

            Debug.DrawLine(transform.position, _TargetPosition, Color.red);
        }

        private void Stop()
        {
            m_Timer -= Time.deltaTime;

            if(m_Timer <= 0)
            {
                m_MoveState = MoveState.FindRoute;
                m_Timer = Random.Range(0f, 5f);
            }
        }
    }
}
using Mirror;
using Mirror.SimpleWeb;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using static GamePlayBattle;

namespace Assets.SimulateTest
{
    public class SimulateTestManager : NetworkManager
    {
        private Config m_Config;

        [SerializeField] private GameObject m_SimulateTestPrefab;
        [SerializeField] private int m_ObjectAmount;
        [SerializeField] private int m_ClientAmount;

        [SerializeField] private TextMeshProUGUI m_ObjectAmountText;
        [SerializeField] private TextMeshProUGUI m_ClientAmountText;
        [SerializeField] private TextMeshProUGUI m_FramerateText;

        public override void Awake()
        {
            m_Config = LoadConfigFile();

            Application.targetFrameRate = m_Config.TargetFramerate;
            networkAddress = m_Config.NetworkAddress;
            ((SimpleWebTransport)transport).port = m_Config.Port;

            InvokeRepeating("CountFPS", 1f, 3f);

            base.Awake();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            Debug.Log("Start Server");
            StartCoroutine(nameof(DoSpawnObject));
        }

        public override void OnStopServer()
        {
            base.OnStopServer();

            Debug.Log("Stop Server");
            m_ObjectAmount = 0;
            m_ClientAmount = 0;
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Debug.Log("Newly client");
            GameObject player = Instantiate(playerPrefab);
            player.transform.position = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";

            NetworkServer.Spawn(player);
            NetworkServer.AddPlayerForConnection(conn, player);

            Debug.Log("Client Joined " + player.name);
            SetClientAmount(true);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);

            Debug.Log("Client Disconnected");

            SetClientAmount(false);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();

            RegisterClientMessages();
            StartCoroutine(nameof(WaitConnection));
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();

            UnregisterClientMessages();
        }

        IEnumerator WaitConnection()
        {
            yield return new WaitUntil(() => NetworkClient.localPlayer != null);

            SimulateTestClient client = NetworkClient.localPlayer.GetComponent<SimulateTestClient>();
            client.Setup();
        }

        private void RegisterClientMessages()
        {
            NetworkClient.RegisterHandler<SimulateInterval>(UpdateInterval);
        }

        private void UnregisterClientMessages()
        {
            NetworkClient.UnregisterHandler<SimulateInterval>();
        }

        private void UpdateInterval(SimulateInterval message)
        {
            if (NetworkClient.isHostClient)
                return;

            m_ObjectAmountText.text = $"Object: {message.ObjectAmount}";
            m_ClientAmountText.text = $"Client: {message.ClientAmount}";
        }

        private Config LoadConfigFile()
        {
            string filePath = Application.dataPath + "/StreamingAssets/Config.json";

            if (File.Exists(filePath)) //Problematic part 
            {
                string dataAsJson = File.ReadAllText(filePath);
                Config loadedData = JsonConvert.DeserializeObject<Config>(dataAsJson);

                Debug.Log(loadedData.ToString());

                return loadedData;
            }

            return new Config();
        }

        [Server]
        private void SetObjectAmount(bool isIncrease)
        {
            if (isIncrease)
                m_ObjectAmount++;
            else
                m_ObjectAmount--;

            m_ObjectAmountText.text = $"Object: {m_ObjectAmount}";

            SimulateInterval request = new SimulateInterval
            {
                ObjectAmount = m_ObjectAmount,
                ClientAmount = m_ClientAmount
            };

            NetworkServer.SendToAll<SimulateInterval> (request);
        }

        [Server]
        private void SetClientAmount(bool isIncrease)
        {
            if (isIncrease)
                m_ClientAmount++;
            else
                m_ClientAmount--;

            m_ClientAmountText.text = $"Client: {m_ClientAmount}";

            SimulateInterval request = new SimulateInterval
            {
                ObjectAmount = m_ObjectAmount,
                ClientAmount = m_ClientAmount
            };

            NetworkServer.SendToAll(request);
        }

        [Server]
        public IEnumerator DoSpawnObject()
        {
            while (m_ObjectAmount < m_Config.SimulateAmount && NetworkServer.active)
            {
                GameObject go = Instantiate(spawnPrefabs[0]);
                NetworkServer.Spawn(go);
                SetObjectAmount(true);

                Debug.Log("Simulate Object " + m_ObjectAmount);
                yield return new WaitForSeconds(Random.Range(0f, 3f));
            }
        }

        private void CountFPS()
        {
            m_FramerateText.text = $"FPS: {(1.0f / Time.smoothDeltaTime).ToString("00")}";
        }
    }

    public struct SimulateInterval : NetworkMessage
    {
        public int ObjectAmount;
        public int ClientAmount;
    }
}
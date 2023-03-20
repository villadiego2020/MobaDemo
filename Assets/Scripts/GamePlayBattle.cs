using kcp2k;
using Mirror;
using Mirror.SimpleWeb;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using UnityEngine;

public class GamePlayBattle : NetworkManager
{
    [SerializeField] private SpawnTransform[] _Transform;
    [SerializeField] private int _CurrentIndex;
    private Config _Config;

    public override void Awake()
    {
        _Config = LoadConfigFile();
        networkAddress = _Config.NetworkAddress;
        //((SimpleWebTransport)((LatencySimulation)transport).wrap).port = _Config.Port;
        ((SimpleWebTransport)transport).port = _Config.Port;
        //((KcpTransport)transport).Port = _Config.Port;

        base.Awake();
    } 

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if(_CurrentIndex > _Transform.Length -1)
        {
            _CurrentIndex = 0;
        }

        Transform startPos = _Transform[_CurrentIndex].transform;
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        Character character = player.GetComponent<Character>();
        character.UserNo = numPlayers;
        character.SpawnIndex = _CurrentIndex;
        // instantiating a "Player" prefab gives it the name "Player(clone)"
        // => appending the connectionId is WAY more useful for debugging!
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, player);
        _CurrentIndex++;
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        StartCoroutine(DoWaitForDestroy());
        IEnumerator DoWaitForDestroy()
        {
            yield return new WaitForSeconds(1f);

            Character character = NetworkClient.connection.identity.gameObject.GetComponent<Character>();
            character.Setup("Player" + ":" + numPlayers, _Transform[character.SpawnIndex]);
            character.ConfiMP(_Config.MPRegenerateRate, _Config.MPRegenerateValue);
            character.ConfigSkillCooldown(_Config.SkillCooldown);
            character.ConfigSkillUsageMP(20, 50, 60, 90);
        };
    }

    private Config LoadConfigFile()
    {
        string filePath = Application.dataPath + "/StreamingAssets/Config.json";

        if (File.Exists(filePath)) //Problematic part 
        {
            string dataAsJson = File.ReadAllText(filePath);
            Config loadedData = JsonConvert.DeserializeObject<Config>(dataAsJson);

            return loadedData;
        }

        return new Config();
    }

    public struct Config
    {
        [JsonProperty("network_address")]
        public string NetworkAddress { get; set; }

        [JsonProperty("port")]
        public ushort Port { get; set; }

        [JsonProperty("mp_regen_rate")]
        public int MPRegenerateRate { get; set; }

        [JsonProperty("mp_regen_value")]
        public int MPRegenerateValue { get; set; }

        [JsonProperty("skill_cooldown")]
        public int SkillCooldown { get; set; }

        [JsonProperty("simulate_amount")]
        public int SimulateAmount { get; set; }

        [JsonProperty("target_fps")]
        public int TargetFramerate { get; set; }

        public override string ToString()
        {
            return $"NetworkAddress {NetworkAddress}:{Port}" +
                $"SimulateAmount {SimulateAmount}" +
                $"TargetFramerate {TargetFramerate}";
        }
    }
}
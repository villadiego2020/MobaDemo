using kcp2k;
using Mirror;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using UnityEngine;

public class GamePlayBattle : NetworkManager
{
    private Config _Config;

    public override void Awake()
    {
        _Config = LoadConfigFile();
        networkAddress = _Config.NetworkAddress;
        ((KcpTransport)transport).Port = _Config.Port;

        base.Awake();
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        StartCoroutine(DoWaitForDestroy());
        IEnumerator DoWaitForDestroy()
        {
            yield return new WaitForSeconds(1f);

            Character character = NetworkClient.connection.identity.gameObject.GetComponent<Character>();
            character.Setup(NetworkClient.connection.identity.name + ":" + numPlayers);
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
    }
}

using Mirror;
using System.Collections;
using UnityEngine;

public class Character : NetworkBehaviour
{
    [Header("Character - UI")]
    [SerializeField] private CharacterHUD _CharacterHUD;
    [SerializeField] private CharacterRegenerateMP _CharacterRegenerateMP;
    [SerializeField] private CharacterSkillCooldown _CharacterSkillCooldown;

    private CharacterStatistic _CharacterStatistic;

    [Header("Effect")]
    [SerializeField] private Transform _EffectFrontPivot;
    [SerializeField] private Transform _EffectBodyPivot;

    [Header("Effect - Prefab")]
    [SerializeField] private GameObject _HealPrefab;
    [SerializeField] private GameObject _Atk1Prefab;
    [SerializeField] private GameObject _Atk2Prefab;
    [SerializeField] private GameObject _Atk3Prefab;

    [SyncVar]
    [SerializeField] private float _BulletForce = 17;
    [SerializeField] private bool isSkill1Cooldown;
    [SerializeField] private bool isSkill2Cooldown;
    [SerializeField] private bool isSkill3Cooldown;
    [SerializeField] private bool isSkill4Cooldown;

    [SerializeField] private GameObject _Model;
    [SerializeField] private Follower _Follower;

    private int _Skill1MPUsage;
    private int _Skill2MPUsage;
    private int _Skill3MPUsage;
    private int _Skill4MPUsage;

    public void Setup(string characterName)
    {
        GameObject uiGo = GameObject.Find("UICanvas");

        _CharacterHUD = uiGo.GetComponent<CharacterHUD>();
        _CharacterRegenerateMP = uiGo.GetComponent<CharacterRegenerateMP>();
        _CharacterSkillCooldown = uiGo.GetComponent<CharacterSkillCooldown>();

        GameObject camGo = GameObject.Find("CameraPivot");
        _Follower = camGo.GetComponent<Follower>();
        _Follower.Target = _Model.transform;

        BaseStatistic stat = new BaseStatistic()
        {
            Atk = 20,
            MaxHP = 200,
            HP = 100,
            MaxMP = 100,
            MP = 100
        };

        _CharacterStatistic = new CharacterStatistic()
        {
            Name = characterName,
            Stat = stat
        };

        _CharacterHUD.SetName(_CharacterStatistic.Name);
        _CharacterHUD.AdjustHP(_CharacterStatistic.Stat.HP, _CharacterStatistic.Stat.MaxHP);
        _CharacterHUD.AdjustMP(_CharacterStatistic.Stat.MP, _CharacterStatistic.Stat.MaxMP);
    }

    public void ConfiMP(int regenRate, int regenMPValue)
    {
        _CharacterRegenerateMP.RegenRate = regenRate;
        _CharacterRegenerateMP.RegenValue = regenMPValue;
        _CharacterRegenerateMP.AdjustMP = RegenerateMP;
    }

    public void ConfigSkillCooldown(int cooldownRate)
    {
        _CharacterSkillCooldown.ConfigSkillCooldown(cooldownRate, Skill1CooldownDone, Skill2CooldownDone, Skill3CooldownDone, Skill4CooldownDone);
    }

    public void ConfigSkillUsageMP(int skill1, int skill2, int skill3, int skill4)
    {
        _Skill1MPUsage = skill1;
        _Skill2MPUsage = skill2;
        _Skill3MPUsage = skill3;
        _Skill4MPUsage = skill4;

        _CharacterHUD.SetSkillsUsageMP(skill1, skill2, skill3, skill4);
    }

    public void RegenerateMP(int regenMPValue, int max)
    {
        _CharacterStatistic.Stat.MP += regenMPValue;
        _CharacterHUD.AdjustMP(_CharacterStatistic.Stat.MP, _CharacterStatistic.Stat.MaxMP);
    }

    public bool UseMP(int mp)
    {
        if (_CharacterStatistic.Stat.MP > 0 && _CharacterStatistic.Stat.MP >= mp)
        {
            _CharacterStatistic.Stat.MP -= mp;
            _CharacterHUD.AdjustMP(_CharacterStatistic.Stat.MP, _CharacterStatistic.Stat.MaxMP);
            _CharacterRegenerateMP.StartRegen(_CharacterStatistic.Stat.MP, _CharacterStatistic.Stat.MaxMP);

            return true;
        }

        return false;
    }
    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Heal(_Skill1MPUsage);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Atk1(_Skill2MPUsage);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Atk2(_Skill3MPUsage);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Atk3(_Skill4MPUsage);
        }
    }

    private void Heal(int mp)
    {
        if (isSkill1Cooldown)
            return;

        if (!UseMP(mp))
            return;

        isSkill1Cooldown = true;

        _CharacterStatistic.Stat.HP += 20;

        if (_CharacterStatistic.Stat.HP > _CharacterStatistic.Stat.MaxHP)
        {
            _CharacterStatistic.Stat.HP = _CharacterStatistic.Stat.MaxHP;
        }

        _CharacterHUD.AdjustHP(_CharacterStatistic.Stat.HP, _CharacterStatistic.Stat.MaxHP);
        _CharacterSkillCooldown.Skill1.StartCooldown();
        CommandServerHeal();
    }

    [Command]
    private void CommandServerHeal()
    {
        StartCoroutine(DoWaitForDestroy());
        IEnumerator DoWaitForDestroy()
        {
            GameObject go = Instantiate(_HealPrefab);
            go.transform.SetParent(_EffectBodyPivot);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            NetworkServer.Spawn(go);
            yield return new WaitForSeconds(2f);
            NetworkServer.Destroy(go);
        };
    }

    private void Atk1(int mp)
    {
        if (isSkill2Cooldown)
            return;

        if (!UseMP(mp))
            return;

        isSkill2Cooldown = true;
        _CharacterSkillCooldown.Skill2.StartCooldown();
        CommandServerAtk1();
    }

    [Command]
    private void CommandServerAtk1()
    {
        StartCoroutine(DoWaitForDestroy());
        IEnumerator DoWaitForDestroy()
        {
            GameObject go = Instantiate(_Atk1Prefab);
            go.transform.rotation = _EffectFrontPivot.transform.rotation;
            go.transform.position = _EffectFrontPivot.transform.position;
            go.transform.localScale = Vector3.one;
            NetworkServer.Spawn(go);

            LightBullet bullet = go.GetComponent<LightBullet>();
            bullet.Apply(_EffectFrontPivot, _BulletForce, 10);
            yield return new WaitForSeconds(0.15f);

        };
    }

    private void Atk2(int mp)
    {
        if (isSkill3Cooldown)
            return;

        if (!UseMP(mp))
            return;

        isSkill3Cooldown = true;
        _CharacterSkillCooldown.Skill3.StartCooldown();
        CommandServerAtk2();
    }

    [Command]
    private void CommandServerAtk2()
    {
        StartCoroutine(DoWaitForDestroy());
        IEnumerator DoWaitForDestroy()
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject go = Instantiate(_Atk2Prefab);
                go.transform.rotation = _EffectFrontPivot.transform.rotation;
                go.transform.position = _EffectFrontPivot.transform.position;
                NetworkServer.Spawn(go);

                HeavyBullet bullet = go.GetComponent<HeavyBullet>();
                bullet.Apply(_EffectFrontPivot, _BulletForce, 20);
                yield return new WaitForSeconds(0.15f);
            }
        };
    }

    private void Atk3(int mp)
    {
        if (isSkill4Cooldown)
            return;

        if (!UseMP(mp))
            return;

        isSkill4Cooldown = true;

        _CharacterSkillCooldown.Skill4.StartCooldown();
        CommandServerAtk3();
    }

    [Command]
    private void CommandServerAtk3()
    {
        StartCoroutine(DoWaitForDestroy());
        IEnumerator DoWaitForDestroy()
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject go = Instantiate(_Atk3Prefab);
                go.transform.rotation = _EffectFrontPivot.transform.rotation;
                go.transform.position = _EffectFrontPivot.transform.position;
                NetworkServer.Spawn(go);

                SuperHeavyBullet bullet = go.GetComponent<SuperHeavyBullet>();
                bullet.Apply(_EffectFrontPivot, _BulletForce, 50);
                yield return new WaitForSeconds(0.15f);
            }
        };
    }

    private void Skill1CooldownDone()
    {
        isSkill1Cooldown = false;
    }

    private void Skill2CooldownDone()
    {
        isSkill2Cooldown = false;
    }

    private void Skill3CooldownDone()
    {
        isSkill3Cooldown = false;
    }

    private void Skill4CooldownDone()
    {
        isSkill4Cooldown = false;
    }
}
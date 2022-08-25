using System;
using UnityEngine;

public class CharacterRegenerateMP : MonoBehaviour
{
    public int MaxMP { get; set; }
    public int MP { get; set; }
    public int RegenRate { get; set; }
    public int RegenValue { get; set; }

    [SerializeField] private float _Timer;
    [SerializeField] private bool _IsRegenerating;

    public Action<int, int> AdjustMP;

    private void Update()
    {
        if(_IsRegenerating)
        {
            _Timer -= Time.deltaTime;

            if(_Timer <= 0f)
            {
                Config();
                AdjustRegen();
            }
        }
    }

    public void StartRegen(int mpLeft, int maxMp)
    {
        MP = mpLeft;
        MaxMP = maxMp;
        Config();
    }

    private void Config()
    {
        _Timer = RegenRate;
        _IsRegenerating = true;
    }

    private void AdjustRegen()
    {
        MP += RegenValue;

        if (MP >= MaxMP)
        {
            MP = MaxMP;
            _IsRegenerating = false;
        }

        AdjustMP?.Invoke(RegenValue, MaxMP);

    }
}
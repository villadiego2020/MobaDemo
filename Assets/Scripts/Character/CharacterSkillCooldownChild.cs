using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkillCooldownChild : MonoBehaviour
{
    public float SkillCooldownRate { get; set; }

    [SerializeField] private float _Timer;
    [SerializeField] private Image _Background;
    [SerializeField] private bool _IsProgresing;

    public Action Done;

    private void Update()
    {
        if(_IsProgresing)
        {
            if (_Timer > 0)
            {
                _Timer -= Time.deltaTime;
                _Background.fillAmount = _Timer / SkillCooldownRate;

                if (_Timer <= 0)
                {
                    _Timer = 0;
                    _Background.fillAmount = 1;
                }
            }
            else
            {
                _IsProgresing = false;
                Config();
                Done?.Invoke();
            }
        }
    }

    public void StartCooldown()
    {
        _IsProgresing = true;
        Config();
    }

    private void Config()
    {
        _Timer = SkillCooldownRate;
    }
}
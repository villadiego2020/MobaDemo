using System;
using UnityEngine;

public class CharacterSkillCooldown : MonoBehaviour
{
    [SerializeField] private CharacterSkillCooldownChild _CharacterSkillCooldown1;
    [SerializeField] private CharacterSkillCooldownChild _CharacterSkillCooldown2;
    [SerializeField] private CharacterSkillCooldownChild _CharacterSkillCooldown3;
    [SerializeField] private CharacterSkillCooldownChild _CharacterSkillCooldown4;

    public CharacterSkillCooldownChild Skill1 => _CharacterSkillCooldown1;
    public CharacterSkillCooldownChild Skill2 => _CharacterSkillCooldown2;
    public CharacterSkillCooldownChild Skill3 => _CharacterSkillCooldown3;
    public CharacterSkillCooldownChild Skill4 => _CharacterSkillCooldown4;

    public void ConfigSkillCooldown(int cooldownRate, Action cb1, Action cb2, Action cb3, Action cb4)
    {
        _CharacterSkillCooldown1.SkillCooldownRate = cooldownRate;
        _CharacterSkillCooldown2.SkillCooldownRate = cooldownRate;
        _CharacterSkillCooldown3.SkillCooldownRate = cooldownRate;
        _CharacterSkillCooldown4.SkillCooldownRate = cooldownRate;

        _CharacterSkillCooldown1.Done = cb1;
        _CharacterSkillCooldown2.Done = cb2;
        _CharacterSkillCooldown3.Done = cb3;
        _CharacterSkillCooldown4.Done = cb4;
    }
}
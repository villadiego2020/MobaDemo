using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _Name;
    [SerializeField] private TextMeshProUGUI _TextHP;
    [SerializeField] private Image _BarHP;
    [SerializeField] private TextMeshProUGUI _TextMP;
    [SerializeField] private Image _BarMP;

    [SerializeField] private TextMeshProUGUI _SkillUsage1;
    [SerializeField] private TextMeshProUGUI _SkillUsage2;
    [SerializeField] private TextMeshProUGUI _SkillUsage3;
    [SerializeField] private TextMeshProUGUI _SkillUsage4;

    public void SetName(string characterName)
    {
        _Name.text = characterName;
    }

    public void AdjustHP(int hp, int maxHP)
    {
        _TextHP.text = $"{hp}/{maxHP}";
        _BarHP.fillAmount = (float)hp / (float)maxHP;
    }

    public void AdjustMP(int mp, int maxMP)
    {
        _TextMP.text = $"{mp}/{maxMP}";
        _BarMP.fillAmount = (float)mp / (float)maxMP;
    }

    public void SetSkillsUsageMP(int skill1, int skill2, int skill3, int skill4)
    {
        _SkillUsage1.text = $"{skill1}";
        _SkillUsage2.text = $"{skill2}";
        _SkillUsage3.text = $"{skill3}";
        _SkillUsage4.text = $"{skill4}";
    }
}
using Mirror;
using System;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    public Action<int> Heal;
    public Action<int> Atk1;
    public Action<int> Atk2;
    public Action<int> Atk3;

    public int Skill1MPUsage { get; set; }
    public int Skill2MPUsage { get; set; }
    public int Skill3MPUsage { get; set; }
    public int Skill4MPUsage { get; set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Heal?.Invoke(Skill1MPUsage);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Atk1?.Invoke(Skill2MPUsage);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Atk2?.Invoke(Skill3MPUsage);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Atk3?.Invoke(Skill4MPUsage);
        }
    }
}


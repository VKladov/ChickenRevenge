using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapuanSeePlayerTransition : PapuanTransition
{
    public PapuanSeePlayerTransition(Papuan papuan, PapuanState targetState) : base(papuan, targetState) { }

    public override bool NeedTransit()
    {
        List<Character> enemies = CharacterGroups.main.GetEnemies(Papuan);
        foreach (Character enemy in enemies)
        {
            if (Papuan.CouldSeeCharacter(enemy))
            {
                Papuan.SetTargetEnemy(enemy);
                return true;
            }
        }

        return false;
    }
}

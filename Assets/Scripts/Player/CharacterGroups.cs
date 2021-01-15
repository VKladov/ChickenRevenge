using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroups : MonoBehaviour
{
    public static CharacterGroups main = null;

    private List<Character> _groupA = new List<Character>();
    private List<Character> _groupB = new List<Character>();

    void Start()
    {
        if (main == null)
            main = this;
        else if (main == this)
            Destroy(gameObject);
    }

    public void AddItemToGroupA(Character character)
    {
        character.Destroyed += OnItemDestroyed;
        _groupA.Add(character);
    }

    public void AddItemToGroupB(Character character)
    {
        character.Destroyed += OnItemDestroyed;
        _groupB.Add(character);
    }

    public List<Character> GetEnemies(Character character)
    {
        if (_groupA.Contains(character))
            return _groupB;

        if (_groupB.Contains(character))
            return _groupA;

        return new List<Character>();
    }

    public void OnItemDestroyed(DamageReceiver receiver)
    {
        if (_groupA.Contains(receiver as Character))
            _groupA.Remove(receiver as Character);

        if (_groupB.Contains(receiver as Character))
            _groupB.Remove(receiver as Character);

        foreach (Character character in _groupA)
            if (character.TargetEnemy == receiver)
                character.SetTargetEnemy(null);

        foreach (Character character in _groupB)
            if (character.TargetEnemy == receiver)
                character.SetTargetEnemy(null);
    }
}

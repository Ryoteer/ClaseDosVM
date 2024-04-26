using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Enemy : MonoBehaviour
{
    [Header("<color=red>Values</color>")]
    [SerializeField] private int _maxHP = 100;

    private int _actualHP;

    private void Awake()
    {
        _actualHP = _maxHP;
    }

    public void TakeDamage(int dmg = 0)
    {
        _actualHP -= dmg;

        if(_actualHP <= 0)
        {
            print($"Oh my God, they killed {name}!");
            Destroy(gameObject);
        }
        else
        {
            print($"<color=red>{name}</color> has recieved <color=black>{dmg}</color> points of damage and it's left with <color=green>{_actualHP}</color> Health Points.");
        }
    }
}

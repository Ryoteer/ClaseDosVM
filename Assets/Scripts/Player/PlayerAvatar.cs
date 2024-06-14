using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    private Player _parent;

    private void Start()
    {
        _parent = GetComponentInParent<Player>();
    }

    public void CanAttack(int state)
    {
        _parent.CanAttack(state);
    }

    public void Attack()
    {
        _parent.Attack();
    }

    public void AreaAttack()
    {
        _parent.AreaAttack();
    }

    public void Jump()
    {
        _parent.Jump();
    }

    public void PlayStepClip()
    {
        _parent.PlayStepClip();
    }
}

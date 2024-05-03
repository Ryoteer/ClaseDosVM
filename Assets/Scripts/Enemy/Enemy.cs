using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Enemy : MonoBehaviour
{
    [Header("<color=red>AI</color>")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform[] _patrolNodes;
    [SerializeField] private float _changeNodeDist = .75f;
    [SerializeField] private float _chaseDist = 7.5f;
    [SerializeField] private float _attackDist = 2.5f;

    [Header("<color=red>Values</color>")]
    [SerializeField] private int _maxHP = 100;

    private int _actualHP;

    private Transform _actualNode = null;

    private void Awake()
    {
        _actualHP = _maxHP;
    }

    private void Start()
    {
        if (!_agent)
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        _actualNode = GetNewNode();

        _agent.SetDestination(_actualNode.position);
    }

    private void FixedUpdate()
    {
        if((_target.position - transform.position).sqrMagnitude <= Mathf.Pow(_chaseDist, 2))
        {
            _agent.SetDestination(_target.position);

            if((_target.position - transform.position).sqrMagnitude <= Mathf.Pow(_attackDist, 2))
            {
                _agent.isStopped = true;
                transform.LookAt(_target);
            }
            else if (_agent.isStopped)
            {
                _agent.isStopped = false;
            }
        }
        else if((_actualNode.position - transform.position).sqrMagnitude <= Mathf.Pow(_changeNodeDist, 2))
        {
            _actualNode = GetNewNode(_actualNode);

            _agent.SetDestination(_actualNode.position);
        }
        else
        {
            _agent.SetDestination(_actualNode.position);
        }
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

    private Transform GetNewNode(Transform actualNode = null)
    {
        Transform newNode = null;

        do
        {
            newNode = _patrolNodes[Random.Range(0, _patrolNodes.Length)];
        }
        while (newNode == actualNode);

        return newNode;
    }
}

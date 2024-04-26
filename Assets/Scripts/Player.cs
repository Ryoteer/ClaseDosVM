using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[SelectionBase]
public class Player : MonoBehaviour
{
    [Header("<color=orange>Animation</color>")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";
    [SerializeField] private string _onJumpName = "onJump";
    [SerializeField] private string _onAttackName = "onAttack";
    [SerializeField] private string _onAreaAttackName = "onAreaAttack";
    [SerializeField] private string _isMovingName = "isMoving";
    [SerializeField] private string _isGroundedName = "isGrounded";

    [Header("<color=orange>Inputs</color>")]
    [SerializeField] private KeyCode _attackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _areaAtackKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    [Header("<color=orange>Physics</color>")]
    [SerializeField] private float _areaAtkRayRadius = 2f;
    [SerializeField] private Transform _atkRayOrigin;
    [SerializeField] private float _atkRayDist = 1f;
    [SerializeField] private LayerMask _atkRayMask;
    [SerializeField] private float _jumpRayDist = .5f;
    [SerializeField] private LayerMask _jumpRayMask;
    [SerializeField] private float _movRayDist = .5f;
    [SerializeField] private LayerMask _movRayMask;

    [Header("<color=orange>Values</color>")]
    [Tooltip("Modifies Player's attack damage.")]
    [Range(0f, 50f)][SerializeField] private int _attackDmg = 20;
    [Tooltip("Modifies Player's jump height.")]
    [Range(0f, 10f)][SerializeField] private float _jumpForce = 5f;
    [Tooltip("Modifies Player's movement speed.")]
    [Range(1f, 10f)][SerializeField] private float _movSpeed = 5f;

    private bool _isAttacking = false;
    private float _xAxis = 0f, _zAxis = 0f;
    private Vector3 _dir = new(), _transformOffset = new(), _dirOffset = new(), _rayDir = new();

    private Rigidbody _rb;

    private Ray _jumpRay, _movRay, _attackRay;
    private RaycastHit _attackHit;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.angularDrag = 1f;
    }

    private void Start()
    {
        if (!_animator)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");

        _animator.SetBool(_isMovingName, (_xAxis != 0 || _zAxis != 0));
        _animator.SetBool(_isGroundedName, IsGrounded());

        _animator.SetFloat(_xAxisName, _xAxis);
        _animator.SetFloat(_zAxisName, _zAxis);

        if (Input.GetKeyDown(_jumpKey) && IsGrounded())
        {
            _animator.SetTrigger(_onJumpName);
        }

        if (Input.GetKeyDown(_attackKey) && !_isAttacking)
        {
            _animator.SetTrigger(_onAttackName);
        }
        else if (Input.GetKeyDown(_areaAtackKey))
        {
            _animator.SetTrigger(_onAreaAttackName);
        }
    }

    private void FixedUpdate()
    {
        if((_xAxis != 0f || _zAxis != 0f) && !IsBlocked(_xAxis, _zAxis))
        {
            Movement(_xAxis, _zAxis);
        }
    }

    private void Movement(float xAxis, float zAxis)
    {
        _dir = (transform.right * xAxis + transform.forward * zAxis).normalized;

        _rb.MovePosition(transform.position + _dir * _movSpeed * Time.fixedDeltaTime);
    }

    public void CanAttack(int state)
    {
        if(state == 0)
        {
            _isAttacking = false;
        }
        else if ((state == 1))
        {
            _isAttacking = true;
        }
    }

    public void Attack()
    {
        _attackRay = new Ray(_atkRayOrigin.position, transform.forward);

        if(Physics.Raycast(_attackRay, out _attackHit, _atkRayDist, _atkRayMask))
        {
            if(_attackHit.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_attackDmg);
            }
        }
    }

    public void AreaAttack()
    {
        Collider[] hitObjs = Physics.OverlapSphere(transform.position, _areaAtkRayRadius, _atkRayMask);

        foreach(Collider hitObj in hitObjs) 
        {
            if(hitObj.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_attackDmg * 2);
            }
        }
    }

    public void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private bool IsBlocked(float xAxis, float zAxis)
    {
        _dirOffset = new Vector3(transform.position.x,
                                       transform.position.y + .1f,
                                       transform.position.z);

        _rayDir = (transform.right * xAxis + transform.forward * zAxis);

        _movRay = new Ray(_dirOffset, _rayDir);

        return Physics.Raycast(_movRay, _movRayDist, _movRayMask);
    }

    private bool IsGrounded()
    {
        _transformOffset = new Vector3(transform.position.x,
                                       transform.position.y + _jumpRayDist / 4,
                                       transform.position.z);

        _jumpRay = new Ray(_transformOffset, -transform.up);

        return Physics.Raycast(_jumpRay, _jumpRayDist, _jumpRayMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_movRay);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_jumpRay);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_attackRay);
    }
}

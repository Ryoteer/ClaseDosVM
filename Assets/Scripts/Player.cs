using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("<color=orange>Animation</color>")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";
    [SerializeField] private string _onJumpName = "onJump";
    [SerializeField] private string _onAttackName = "onAttack";
    [SerializeField] private string _isMovingName = "isMoving";
    [SerializeField] private string _isGroundedName = "isGrounded";

    [Header("<color=orange>Inputs</color>")]
    [SerializeField] private KeyCode _attackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    [Header("<color=orange>Physics</color>")]
    [SerializeField] private float _jumpRayDist = .5f;
    [SerializeField] private LayerMask _jumpRayMask;
    [SerializeField] private float _movRayDist = .5f;
    [SerializeField] private LayerMask _movRayMask;

    [Header("<color=orange>Values</color>")]
    [Tooltip("Modifies Player's jump height.")]
    [Range(0f, 10f)][SerializeField] private float _jumpForce = 5f;
    [Tooltip("Modifies Player's movement speed.")]
    [Range(1f, 10f)][SerializeField] private float _movSpeed = 5f;

    private bool _canJump = true, _isAttacking = false;
    private float _xAxis = 0f, _zAxis = 0f;
    private Vector3 _dir = new Vector3(), _transformOffset = new(), _rayDir;

    private Rigidbody _rb;

    private Ray _jumpRay, _movRay;

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

        if (Input.GetKeyDown(_jumpKey) && _canJump)
        {
            _animator.SetTrigger(_onJumpName);
        }

        if (Input.GetKeyDown(_attackKey) && !_isAttacking)
        {
            _animator.SetTrigger(_onAttackName);
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
        print($"Usted se tiene que arrepentir de lo que dijo.");
    }

    public void CanJump(int state)
    {
        if (state == 0)
        {
            _canJump = false;
        }
        else if ((state == 1))
        {
            _canJump = true;
        }
    }

    public void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private bool IsBlocked(float xAxis, float zAxis)
    {
        _rayDir = (transform.right * xAxis + transform.forward * zAxis);

        _movRay = new Ray(transform.position, _rayDir);

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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";
    [SerializeField] private string _onJumpName = "onJump";
    [SerializeField] private string _onLandName = "onLand";

    [Header("Inputs")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    [Header("Values")]
    [Tooltip("Modifies Player's jump height.")]
    [Range(0f, 10f)][SerializeField] private float _jumpForce = 5f;
    [Tooltip("Modifies Player's movement speed.")]
    [Range(1f, 10f)][SerializeField] private float _movSpeed = 5f;

    private float _xAxis = 0f, _zAxis = 0f;
    private Vector3 _dir = new Vector3();

    private Rigidbody _rb;

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

        _animator.SetFloat(_xAxisName, _xAxis);
        _animator.SetFloat(_zAxisName, _zAxis);

        if (Input.GetKeyDown(_jumpKey))
        {
            _animator.SetTrigger(_onJumpName);
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if(_xAxis != 0f || _zAxis != 0f)
        {
            Movement(_xAxis, _zAxis);
        }
    }

    private void Movement(float xAxis, float zAxis)
    {
        _dir = (transform.right * xAxis + transform.forward * zAxis).normalized;

        _rb.MovePosition(transform.position + _dir * _movSpeed * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _animator.SetTrigger(_onLandName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
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

    private void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(_jumpKey))
        {
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
}

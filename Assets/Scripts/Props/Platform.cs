using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("<color=purple>Interaction</color>")]
    [SerializeField] private float _fadeTime = 3f;
    [SerializeField] private float _intermission = 10f;
    [SerializeField] private float _respawnTime = 3f;

    private bool _isActive = false;

    private Collider _collider;
    private Material _mat;

    private NavMeshModifier _modifier;
    private NavMeshSurface _surface;

    private Color _ogColor;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _mat = GetComponent<Renderer>().material;
        _ogColor = _mat.color;        
    }

    private void Start()
    {    
        if (!_modifier)
        {
            _modifier = GetComponent<NavMeshModifier>();
        }
        if (!_surface)
        {
            _surface = GetComponentInParent<NavMeshSurface>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() && !_isActive)
        {
            StartCoroutine(Interaction());
        }
    }

    private IEnumerator Interaction()
    {
        _isActive = !_isActive;

        float t = 0f;

        while(t < 1f)
        {
            t += Time.deltaTime / _fadeTime;

            _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, Mathf.Lerp(1f, 0f, t));

            yield return null;
        }

        _modifier.enabled = false;

        _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, 0f);
        _collider.enabled = false;

        _surface.BuildNavMesh();

        yield return new WaitForSeconds(_intermission);

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / _respawnTime;

            _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, Mathf.Lerp(0f, 1f, t));

            yield return null;
        }

        _modifier.enabled = true;

        _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, 1f);
        _collider.enabled = true;

        _surface.BuildNavMesh();

        _isActive = !_isActive;
    }
}

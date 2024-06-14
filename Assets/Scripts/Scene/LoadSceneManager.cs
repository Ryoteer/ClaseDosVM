using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    [Header("<color=green>Keys</color>")]
    [SerializeField]private KeyCode _activationKey = KeyCode.Space;

    [Header("<color=green>UI</color>")]
    [SerializeField] private Color _loadingColor = Color.red;
    [SerializeField] private Color _doneColor = Color.green;
    [SerializeField] private Image _loadFillImage;

    public void LoadSceneAsync(string scene)
    {
        StartCoroutine(AsyncLoad(scene));
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    private IEnumerator AsyncLoad(string scene)
    {
        _loadFillImage.color = _loadingColor;

        _loadFillImage.fillAmount = 0f;

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene);

        asyncOp.allowSceneActivation = false;

        float progress = 0f;

        while (asyncOp.progress < .9f)
        {
            progress = asyncOp.progress / .9f;

            _loadFillImage.fillAmount = progress;

            yield return null;
        }

        _loadFillImage.color = _doneColor;
        _loadFillImage.fillAmount = 1;

        while (!Input.GetKey(_activationKey))
        {
            yield return null;
        }

        asyncOp.allowSceneActivation = true;
    }
}

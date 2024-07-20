using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int _buildIndex;

    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => SceneManager.LoadScene(_buildIndex));
    }
}

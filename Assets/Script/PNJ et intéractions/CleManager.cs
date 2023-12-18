using UnityEngine;

public class CleManager : MonoBehaviour
{
    public static CleManager Instance;

    public bool HasKey { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

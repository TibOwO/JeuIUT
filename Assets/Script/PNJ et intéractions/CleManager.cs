using UnityEngine;

public class CleManager : MonoBehaviour
{
    public static CleManager Instance;
    private bool keyPickedUp = false;

    public bool KeyPickedUp
    {
        get { return keyPickedUp; }
        set { keyPickedUp = value; }
    }

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

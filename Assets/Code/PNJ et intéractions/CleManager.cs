using UnityEngine;

public class CleManager : MonoBehaviour
{
    public static CleManager Instance;
    private bool itemPickedUp = false;

    public bool ItemPickedUp
    {
        get { return itemPickedUp; }
        set { itemPickedUp = value; }
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

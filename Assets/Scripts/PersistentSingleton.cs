using UnityEngine;

public class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    new protected static void OnCreated(GameObject singleton)
    {
	    DontDestroyOnLoad(singleton);
	}
}

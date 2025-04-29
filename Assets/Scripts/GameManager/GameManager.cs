using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{


    [Header("Prefabs")]
    [HideInInspector]
    public static GameManager Instance;
    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    private GameObject blurVolume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: Keep between scenes
    }

    public void SetBlur(bool active)
    {
        blurVolume.SetActive(active);
    }
}




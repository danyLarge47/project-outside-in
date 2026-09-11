using System;
using System.Threading.Tasks;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    private const string PrefabResourcePath = "GameConfig";

    private static GameConfig instance;

    public static GameConfig Instance
    {
        get
        {
            if (instance != null) return instance;

            // Use an existing instance already in the scene, if any.
            instance = FindObjectOfType<GameConfig>();
            if (instance != null) return instance;

            // Otherwise instantiate from the prefab in a Resources folder.
            var prefab = Resources.Load<GameConfig>(PrefabResourcePath);
            if (prefab == null)
            {
                Debug.LogError($"GameConfig: no instance in the scene and no prefab found at Resources/{PrefabResourcePath}.");
                return null;
            }

            instance = Instantiate(prefab);
            instance.name = prefab.name;
            return instance;
        }
    }

    public GameProgression gameProgression;
    public VN_ContentDownloader contentDatabase;
    public Actions showCurtain;
    public Actions hideCurtain;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }


  
}

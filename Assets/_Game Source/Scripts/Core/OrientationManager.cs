using System;
using Sirenix.OdinInspector;
using UnityEngine;

public enum GameMode
{
    VisualNovel,
    Platformer
}

[DefaultExecutionOrder(-100)]
public class OrientationManager : MonoBehaviour
{
    public static OrientationManager Instance { get; private set; }

    public GameMode CurrentMode { get; private set; }
    public bool IsLocked { get; private set; }

    /// Fires when the mode flips, and once at initialization.
    public event Action<GameMode> OnModeChanged;

    [Tooltip("Which landscape variant to use when locking to Platformer.")] [SerializeField]
    private ScreenOrientation preferredLandscape = ScreenOrientation.LandscapeLeft;

    private int _lastW, _lastH;
    private bool _initialized;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ReleaseOrientation();
        // start in Auto, follow the phone
    }

    private void Update()
    {
        if (IsLocked) return; // OS won't rotate while locked
        if (Screen.width == _lastW && Screen.height == _lastH) return;
        _lastW = Screen.width;
        _lastH = Screen.height;
        ApplyMode(Screen.width >= Screen.height ? GameMode.Platformer : GameMode.VisualNovel);
    }

    // ---- Control -------------------------------------------------------
    [Button(ButtonSizes.Large)]
    private void ToggleOrientation()
    {
        var target = ScreenOrientation.Portrait;

        if (target == ScreenOrientation.Portrait)
            target = ScreenOrientation.LandscapeLeft;
        else
            target = ScreenOrientation.Portrait;

        SetGameOrientation(target);
    }


    public void SetGameOrientation(ScreenOrientation target)
    {
        if (target == ScreenOrientation.AutoRotation)
        {
            ReleaseOrientation();
            return;
        }

        Screen.orientation = target; // OS locks to this
        IsLocked = true;
        ApplyMode(ToMode(target)); // announce immediately (instant)
    }

    public void SetGameMode(GameMode mode)
    {
        SetGameOrientation(mode == GameMode.VisualNovel
            ? ScreenOrientation.Portrait
            : preferredLandscape);
    }

    public void ReleaseOrientation()
    {
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;

        IsLocked = false;
        _lastW = Screen.width;
        _lastH = Screen.height;
        ApplyMode(Screen.width >= Screen.height ? GameMode.Platformer : GameMode.VisualNovel);
    }

    // ---- Internals -----------------------------------------------------

    private static GameMode ToMode(ScreenOrientation o)
    {
        return o == ScreenOrientation.Portrait || o == ScreenOrientation.PortraitUpsideDown
            ? GameMode.VisualNovel
            : GameMode.Platformer;
    }

    private void ApplyMode(GameMode mode)
    {
        
        Debug.Log($"Change mode to {mode}");
        if (_initialized && mode == CurrentMode) return;
        _initialized = true;
        CurrentMode = mode;
        OnModeChanged?.Invoke(mode);
    }
}
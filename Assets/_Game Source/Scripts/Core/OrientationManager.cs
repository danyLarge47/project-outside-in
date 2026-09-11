using GameCreator.Runtime.VisualScripting;
using Sirenix.OdinInspector;
using UnityEngine;

public enum GameViewMode
{
    VisualNovel,
    Platformer
}

public class OrientationManager : MonoBehaviour
{
    [Title("State")]
    public GameViewMode CurrentMode;
    [ReadOnly] public bool IsLocked;

    [Title("Transitions")]
    public Actions transitionToVisualNovel;
    public Actions transitionToPlatformer;

    private void OnEnable()
    {
        GameEvents.OnModeChanged.AddListener(SetGameMode);
    }

    private void OnDisable()
    {
        GameEvents.OnModeChanged.RemoveListener(SetGameMode);
    }

    // ---- Control -------------------------------------------------------

    [Button(ButtonSizes.Large)]
    private void ToggleOrientation()
    {
        SetGameMode(CurrentMode == GameViewMode.Platformer
            ? GameViewMode.VisualNovel
            : GameViewMode.Platformer);
    }

    // async void: this is an event handler (OnModeChanged listener), which is the
    // one place async void is appropriate. IsLocked guards against re-entry.
    public async void SetGameMode(GameViewMode mode)
    {
        // Ignore new requests while a transition is already running.
        if (IsLocked) return;
        IsLocked = true;

        CurrentMode = mode;

        try
        {
            // Freeze rotation so the device can't rotate mid-transition,
            // then run the transition (block view -> adjust UI -> reveal).
            FreezeOrientation();

            Actions transition = mode == GameViewMode.Platformer
                ? transitionToPlatformer
                : transitionToVisualNovel;

            if (transition != null) await transition.Run();

            // Let orientation follow the device again once we've settled.
            ReleaseOrientation();
        }
        finally
        {
            // Always release the lock, even if the transition throws.
            IsLocked = false;
        }
    }

    // ---- Rotation -------------------------------------------------------

    /// Lock the screen to the orientation currently showing (no visual rotation,
    /// just prevents the device from rotating during a transition).
    public void FreezeOrientation()
    {
        Screen.orientation = Screen.width >= Screen.height
            ? ScreenOrientation.LandscapeLeft
            : ScreenOrientation.Portrait;
    }

    /// Re-enable auto-rotation so orientation follows the device again.
    public void ReleaseOrientation()
    {
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}

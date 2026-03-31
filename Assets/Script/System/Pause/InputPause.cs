using UnityEngine;

public class InputPause : MonoBehaviour
{
    private PauseManager _pauseManager = default;

    public void ToStart(PauseManager pause)
    {
        _pauseManager = pause;
    }

    public void UpdateMethod(PlayerInputManager input)
    {
        if (input.IsPause)
        {
            _pauseManager.Pause();
        }
    }
}

using UnityEngine;

public class NoteSlide : Note {

    private SpriteRenderer _renderer;
    private bool _pending;
    
    new void Start() {
        base.Start();

        _renderer = GetComponent<SpriteRenderer>();
    }
    
    new void Update() {
        if (G.PlaySettings.AutoPlay && G.InGame.Time >= time) {
            Judge();
        }
        
        if (_pending && G.InGame.Time - time >= 0) Judge();
        
        base.Update();
    }

    protected override void PlayErrorAnim() {
        AlphaAnim(_renderer);
    }

    protected override void PlayDestroyAnim() {
        ScaleAnim();
        AlphaAnim(_renderer);
    }

    protected override void HandleInput(Touch touch) {
        if (!(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)) return;

        var inputPosition = GetInputPosition(touch);

        if (!IsTargeted(inputPosition.x)) return;
        if (IsHiddenByOtherNote(inputPosition.x)) return;

        StartCoroutine(GiveInput());
        _pending = true;
    }

    protected override int DifferenceToJudge(float diff) {
        for (var i = 0; i < 3; i++)
            if (diff > G.InternalSettings.JudgeOfSlide[i])
                return i;

        return 3;
    }
    
}
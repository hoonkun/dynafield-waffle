using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteSlide : Note {

    private SpriteRenderer _renderer;
    private bool _pending;
    
    new void Start() {
        base.Start();
    }
    
    new void Update() {
        if (G.PlaySettings.AutoPlay && G.InGame.Time >= time) {
            Judge();
        }
        
        if (_pending && G.InGame.Time - time >= 0) Judge();
        
        base.Update();
    }
    
    public override void SetRenderer(SpriteRenderer noteRenderer) {
        base.SetRenderer(noteRenderer);
        _renderer = noteRenderer;
    }
    
    public override List<SpriteRenderer> GetRenderers() {
        return new List<SpriteRenderer> { _renderer };
    }

    protected override void PlayErrorAnim() {
        AlphaAnim(_renderer);
    }

    protected override void PlayDestroyAnim() {
        ScaleAnim();
        AlphaAnim(_renderer);
    }

    protected override bool IsPending() {
        return !destroying && !hasInput;
    }

    protected override float GetTimeDifference() {
        return Math.Abs(G.InGame.Time - time);
    }

    protected override void HandleInput(Touch touch) {
        if (!(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)) return;

        var inputPosition = GetInputPosition(touch);

        if (!IsTargeted(inputPosition)) return;
        if (IsHiddenByOtherNote(inputPosition)) return;

        StartCoroutine(GiveInput());
        _pending = true;
    }

    protected override int TimeDifferenceToJudge(float diff) {
        for (var i = 0; i < 3; i++)
            if (diff > G.InternalSettings.JudgeOfSlide[i])
                return i;

        return 3;
    }
    
}

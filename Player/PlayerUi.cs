using Godot;
using System;

public class PlayerUi : CanvasLayer
{
    private Label ammoLabel ,scoreLabel;
    private ProgressBar hpBar;
    private SceneTreeTween tween;
    public override void _EnterTree() {
        StaticRefs.PlayerUi = this;
    }
    
    public override void _Ready()
    {
        ammoLabel  = GetNode("BulletsNum") as Label;
        scoreLabel = GetNode("Score/ScoreLabel") as Label;
        hpBar = GetNode("hpLabel") as ProgressBar;
    }

    public void UpdateAmmoNumber(int n ) {
        ammoLabel.Text = n.ToString();
    }

    public void UpdateScore(int s) {
        scoreLabel.Text = s.ToString();
    }
    
    public void UpdateHp(HealthSystem health) {
        var value = (float) health.Health/(float)health.MaxHealth *100f;
        tween?.Stop();
        tween = CreateTween().SetTrans(trans:Tween.TransitionType.Quint);
        tween.TweenProperty(hpBar,"value",value,0.4f);
        GetNode<Label>("HealthLabel").Text = health.Health.ToString() + "/" +health.MaxHealth.ToString();
    }

    public void UpdateLevel(int l) {
        GetNode<Label>("Level/TextureRect/TextureRect2/Label").Text =l.ToString();
    }

}

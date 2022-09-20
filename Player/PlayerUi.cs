using Godot;
using System;

public class PlayerUi : CanvasLayer
{
    private Label ammoLabel ,scoreLabel;
    private ProgressBar hpLabel;
    public override void _EnterTree() {
        StaticRefs.PlayerUi = this;
    }
    
    public override void _Ready()
    {
        ammoLabel  = GetNode("BulletsNum") as Label;
        scoreLabel = GetNode("Score/ScoreLabel") as Label;
        hpLabel = GetNode("hpLabel") as ProgressBar;
    }

    public void UpdateAmmoNumber(int n ) {
        ammoLabel.Text = n.ToString();
    }

    public void UpdateScore(int s) {
        scoreLabel.Text = s.ToString();
    }
    
    public void UpdateHp(float s) {
        hpLabel.Value = s;
    }

}

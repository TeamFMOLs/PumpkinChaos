using Godot;
using System;

public class PlayerUi : Node2D
{
    private Label ammoLabel ,scoreLabel;
    public override void _EnterTree() {
        StaticRefs.PlayerUi = this;
    }
    
    public override void _Ready()
    {
        ammoLabel  = GetNode("BulletsNum") as Label;
        scoreLabel = GetNode("ScoreLabel") as Label;
    }

    public void UpdateAmmoNumber(int n ) {
        ammoLabel.Text = n.ToString();
    }

    public void UpdateScore(int s) {
        scoreLabel.Text = s.ToString();
    }


}

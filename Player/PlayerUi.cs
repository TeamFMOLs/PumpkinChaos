using Godot;
using System;

public class PlayerUi : Node2D
{
    private Label ammoLabel ;
    public override void _EnterTree() {
        StaticRefs.PlayerUi = this;
    }
    
    public override void _Ready()
    {
        ammoLabel  = GetNode("BulletsNum") as Label;
        
    }

    public void UpdateAmmoNumber(int n ) {
        ammoLabel.Text = n.ToString();
    }


}

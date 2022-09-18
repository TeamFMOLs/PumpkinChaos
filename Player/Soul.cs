using Godot;
using System;

public class Soul : Node2D
{
   /*  // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private Player CurrentPlayer;
    public override void _EnterTree()
    {
        
    }

    public override void _Ready()
    { 
        GlobalPosition = StaticRefs.CurrentPlayer.GlobalPosition;
        Visible = false;
    }

    public void SetCurrent() {
        StaticRefs.PlayerSoul = this;
        (GetNode("Camera2D")  as SceneCamera).Current= true;
        StaticRefs.CurrentCamera = GetNode("Camera2D") as SceneCamera;
    }


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void TransferToBody(Player player)
    {
        Visible = true;
        CurrentPlayer = player;
        var tween = CreateTween();
        tween.TweenProperty(this, "global_position", player.GlobalPosition, 0.6f);
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Show");
        GetTree().CreateTimer(0.6f).Connect("timeout", this, nameof(CompleteTransfer));

    }

    public void CompleteTransfer()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Hide");
        GetParent()?.RemoveChild(this);
        StaticRefs.CurrentPlayer.AddChild(this);
        CurrentPlayer.HasSoul = true;

    } */


}

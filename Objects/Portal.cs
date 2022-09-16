using Godot;
using System;

public class Portal : Area2D
{
    [Export]
    private NodePath TargetObjectPath;
    private Portal OtherPortal;
    
    public override void _Ready(){
        Monitoring = true;
        OtherPortal = GetNode(TargetObjectPath) as Portal;
        Connect("body_entered", this, nameof(OnObjectEntered));
    }

    void OnObjectEntered(PhysicsBody2D other)
    {
        if (other is Player)
        {
            if(!StaticRefs.CurrentPlayer._Teleport){
                var portalpos=OtherPortal.GlobalPosition;
                //StaticRefs.CurrentPlayer.GlobalPosition=portalpos;
                StaticRefs.CurrentPlayer.Teleport(portalpos);
            }
        }
    }

}
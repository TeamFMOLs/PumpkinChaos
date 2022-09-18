using Godot;
using System;

public class Ammo : Area2D
{
    float t = 0;
    Node2D Target;
    bool isFollowing = false;
    public SceneTreeTween currentTween;
    public override void _Ready()
    {
        Monitoring = true;
        Connect("body_entered" , this , nameof(OnContactWithPlayer));
    }

    private void OnContactWithPlayer(PhysicsBody2D other) {
        if(other is Player) {
            GetNode<AnimationPlayer>("AnimationPlayer").Play("shrink");
            Monitoring = false;
            //GetParent().RemoveChild(this);
            //other.AddChild(this);
            (other as Player).AddAmmo(this);
            GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
            isFollowing = true;
            Target = (other as Player).Weapon;
            currentTween.Stop();
        }
    }

    public void OnLand() {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("RESET");
    }

    public override void _Process(float delta) {
        if (isFollowing)
        {
            if (t<1)
            {
                GlobalPosition = GlobalPosition.LinearInterpolate(Target.GlobalPosition,t);  
                t +=delta/0.4f;
            }
            else
            {
                QueueFree();
            }
        }
    }

    
}

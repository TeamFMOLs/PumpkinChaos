using Godot;
using System;

public class PickableObject : Area2D
{
    float t = 0;
    Node2D Target;
    bool isFollowing = false;
    public SceneTreeTween currentTween;
    public override void _Ready()
    {
        Monitoring = true;
        Connect("body_entered", this, nameof(OnContactWithPlayer));
    }

    protected virtual void OnContactWithPlayer(PhysicsBody2D other)
    {
        if (other is Player)
        {
            GetNode<AnimationPlayer>("AnimationPlayer").Play("shrink");
            GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
            isFollowing = true;
            Target = (other as Player).Weapon;
            currentTween?.Stop();
        }
    }


    public override void _Process(float delta)
    {
        if (isFollowing)
        {
            Monitoring = false;
            if (t < 1)
            {
                GlobalPosition = GlobalPosition.LinearInterpolate(Target.GlobalPosition, t);
                t += delta / 0.4f;
            }
            else
            {
                QueueFree();
            }
        }
    }

    protected virtual void OnLand() {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("RESET");
    }

    public void StartTweenPos(Vector2 pos , float t) {
        currentTween = CreateTween();
        currentTween.TweenProperty(this,"global_position" ,pos,t );
        currentTween.TweenCallback(this,nameof(OnLand)  );   
    }

}

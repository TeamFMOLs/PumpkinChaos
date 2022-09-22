using Godot;
using System;

public class SpriteAnimationController : Node2D
{
    [Export]
    public bool IsPlayer = false;
    [Export]
    private NodePath AnimatedSpriteNodePath;
    [Export]
    private float TimeToChangeAnimationInterval = 0.3f, MinMovementVector = 1.2f;
    private AnimatedSprite animatedSprite;
    public CharacterController characterController;
    private Vector2 lastDir = Vector2.Up;
    private float lastChangeTime = 0;
    private float LastPlayerSpeed;
    public override void _Ready()
    {
        animatedSprite = GetNode(AnimatedSpriteNodePath) as AnimatedSprite;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        var angle = 0f;
        if (IsPlayer)
        {
            angle = (StaticRefs.inputManager.MousePos - StaticRefs.CurrentPlayer.GlobalPosition).Angle() * 180 / Mathf.Pi;
            GD.Print(angle);
        }
        else
            angle = characterController.Velocity.Angle() * 180 / Mathf.Pi;

        Vector2 newDir;
        if (angle >= -45 && angle < 45)
            newDir = Vector2.Right;
        else if (angle >= 45 && angle < 135 )
            newDir = Vector2.Down;
        else if (angle >= 135 && angle < 225 || angle >= -180 && angle < -135)
            newDir = Vector2.Left;
        else
            newDir = Vector2.Up;
        if (lastDir != newDir || (IsPlayer && Mathf.Abs(characterController.Velocity.Length() - LastPlayerSpeed) > MinMovementVector))
        {
            lastChangeTime += delta;
            if (lastChangeTime >= TimeToChangeAnimationInterval)
            {
                ChangeAnimation(newDir);
            }
        }
    }

    private void ChangeAnimation(Vector2 vec)
    {
        if (!IsPlayer)
        {
            if (vec == Vector2.Down)
            {
                animatedSprite.Play("front");
            }
            else if (vec == Vector2.Up)
            {
                animatedSprite.Play("back");
            }
            else if (vec == Vector2.Left)
            {
                animatedSprite.Play("side");
                animatedSprite.FlipH = false;
            }
            else if (vec == Vector2.Right)
            {
                animatedSprite.Play("side");
                animatedSprite.FlipH = true;
            }
        }
        else
        {
            animatedSprite.ZIndex = 0;
            if (characterController.Velocity.Length() >= MinMovementVector)
            {
                if (vec == Vector2.Down)
                {
                    animatedSprite.Play("front");
                }
                else if (vec == Vector2.Up)
                {
                    animatedSprite.ZIndex = 1;
                    animatedSprite.Play("back");
                }
                else if (vec == Vector2.Left)
                {
                    animatedSprite.Play("side");
                    animatedSprite.FlipH = false;
                }
                else if (vec == Vector2.Right)
                {
                    animatedSprite.Play("side");
                    animatedSprite.FlipH = true;
                }
            }
            else
            {
                if (vec == Vector2.Down)
                {
                    animatedSprite.Play("front_idle");
                }
                else if (vec == Vector2.Up)
                {
                    animatedSprite.ZIndex = 1;
                    animatedSprite.Play("back_idle");
                }
                else if (vec == Vector2.Left)
                {
                    animatedSprite.Play("side_idle");
                    animatedSprite.FlipH = false;
                }
                else if (vec == Vector2.Right)
                {
                    animatedSprite.Play("side_idle");
                    animatedSprite.FlipH = true;
                }
            }
            LastPlayerSpeed = characterController.Velocity.Length();
        }





        lastChangeTime = 0;
        lastDir = vec;
    }
}

using Godot;
using System;

public class SpriteAnimationController : Node2D
{
    [Export]
    private NodePath AnimatedSpriteNodePath;
    [Export]
    private float TimeToChangeAnimationInterval = 0.3f;
    private AnimatedSprite animatedSprite;
    public CharacterController characterController;
    private Vector2 lastDir = Vector2.Down;
    private float lastChangeTime = 0;
    public override void _Ready()
    {
        animatedSprite = GetNode(AnimatedSpriteNodePath) as AnimatedSprite;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        var angle = characterController.Velocity.Angle() * 180 / Mathf.Pi;
        Vector2 newDir;
        //GD.Print(angle * 180 / 3.14f);
        if (angle >= -45 && angle < 45)
            newDir = Vector2.Right;
        else if (angle >= 45 && angle < 135)
            newDir = Vector2.Down;
        else if (angle >= 135 && angle < 225)
            newDir = Vector2.Left;
        else
            newDir = Vector2.Up;
        // GD.Print(newDir);
        if (lastDir != newDir)
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
        /*  GD.Print("hmm");
         if (vec == Vector2.Up)
         {

             if (lastDir == Vector2.Down)
                 animatedSprite.Play("front_back");
             else
                 animatedSprite.Play("back");
         }
         else if (lastDir == Vector2.Up)
         {

             if (vec == Vector2.Right)
                 animatedSprite.Play("back_right");
             else if (vec == Vector2.Left)
                 animatedSprite.Play("back_left");
             else if (vec == Vector2.Down)
                 animatedSprite.Play("back_front");
         }
         else if (lastDir == Vector2.Right)
         {
             if (vec == Vector2.Down)
                 animatedSprite.Play("right_front");
             else if (vec == Vector2.Left)
                 animatedSprite.Play("right_left");
         }
         else if (lastDir == Vector2.Left)
         {
             if (vec == Vector2.Down)
                 animatedSprite.Play("left_front");
             else if (vec == Vector2.Right)
                 animatedSprite.Play("left_right");
         } 
         else if (lastDir == Vector2.Down)
         {
             if (vec == Vector2.Left)
                 animatedSprite.Play("front_left");
             else if (vec == Vector2.Right)
                 animatedSprite.Play("front_right");
         }  */
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



        lastChangeTime = 0;
        lastDir = vec;
    }
}

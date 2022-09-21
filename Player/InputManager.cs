using Godot;
using System;

public class InputManager : Node2D
{
    public Vector2 MoventDir {get; private set;} = Vector2.Zero;
    public Vector2 MousePos {get; private set;} = Vector2.Zero;
    public event Action<Vector2> OnAttack;
    public event Action OnMelee,OnDash;

    public override void _EnterTree()
    {
        StaticRefs.inputManager = this;
    }
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var left = Vector2.Left * (Input.GetActionStrength("move_left") - Input.GetActionStrength("move_right"));
        var up = Vector2.Up * (Input.GetActionStrength("move_up") - Input.GetActionStrength("move_down"));
        MoventDir = left + up;
        MousePos = GetGlobalMousePosition();
        if (Input.IsActionPressed("attack"))
        {
            OnAttack?.Invoke(MousePos);
        }
        if (Input.IsActionPressed("melee"))
        {
            OnMelee?.Invoke();
        }
        if(Input.IsActionPressed("Dash")){
            OnDash?.Invoke();
        }
    }
}

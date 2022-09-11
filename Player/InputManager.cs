using Godot;
using System;

public class InputManager : Node2D
{
    public bool MoveRight { get { return _right; } }
    public bool MoveLeft { get { return _left; } }
    public bool MoveUp { get { return _up; } }
    public bool MoveDown { get { return _down; } }
    private bool _right;
    private bool _left;
    private bool _up;
    private bool _down;
    
    public event Action ChangePlayer;

    public override void _EnterTree()
    {
        StaticRefs.inputManager = this;
    }
    public override void _Ready()
    {

    }


    public override void _Process(float delta)
    {
        _down = Input.IsActionPressed("move_down");
        _right = Input.IsActionPressed("move_right") ;
        _left = Input.IsActionPressed("move_left");
        _up = Input.IsActionPressed("move_up");

        if (Input.IsActionJustPressed("change_player"))
        {
            ChangePlayer?.Invoke();
        }
    }
}

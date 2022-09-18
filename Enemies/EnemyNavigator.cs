using Godot;
using System;
using System.Collections.Generic;

public class EnemyNavigator : Node2D
{

    public CharacterController Controller
    {
        get => _characterController;
        set
        {
            _characterController = value;
            _rayCast.AddException(_characterController);
        }
    }
    private CharacterController _characterController;
    private Vector2[] _rays = new Vector2[16];
    private Vector2[] hitLocations = new Vector2[16];
    private float[] _rayWeights = new float[16];
    private RayCast2D _rayCast;
    private bool _isMoving = false;
    private float _movementSpeed;

    private Vector2 _targetLocation;

    public override void _Ready()
    {
        _rayCast = GetNode<RayCast2D>("RayCast2D");
        for (int i = 0; i < 16; i++)
        {
            _rays[i] = new Vector2(Mathf.Cos(Mathf.Pi / 8 * i), Mathf.Sin(Mathf.Pi / 8 * i));
        }
    }
    public override void _Draw()
    {
        int index = 0;
        foreach (var ray in _rays)
        {
            if (ray != null && ray != Vector2.Zero)
            {
                DrawLine(Vector2.Zero, Vector2.Zero + ray * 100 * _rayWeights[index], Colors.Red);
                index++;
                //DrawCircle(GlobalPosition , 100, Colors.Red);
            }


        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_isMoving)
        {
            CalculateWeaights();
            var DesiredIndex = GetBestWeight();
            var DesiredDirection = _rays[DesiredIndex].Normalized();


            _characterController.Velocity = DesiredDirection * _movementSpeed;

        }
        Update();

    }


    public void GoToLocation(Vector2 location, float velocity)
    {
        _targetLocation = location;
        _movementSpeed = velocity;
        _isMoving = true;

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    private void CalculateWeaights()
    {
        for (int i = 0; i < _rays.Length; i++)
        {
            CheckCollision(i);
        }

        for (int i = 0; i < _rays.Length; i++)
        {
            var ray = _rays[i];
            var weight = ray.Normalized().Dot((_targetLocation - GlobalPosition).Normalized()) * 0.5f + 0.5f;
            foreach (var point in hitLocations)
            {
                if (point != Vector2.Inf)
                {
                   
                    weight -= ray.Normalized().Dot((point - GlobalPosition).Normalized())*0.4f * ( 100-GlobalPosition.DistanceTo(point))/40 ;
                }
            }
            _rayWeights[i] = weight;

        }
    }

    private int GetBestWeight()
    {
        int bestIndex = 0;
        float bestWeight = 0;
        for (int i = 0; i < _rayWeights.Length; i++)
        {
            if (_rayWeights[i] > bestWeight)
            {
                bestWeight = _rayWeights[i];
                bestIndex = i;
            }
        }
        return bestIndex;
    }

    private void CheckCollision(int index)
    {
        var ray = _rays[index];
        var rayCast = _rayCast;
        rayCast.CastTo = ray.Normalized() * 80;
        rayCast.ForceRaycastUpdate();

        if (rayCast.IsColliding())
            hitLocations[index] = rayCast.GetCollisionPoint();
        else
            hitLocations[index] = Vector2.Inf;
    }
}

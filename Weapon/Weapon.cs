using Godot;
using System;

public class Weapon : Node2D
{
    [Export]
    protected float AttackSpeed = 1.5f , BulletSpeed = 200f;
    [Export]
    protected PackedScene BulletScene;
    [Export]
    public int Ammo = 20;
    protected Timer FireTimer;
    protected bool _canFire = true;
    private Node2D WeaponHead;
    public override void _Ready()
    {
        FireTimer = GetNode("Timer") as Timer;
        WeaponHead = GetNode("GunParent/GunHead") as Node2D;
        FireTimer.WaitTime = 1/AttackSpeed;
        FireTimer.Connect("timeout" , this , nameof(ReEnableFire));
    }
    public void WeaponPointAt(Vector2 target)
    {
        LookAt(target);
    }

    public bool Attack(Vector2 target)
    {
        if (_canFire)
        {
            Fire(target);
            _canFire = false;
            FireTimer.Start();
            return true;
        }
        return false;
    }

    public void ReEnableFire() {
        _canFire = true;
    }


    protected void Fire( Vector2 taget) {
        
        var bullet = BulletScene.Instance() as Bullet;
        GetTree().Root.AddChild(bullet);
        bullet.GlobalPosition = GlobalPosition;
        bullet.GlobalRotation = WeaponHead.GlobalRotation;
        bullet.Velocity = BulletSpeed* (taget -  WeaponHead.GlobalPosition).Normalized();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    
}

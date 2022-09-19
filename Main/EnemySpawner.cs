using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner : Node2D
{
    [Export]
    private PackedScene FirstEnemy;
    private List<BasicEnemy> enemies = new List<BasicEnemy>();
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private BasicEnemy SpawnEnemy(PackedScene enemy , Vector2 pos) {
        var newEnemy = enemy.Instance<BasicEnemy>();
        GetTree().Root.AddChild(newEnemy);
        newEnemy.GlobalPosition = pos;
        enemies.Add(newEnemy);
        return newEnemy;
    }

    public void OnEnemyDied(BasicEnemy enemy) {
        enemies.Remove(enemy);
        if (enemies.Count ==0)
        {
            //
        }
    }
}

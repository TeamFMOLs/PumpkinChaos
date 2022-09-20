using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner : Node2D
{
    [Export]
    private PackedScene FirstEnemy;
    [Export]
    private float EnemyHealthScale = 0.2f , EnemyDamageScale  = 0.2f, EnemyNumberIncreasePerWave = 0.5f;
    [Export]
    private int StartingWaveNumber = 3;
    [Export]
    private Vector2 MinMaxTimeBetweenWaves = new Vector2(5,13);
    private List<BasicEnemy> enemies = new List<BasicEnemy>();
    private SceneTreeTimer timer;
    private int CurrentWaveIndex = 0;

    private Node2D start, end, sky;
    private RandomNumberGenerator rnd = new RandomNumberGenerator();
    public override void _Ready()
    {
        rnd.Randomize();
        start = GetNode<Node2D>("SrartPos");
        end = GetNode<Node2D>("EndPos");
        sky = GetNode<Node2D>("SkyPos");
        GetTree().CreateTimer(1f).Connect("timeout", this, nameof(BeginSpawning));
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
            SpawnNext();
        }
    }

    private void BeginSpawning() {
        SpawnNext();
    }

    private void SpawnNextWaveNow() {
        timer?.Disconnect("timeout",this ,nameof(SpawnNext));
        timer = null;
        SpawnNext();
    }

    private Vector2 PickRandomPoint()
    {
        var loc = new Vector2(
            rnd.RandfRange(start.GlobalPosition.x, end.GlobalPosition.x),
            rnd.RandfRange(start.GlobalPosition.y, end.GlobalPosition.y)
        );
        return loc;

    }

    private void SpawnNext() {
        var number =StartingWaveNumber+ StartingWaveNumber * (int) ((float)CurrentWaveIndex * EnemyNumberIncreasePerWave );
        number = (number >6)? 6 : number; 
        number = (enemies.Count == 0) ? number : number/2;
        for (int i = 0; i < number; i++)
        {
            var enm =SpawnEnemy(FirstEnemy,PickRandomPoint());
            SetEnemyStats(enm);
        }
        CurrentWaveIndex ++;
        GetTree().CreateTimer(rnd.RandfRange(MinMaxTimeBetweenWaves.x,MinMaxTimeBetweenWaves.y)).Connect("timeout",this,nameof(SpawnNext));
    }

    private void SetEnemyStats(BasicEnemy enemy) {
        enemy.healthSystem.MaxHealth += (int) ((float)enemy.healthSystem.MaxHealth * (float) CurrentWaveIndex * EnemyHealthScale);
        enemy.OnHitDamage += (int) ((float)enemy.OnHitDamage * (float) CurrentWaveIndex * EnemyDamageScale);
        enemy.ProjectileDamage += (int) ((float)enemy.ProjectileDamage * (float) CurrentWaveIndex * EnemyDamageScale);
    }
}

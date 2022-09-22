using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner : Node2D
{
    [Export]
    private PackedScene FirstEnemy,SecondEnemy,ThirdEnemy, FirstBoss;
    [Export]
    private float EnemyHealthScale = 0.2f, EnemyDamageScale = 0.2f, EnemyNumberIncreasePerWave = 0.5f , SecondEnemyChance = 0.33f,ThirdEnemyChance=0.22f;
    [Export]
    private int StartingWaveNumber = 3;
    [Export]
    private Vector2 MinMaxTimeBetweenWaves = new Vector2(5, 13);
    private List<BasicEnemy> enemies = new List<BasicEnemy>();
    private SceneTreeTimer timer;
    private int CurrentWaveIndex = 0;

    private Node2D start, end, sky;
    private RandomNumberGenerator rnd = new RandomNumberGenerator();
    public override void _EnterTree()
    {
        StaticRefs.EnemySpawner = this;
    }
    public override void _Ready()
    {
        rnd.Randomize();
        start = GetNode<Node2D>("SrartPos");
        end = GetNode<Node2D>("EndPos");
        sky = GetNode<Node2D>("SkyPos");
        GetTree().CreateTimer(1f, false).Connect("timeout", this, nameof(BeginSpawning));
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    private BasicEnemy SpawnEnemy(PackedScene enemy, Vector2 pos)
    {
        var newEnemy = enemy.Instance<BasicEnemy>();
        GetTree().Root.AddChild(newEnemy);
        newEnemy.GlobalPosition = pos;
        enemies.Add(newEnemy);
        return newEnemy;
    }

    public void OnEnemyDied(BasicEnemy enemy)
    {
        enemies.Remove(enemy);
        GD.Print("n");
        GD.Print(enemies.Count);
        if (enemies.Count == 0)
        {
            SpawnNextWaveNow();
        }
    }

    private void BeginSpawning()
    {
        SpawnNext();
    }

    private void SpawnNextWaveNow()
    {
        timer?.Disconnect("timeout", this, nameof(SpawnNext));
        timer = null;
        SpawnNext();
    }

    private Vector2 PickRandomPoint()
    {
        var loc = new Vector2(
            rnd.RandfRange(start.GlobalPosition.x, end.GlobalPosition.x),
            rnd.RandfRange(start.GlobalPosition.y, end.GlobalPosition.y)
        );
        while (loc.DistanceTo(StaticRefs.CurrentPlayer.GlobalPosition) <= 400)
        {
            loc = new Vector2(
            rnd.RandfRange(start.GlobalPosition.x, end.GlobalPosition.x),
            rnd.RandfRange(start.GlobalPosition.y, end.GlobalPosition.y)
        );
        }
        return loc;

    }

    private void SpawnNext()
    {
        var number = StartingWaveNumber + StartingWaveNumber * (int)((float)CurrentWaveIndex * EnemyNumberIncreasePerWave);
        number = (number > 10) ? 10 : number;
        number = (enemies.Count == 0) ? number : number / 2;
        
        for (int i = 0; i < number; i++)
        {
            PackedScene enemyToSpawn;
            if (rnd.RandfRange(0f,1f) <= ThirdEnemyChance)
            {
                enemyToSpawn = ThirdEnemy;   
            } else if (rnd.RandfRange(0f,1f) <= SecondEnemyChance) {
                enemyToSpawn = SecondEnemy;
            } else {
                enemyToSpawn = FirstEnemy;
            }
            var enm = SpawnEnemy(enemyToSpawn, PickRandomPoint());
            SetEnemyStats(enm);
        }
        CurrentWaveIndex++;
        if ((float)CurrentWaveIndex % 4f == 0)
        {
            var enm = SpawnEnemy(FirstBoss, PickRandomPoint());
            SetEnemyStats(enm);
        }
        GetTree().CreateTimer(rnd.RandfRange(MinMaxTimeBetweenWaves.x, MinMaxTimeBetweenWaves.y), false).Connect("timeout", this, nameof(SpawnNext));
    }

    public void KillAll() {
        foreach (var item in enemies)
        {
            item.QueueFree();   
        }
    }

    private void SetEnemyStats(BasicEnemy enemy)
    {
        enemy.healthSystem.ResetHealth(enemy.healthSystem.MaxHealth + (int)((float)enemy.healthSystem.MaxHealth * (float)CurrentWaveIndex / 2 * EnemyHealthScale));
        enemy.OnHitDamage += (int)((float)enemy.OnHitDamage * (float)CurrentWaveIndex / 2 * EnemyDamageScale);
        enemy.ProjectileDamage += (int)((float)enemy.ProjectileDamage * (float)CurrentWaveIndex / 2 * EnemyDamageScale);
    }
}

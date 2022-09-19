using Godot;
using System;

public class Loot : PickableObject , IScoreObject
{
    [Export]
    private int _score;
    public int Score { get => _score; set => _score = value; }

    protected override void OnContactWithPlayer(PhysicsBody2D other) {
        base.OnContactWithPlayer(other);
        if (other is Player)
        {
            GiveScore();
        }
    }
    


    public void GiveScore() {
        StaticRefs.CurrentPlayer.IncreaseScore(this.Score);
    }
}
using System;
using Godot;

public interface IScoreObject
{
    void GiveScore();
    int Score {get; set;}
}
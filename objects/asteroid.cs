using Godot;
using System;

public class asteroid : RigidBody
{
    private Spatial Model;
    private CollisionShape Shape;
    private float Life = 3f;
    private AnimationPlayer anim;
    private CanvasLayer Game;
    public override void _Ready()
    {
        Model = GetNode<Spatial>("model");
        Shape = GetNode<CollisionShape>("CollisionShape");
        ApplyImpulse(new Vector3(0, 0, 0), new Vector3(0, 0, (float)GD.RandRange(10, 25)));
        MudarEscala((float)GD.RandRange(1, 3));
        Translation = new Vector3((float)GD.RandRange(-9, 9), 0, - 120);
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        SetProcess(true);
    }

    public override void _Process(float delta)
    {
        if (Translation.x < - 11 || Translation.x > 11 || Translation.y < -5 || Translation.y > 5 || Translation.z < -125 || Translation.z > 5)
        {
            QueueFree();
        }
    }

    public void SetGame(CanvasLayer gmobj)
    {
        Game = gmobj;
    }
    
    private void MudarEscala(float tamanho)
    {
        Model.Scale = new Vector3(1.5f * tamanho, 1.5f * tamanho, 1.5f * tamanho);
        Shape.Scale = new Vector3(1.5f * tamanho, 1.5f * tamanho, 1.5f * tamanho);
    }
    
    public void Damage(float dmg)
    {
        Life -= dmg;
        Game.CallDeferred("Point", 1);
        if (Life <= 0)
        {
            anim.Play("explosao");
            Game.CallDeferred("Point", 2);
        }
    }

    public void _on_asteroid_body_entered(RigidBody body)
    {
        if (body.IsInGroup("player"))
        {
            body.CallDeferred("damage", 1);
        }
        anim.Play("explosao");
    }
}

using Godot;
using System;

public class fighter01 : RigidBody
{
    private MeshInstance Model;
    public Player jogador = new Player();
    [Export]
    private float Velocidade = 60.0f;
    [Export]
    NodePath Hud;
    CanvasLayer HUD;
    private float Rotacao = 0f;
    private int Tecla = 0;
    private int Life = 3;
    private PackedScene Bala;

    public override void _Ready()
    {
        Model = GetNode<MeshInstance>("mesh");
        Bala = GD.Load<PackedScene>("res://objects/shoot.tscn");
        HUD = GetNode<CanvasLayer>(Hud);
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("b_esquerda"))
        {
            if (Translation.x > -8)
            {
                Tecla = -1;
                Rotacao -= delta * 2;
            }
            else
                Tecla = 0;
        }
        else if (Input.IsActionPressed("b_direita"))
        {
            if (Translation.x < 8)
            {
                Tecla = 1;
                Rotacao += delta * 2;
            }
            else
                Tecla = 0;
        }
        else if (Input.IsActionJustReleased("b_esquerda"))
        {
            Tecla = 0;
        }
        else if (Input.IsActionJustReleased("b_direita"))
        {
            Tecla = 0;
        }
        if (Input.IsActionJustPressed("b_atirar"))
        {
            Area obj = (Area)Bala.Instance();
            GetParent().AddChild(obj);
            obj.Translation = new Vector3(Translation.x, Translation.y, Translation.z -5);
        }

        if (Math.Abs(Rotacao) > delta * 1.5f)
        {
            Rotacao -= Math.Sign(Rotacao) * delta * 1.5f;
        }
        else
            Rotacao = 0;
        ApplyImpulse(new Vector3(0, 0, 0), new Vector3(Tecla * Velocidade * delta, 0, 0));
        Model.Rotation = new Vector3(0, Rotacao, 0);
    }
    public Player GetPlayer()
    {
        return jogador;
    }

    public void damage(int value)
    {
        Life -= value;
        if (Life <= 0)
        {
            HUD.CallDeferred("endgame");
        }
    }
}

public class Player
{
    private int Pontos = 0;
}
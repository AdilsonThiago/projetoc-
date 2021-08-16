using Godot;
using System;
using System.Data;
using Npgsql;

public class hud : CanvasLayer
{
    [Export]
    private NodePath Nave;
    PackedScene Fase;
    private PackedScene Asteroide;
    private RigidBody NaveObj;
    private string mName = "";
    private Player ReferenciaJogador;
    private int Pontuacao = 0;
    private float IntervaloGeracao = 0f;
    static string adcon = "Server=tuffi.db.elephantsql.com;User Id=ggiyholl;Password=ihNUP2sEvme6ggdEB8tQQF1hc5ueaToh;";
    NpgsqlConnection connection = new NpgsqlConnection(adcon);
    public override void _Ready()
    {
        NaveObj = GetNode<RigidBody>(Nave);
        //ReferenciaJogador = (Player)NaveObj.Call("GetPlayer");
        Asteroide = GD.Load<PackedScene>("res://objects/asteroid.tscn");
        Fase = GD.Load<PackedScene>("res://fase.tscn");
        connection.Open();
    }
    public void Point(int pontos)
    {
        Pontuacao += pontos;
        GetNode<Label>("pontuacao").Text = "Pontuacao  : " + Pontuacao.ToString();
    }
    public override void _Process(float delta)
    {
        if (IntervaloGeracao > 0)
        {
            IntervaloGeracao -= delta;
        }
        else
        {
            RigidBody o = (RigidBody)Asteroide.Instance();
            GetParent().AddChild(o);
            o.CallDeferred("SetGame", this);
            IntervaloGeracao = 2.0f;
        }
    }

    public void Mudar_Nome(string novo_nome)
    {
        mName = novo_nome;
    }

    public void _on_Button_pressed()
    {
        Spatial o = (Spatial)Fase.Instance();
        GetParent().GetParent().AddChild(o);
        o.GetNode<CanvasLayer>("hud").CallDeferred("Mudar_Nome", mName);
        GetTree().Paused = false;
        GetParent().QueueFree();
    }

    public void endgame()
    {
        GetTree().Paused = true;
        GetNode<Label>("pontuacao").Visible = true;
        GetNode<ItemList>("ItemList").Visible = true;
        Register_Player(mName, Pontuacao);
        Show_Player_List();
    }

    private void Register_Player(string name, int point)
    {
        string cmdinsert = String.Format("INSERT INTO Players (Name, Points) VALUES ('{0}', '{1}')", name, point);
        using (NpgsqlCommand command = new NpgsqlCommand(cmdinsert, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    private void Show_Player_List()
    {
        string cmdgetlist = "SELECT * FROM Players ORDER BY Points";
        NpgsqlDataAdapter dataadapter = new NpgsqlDataAdapter(cmdgetlist, connection);
        DataSet DS = new DataSet();
        dataadapter.Fill(DS);
        foreach (DataTable table in DS.Tables)
        {
            foreach (DataRow row in table.Rows)
            {
                GetNode<ItemList>("ItemList").AddItem(row[1].ToString() + "   pontos : " + row[2].ToString());
            }
        }
    }
}
using ArcadeGame.Models;

namespace ArcadeGame.Patterns;

public interface IRenderer
{
    string Name { get; }
    void BeginFrame(GameWorld world, int tick);
    void Draw(GameObject gameObject, GameWorld world);
    void EndFrame(GameWorld world);
}

public sealed class ConsoleRenderer : IRenderer
{
    public string Name => "Console";

    public void BeginFrame(GameWorld world, int tick)
        => Console.WriteLine($"--- {Name} кадр {tick} ---");

    public void Draw(GameObject gameObject, GameWorld world)
    {
        var behavior = gameObject is Enemy enemy
            ? $" behavior={enemy.Behavior.Name}"
            : string.Empty;

        Console.WriteLine(
            $"{gameObject.Kind,-8} {gameObject.Id,-10} " +
            $"pos={gameObject.Position} vel={gameObject.Velocity}{behavior}");
    }

    public void EndFrame(GameWorld world) { }
}

public sealed class PseudographicRenderer : IRenderer
{
    private const int Width = 40;
    private const int Height = 20;

    private readonly char[,] _buffer = new char[Width, Height];

    public string Name => "Pseudographic";

    public void BeginFrame(GameWorld world, int tick)
    {
        Console.WriteLine($"--- {Name} кадр {tick} ---");

        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
            _buffer[x, y] = '.';
    }

    public void Draw(GameObject gameObject, GameWorld world)
    {
        var x = ToCell(gameObject.Position.X, world.Bounds.X, Width);
        var y = ToCell(gameObject.Position.Y, world.Bounds.Y, Height);

        _buffer[x, y] = gameObject.Kind switch
        {
            "Player" => 'P',
            "Enemy" => 'E',
            "Platform" => '#',
            _ => '?'
        };
    }

    public void EndFrame(GameWorld world)
    {
        for (var y = 0; y < Height; y++)
        {
            var line = new char[Width];
            for (var x = 0; x < Width; x++)
                line[x] = _buffer[x, y];
            Console.WriteLine(new string(line));
        }
    }

    private static int ToCell(double value, double bound, int cells)
    {
        if (bound <= 0) return 0;
        var index = (int)Math.Round((value / bound) * (cells - 1));
        return Math.Clamp(index, 0, cells - 1);
    }
}
using ArcadeGame.Models;

namespace ArcadeGame.Patterns;

public sealed class GameLoop
{
    private readonly GameWorld _world;
    private readonly IPhysicsModel _physics;
    private IRenderer _renderer;
    private int _tick;

    public GameLoop(GameWorld world, IPhysicsModel physics, IRenderer renderer)
    {
        _world = world ?? throw new ArgumentNullException(nameof(world));
        _physics = physics ?? throw new ArgumentNullException(nameof(physics));
        _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
    }

    public void SetRenderer(IRenderer renderer)
    {
        _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        Console.WriteLine($"GameLoop: рендерер изменён на {_renderer.Name}.");
    }

    public void Tick(double deltaTime)
    {
        if (deltaTime <= 0)
            throw new ArgumentOutOfRangeException(nameof(deltaTime), "Шаг времени должен быть больше 0.");

        _tick++;
        _renderer.BeginFrame(_world, _tick);
        _world.UpdateBehaviors(deltaTime);
        _world.UpdatePhysics(_physics, deltaTime);

        foreach (var o in _world.Objects)
            o.Render(_renderer, _world);

        _renderer.EndFrame(_world);
    }

    public void Run(int ticks, double deltaTime)
    {
        for (var i = 0; i < ticks; i++)
            Tick(deltaTime);
    }
}

public sealed class DemoRunner
{
    public void Run()
    {
        Console.WriteLine("Демо-стенд варианта 16: Аркада.");

        var world = new GameWorld(new Vector2(40, 20));
        world.Add(new Player("player-1", new Vector2(5, 5), new Vector2(1, 1)));
        world.Add(new Enemy("enemy-1", new Vector2(30, 10), new Vector2(1, 1), 4.0, new PatrolBehavior()));
        world.Add(new Platform("platform-1", new Vector2(15, 15), new Vector2(8, 1)));

        var physics = new SimplePhysicsModel();
        var loop = new GameLoop(world, physics, new ConsoleRenderer());
        var enemy = world.GetEnemy("enemy-1")
            ?? throw new InvalidOperationException("Объект enemy-1 отсутствует.");

        Console.WriteLine($"Начальное поведение enemy-1: {enemy.Behavior.Name}.");
        Console.WriteLine("Сценарий 1: ConsoleRenderer + PatrolBehavior, 3 тика.");
        loop.Run(3, 1.0);

        Console.WriteLine("Команда: смена поведения enemy-1 на ChaseBehavior.");
        enemy.SetBehavior(new ChaseBehavior());
        Console.WriteLine($"Состояние enemy-1: поведение изменено на {enemy.Behavior.Name}.");
        loop.Run(3, 1.0);

        Console.WriteLine("Команда: смена рендерера на PseudographicRenderer без изменения физики.");
        loop.SetRenderer(new PseudographicRenderer());
        loop.Run(3, 1.0);

        Console.WriteLine("Команда: смена поведения enemy-1 на IdleBehavior.");
        enemy.SetBehavior(new IdleBehavior());
        Console.WriteLine($"Состояние enemy-1: поведение изменено на {enemy.Behavior.Name}.");
        loop.Run(2, 1.0);

        Console.WriteLine("Демонстрация завершена.");
    }
}
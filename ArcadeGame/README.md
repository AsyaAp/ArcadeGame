Вариант 16. Аркада

Консольное приложение на C# (.NET 8) - мини-игра с игровыми объектами, физикой и отрисовкой. Демонстрирует независимое развитие физики и рендеринга и динамическую замену поведения врагов во время выполнения.

Скриншоты вывода программы

<img width="675" height="319" alt="1" src="https://github.com/user-attachments/assets/aafce817-ecbb-47ce-9ce1-af8305109d95" />
<img width="741" height="367" alt="2" src="https://github.com/user-attachments/assets/0619fc84-8697-4efd-8aac-0c371bde2ff6" />

Диаграмма классов
<img width="1080" height="1566" alt="diagram" src="https://github.com/user-attachments/assets/e91a1886-8f33-4bdb-8c07-1e774b41dc2a" />

Использованные паттерны

Bridge

Проблема: физика и рендеринг должны развиваться независимо. Наследование по обоим измерениям даёт N·M классов при N типах объектов и M реализациях, добавление реализации требует правки всех типов.
Решение: абстракция GameObject (наследники Player, Platform, Enemy) отделена от реализаций IRenderer(ConsoleRenderer, PseudographicRenderer) и IPhysicsModel (SimplePhysicsModel). Связь через параметры методов UpdatePhysics и Render. Добавление рендерера или физической модели не требует правки доменной модели.

Strategy

Проблема: поведение врага (преследование, патрулирование, ожидание) заменяется во время выполнения. Условные конструкции в Enemy требуют правки контекста при добавлении модели; наследование Enemy дублирует логику.
Решение: алгоритм вынесен в IEnemyBehavior; реализации - ChaseBehavior, PatrolBehavior, IdleBehavior. Контекст Enemy делегирует обновление текущей стратегии; замена - SetBehavior. Новая модель поведения - новый класс без правки Enemy.

Запуск

bash
dotnet run

using var engine = new LuaEngine();

engine.Expose("log", (Action<string>)Console.WriteLine);
engine.Expose("add", (Func<double, double, double>)((a, b) => a + b));
engine.Set("appName", "LuaHost");

engine.Run("main.lua");
engine.Call("start");

for (var i = 0; i < 3; i++)
{
    var result = engine.Call("update", 0.016);
    Console.WriteLine($"update -> {result.FirstOrDefault()}");
}

Console.WriteLine($"score = {engine.Get<long>("score")}");

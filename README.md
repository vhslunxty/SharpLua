# LuaHost

Moteur minimaliste pour exécuter des scripts Lua 5.4 dans une application C#, basé sur [NLua](https://github.com/NLua/NLua).

![build](https://github.com/OWNER/lua-host/actions/workflows/build.yml/badge.svg)

## Caractéristiques

- Un seul fichier à copier : `src/LuaEngine.cs`
- Scripts et fonctions mis en cache (pas de recompilation à chaque appel)
- Sandbox activé par défaut (`os`, `io`, `debug`, `package`, `require` désactivés)
- Rechargement à chaud d'un script
- Fonctions C# exposées à Lua via de simples delegates

## Démarrage

```bash
git clone https://github.com/OWNER/lua-host.git
cd lua-host/src
dotnet run
```

Prérequis : [.NET 8 SDK](https://dotnet.microsoft.com/download)

## Utilisation

```csharp
using var engine = new LuaEngine();

engine.Expose("log", (Action<string>)Console.WriteLine);
engine.Expose("add", (Func<double, double, double>)((a, b) => a + b));
engine.Set("appName", "MonApp");

engine.Run("main.lua");
engine.Call("start");

var result = engine.Call("update", 0.016);
var score = engine.Get<long>("score");
```

```lua
score = 0

function start()
    log("Démarrage de " .. appName)
end

function update(dt)
    score = score + 1
    return add(score, dt)
end
```

## API

| Méthode | Rôle |
|---|---|
| `Run(path)` | Charge et exécute un script (chunk mis en cache) |
| `Reload(path)` | Recompile et ré-exécute un script |
| `Eval(code)` | Exécute une chaîne Lua |
| `Call(name, args)` | Appelle une fonction Lua globale |
| `Expose(name, delegate)` | Expose une fonction C# à Lua |
| `Set(name, value)` | Définit une variable globale |
| `Get<T>(name)` | Lit une variable globale |

Les nombres entiers Lua sont retournés en `long`, les flottants en `double`.

Pour désactiver le sandbox : `new LuaEngine(sandbox: false)`.

## Licence

MIT

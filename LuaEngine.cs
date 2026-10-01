using System.Text;
using NLua;

public sealed class LuaEngine : IDisposable
{
    static readonly object[] NoArgs = [];

    readonly Lua lua = new();
    readonly Dictionary<string, LuaFunction> chunks = new();
    readonly Dictionary<string, LuaFunction> functions = new();

    public LuaEngine(bool sandbox = true)
    {
        lua.State.Encoding = Encoding.UTF8;
        if (sandbox)
            lua.DoString("os,io,debug,package,dofile,loadfile,require=nil,nil,nil,nil,nil,nil,nil");
    }

    public void Expose(string name, Delegate fn) => lua[name] = fn;

    public void Set(string name, object? value) => lua[name] = value;

    public T? Get<T>(string name) => lua[name] is T value ? value : default;

    public object[] Run(string path)
    {
        if (!chunks.TryGetValue(path, out var chunk))
            chunks[path] = chunk = lua.LoadFile(path);

        var result = chunk.Call();
        ClearFunctions();
        return result;
    }

    public object[] Eval(string code) => lua.DoString(code);

    public object[] Call(string name, params object[] args)
    {
        if (!functions.TryGetValue(name, out var fn))
        {
            fn = lua.GetFunction(name);
            if (fn is null) return NoArgs;
            functions[name] = fn;
        }
        return fn.Call(args);
    }

    public void Reload(string path)
    {
        if (chunks.Remove(path, out var old)) old.Dispose();
        Run(path);
    }

    void ClearFunctions()
    {
        foreach (var fn in functions.Values) fn.Dispose();
        functions.Clear();
    }

    public void Dispose()
    {
        ClearFunctions();
        foreach (var chunk in chunks.Values) chunk.Dispose();
        chunks.Clear();
        lua.Dispose();
    }
}

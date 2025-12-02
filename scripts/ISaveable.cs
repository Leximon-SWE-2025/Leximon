using Godot;
using System.Collections.Generic;
using System.Text.Json;

using LoadType = System.Collections.Generic.Dictionary<string, System.Text.Json.JsonElement>;

internal interface ISaveable
{
    public abstract Dictionary<string,object> Save();
    public abstract void Load(LoadType dict);
}

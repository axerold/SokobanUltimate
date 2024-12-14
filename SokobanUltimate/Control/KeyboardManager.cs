using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework.Input;

namespace SokobanUltimate.Control;

public class KeyboardManager
{
    public readonly string jsonPath = "Control/KeyBindings.json";
    public static Dictionary<Keys, Query> SingleQueries { get; private set; }

    public KeyboardManager()
    {
        DeserializeBindings();
    }

    public static List<Query> ObtainKeyboardQueries()
    {
        var keyboardState = Keyboard.GetState();

        var activeQueries = new List<Query>();

        foreach (var (key, query) in SingleQueries)
        {
            if (keyboardState.IsKeyDown(key))
            {
                activeQueries.Add(query);
            }
        }
        return ReduceKeyboardQueries(activeQueries);
    }

    private static List<Query> ReduceKeyboardQueries(List<Query> queries)
    {
        var sortedQueries = queries.OrderBy(q => q.Priority).ToList();
        return sortedQueries
            .GroupBy(q => q.Priority).FirstOrDefault(g => g.Count() == 1)?.ToList();
    }
    
    public void SerializeBindings()
    {
        var stringKeysDictionary = new Dictionary<string, Query>();
        foreach (var (key, query) in SingleQueries)
        {
            stringKeysDictionary[key.ToString()] = query;
        }

        File.WriteAllText(jsonPath, JsonSerializer.Serialize(stringKeysDictionary));
    }

    public void DeserializeBindings()
    {
        var stringKeysDictionary = 
            JsonSerializer.Deserialize<Dictionary<string, Query>>(File.ReadAllText(jsonPath));

        if (stringKeysDictionary is null)
            throw new IOException("json-file is empty or placed on different location");

        SingleQueries = new Dictionary<Keys, Query>();
        foreach (var (stringKey, query) in stringKeysDictionary)
        {
            if (Enum.TryParse(stringKey, out Keys key))
            {
                SingleQueries[key] = query;
            }
        }
    }
}
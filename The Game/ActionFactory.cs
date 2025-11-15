using System;
using System.Reflection;
using System.Transactions;

/// <summary>
/// Provides factory methods for creating instances of game actions based on their names.
/// </summary>
public static class ActionFactory
{
    private static readonly Dictionary<string, Type> _actionTypes;

    static ActionFactory()
    {
        _actionTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ActionObject)) && !t.IsAbstract)
            .ToDictionary(t => t.Name, t => t);
    }
    // Input is the triggerconfig
    public static ActionObject? Create(string actionName, Dictionary<string, object>? parameters = null)
    {
        if (!_actionTypes.TryGetValue(actionName, out var type))
        {
            Console.WriteLine($"[WARN] No GameAction found for '{actionName}'");
            return null;
        }

        // If no params, try default constructor
        if (parameters == null || parameters.Count == 0)
            return Activator.CreateInstance(type) as ActionObject;

        // Try to match constructor parameters
        foreach (var ctor in type.GetConstructors())
        {
            var ctorParams = ctor.GetParameters();

            var args = new object?[ctorParams.Length];
            bool match = true;

            for (int i = 0; i < ctorParams.Length; i++)
            {
                var p = ctorParams[i];
                if (parameters.TryGetValue(p.Name!, out var rawValue))
                {
                    try
                    {
                        args[i] = Convert.ChangeType(rawValue, p.ParameterType);
                    }
                    catch
                    {
                        match = false;
                        break;
                    }
                }
                else
                {
                    match = false;
                    break;
                }
            }

            if (match)
                return ctor.Invoke(args) as ActionObject;
        }

        Console.WriteLine($"[WARN] No matching constructor for '{actionName}' with given params");
        return null;
    }
}
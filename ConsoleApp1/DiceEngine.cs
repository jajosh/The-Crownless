using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using Windows.UI.ViewManagement;


/// <summary>
/// This class handles all things related to the dice
/// </summary>
public class DiceEngine
{
    Random random = new Random();
    StatusFlags flags = new StatusFlags();
    public DiceEngine() 
    {
        
    }
    /// <summary>
    /// Rolls dice with full control over size, number, modifiers, and advantage/disadvantage.
    /// </summary>
    /// <param name="size">Number of sides on the die (e.g. 6 for a d6).</param>
    /// <param name="numberOfDice">How many dice to roll.</param>
    /// <param name="modifier">A flat modifier applied to the result.</param>
    /// <param name="advantage">If true, uses the higher roll.</param>
    /// <param name="disadvantage">If true, uses the lower roll.</param>
    /// <returns>The final roll result.</returns>
    public int Roll(int size = 20, int numberOfDice = 1, int modifier = 0, bool advantage = false, bool disadvantage = false)
    {
        if (size <= 2)
            throw new Exception("Error: dice needs at least 2 sides");
        if (numberOfDice <= 0)
            throw new Exception("Error: Number of dice needs to be at least 1");

        int roll1 = 0;
        int roll2 = 0;
        for (int i = 0; i < numberOfDice; i++)
        {
            roll1 += random.Next(1, size + 1);
            roll2 += random.Next(1, size + 1);
        }
        if (advantage)
            return Math.Max(roll1, roll2);

        if (!advantage)
            return Math.Min(roll1, roll2);
        if (flags.IsDebug)
        {
            Console.WriteLine($"Rolled {numberOfDice}d{size}: {roll1}");
        }

        return roll1; // normal roll
    }
    #region === Convenience overloads ===

    /// <summary>
    /// Rolls a standard d20 with optional advantage/disadvantage.
    /// </summary>
    public int RollD20(bool advantage = false, bool disadvantage = false) =>
        Roll(20, 1, 0, advantage, disadvantage);

    /// <summary>
    /// Rolls a single die of the given size.
    /// </summary>
    public int RollSingle(int size) => Roll(size);

    /// <summary>
    /// Rolls multiple dice of the same size and adds them together.
    /// </summary>
    public int RollMultiple(int size, int numberOfDice) => Roll(size, numberOfDice);

    /// <summary>
    /// Rolls dice with a modifier.
    /// </summary>
    public int RollWithModifier(int size, int numberOfDice, int modifier) =>
        Roll(size, numberOfDice, modifier);
    #endregion
}

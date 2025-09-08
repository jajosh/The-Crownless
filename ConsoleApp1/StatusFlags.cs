using System;

public class StatusFlags
{
	public bool isRunning = true;

    public bool IsDebug { get; set; }
	public bool HasAdvantage { get; set; }
	public bool HasDisadvantage { get; set; }

	/// <summary>
	/// Controls console related content. Stored here to make saving this information to the player simpler
	/// </summary>
	public int ConsoleSpeed { get; set; } //Global typing speed
	public string ConsoleTextColor { get; set; } //global text color
	public string ConsoleBackgroundColor { get; set; } //global background color
	public bool IsDead { get; set; } //True if player is dead
	/// <summary>
	/// The following bools describe if the player is weak to something or in general
	/// </summary>
	public bool IsWeakened { get; set; }
	public bool IsWeakSlashing { get; set; }
	public bool IsWeakBludgning { get; set; }
	public bool IsWeakPiercing { get; set; }
	public bool IsWeakPosion { get; set; }
	public bool IsWeakFire { get; set; }

	/// <summary>
	/// initializes statusFlags and resets all flags to default values.
	/// </summary>
	public StatusFlags()
	{
		ConsoleSpeed = 100;
		ConsoleTextColor = "White";
		ConsoleBackgroundColor = "Black";
		ResetAllFlags();
	}

	public void ResetAllFlags()
	{
		foreach (var prop in this.GetType().GetProperties())
		{
			if (prop.PropertyType == typeof(bool) && prop.CanWrite)
				prop.SetValue(this, false);
		}
		ConsoleSpeed = 100;
	}
	public int Speed()
	{
		return ConsoleSpeed;
	}
}

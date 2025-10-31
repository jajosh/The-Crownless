using System;
public enum MagicSchool { }
public class Spell
{
	// Identity
	public int ID { get; set; }
	public string Name { get; set; }

	// Type
	public MagicSchool School { get; set; }
	public int Level { get; set; }

	public Spell()
	{

	}
}

using System;
using The_Game;

public class NPCTypeManager : INPCTypeEngine
{
	public static List<NPCTypeObject> NPCTypes { get; set; }
	public NPCTypeManager()
	{
        NPCTypes = LoadFromJson();
	}
	public List<NPCTypeObject> LoadFromJson()
	{
		List<NPCTypeObject> NPCTypes = JsonLoader.LoadFromJson<List<NPCTypeObject>>(FilePaths.NPCTypeFilePath);
		return NPCTypes;
	}
	public NPCObject GenerateNPC(NPCTypeObject type)
	{
		NPCObject npc = new NPCObject();
		return npc;
	}
}

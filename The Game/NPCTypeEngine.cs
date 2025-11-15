using System;

public interface INPCTypeEngine
{
	static abstract List<NPCTypeObject> NPCTypes { get; set; }
	NPCObject GenerateNPC(NPCTypeObject type);
	List<NPCTypeObject> LoadFromJson();
}

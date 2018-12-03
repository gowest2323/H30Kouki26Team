using System.Collections.Generic;
using System.Linq;
/// <summary>
/// タグのenum
/// </summary>
public enum TagName {
	Untagged,
	Respawn,
	Finish,
	EditorOnly,
	MainCamera,
	Player,
	GameController,
	Enemy,
	EnemyDeadArea,
}
public static class TagNameManager {
	public static Dictionary<TagName, string> tagnames = new Dictionary<TagName, string> {
		{TagName.Untagged, "Untagged"},
		{TagName.Respawn, "Respawn"},
		{TagName.Finish, "Finish"},
		{TagName.EditorOnly, "EditorOnly"},
		{TagName.MainCamera, "MainCamera"},
		{TagName.Player, "Player"},
		{TagName.GameController, "GameController"},
		{TagName.Enemy, "Enemy"},
		{TagName.EnemyDeadArea, "EnemyDeadArea"},
	};
	public static bool Equals(TagName tagname, string name) {
		return tagnames[tagname] == name;
	}
	public static bool Equals(string name, TagName tagname) {
		return name == tagnames[tagname];
	}
	public static bool Equals(string name1, string name2) {
		return name1 == name2;
	}
	public static bool Equals(TagName tagname1, TagName tagname2) {
		return tagname1 == tagname2;
	}
	public static string String(this TagName tagname) {
		return tagnames[tagname];
	}
	public static TagName GetKeyByValue(string name) {
		return tagnames.FirstOrDefault(pair => pair.Value == name).Key;
	}
}

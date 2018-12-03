using System.Collections.Generic;
using System.Linq;
/// <summary>
/// タグのenum
/// </summary>
public enum AudioName {
	bougyokamae,
	bougyouke,
	Club,
	Club_Short,
	Cut,
	Dash,
	Dash_Short,
	Dodge,
	Dodge_Short,
	Drawing,
	hajiki,
	Hipdrop,
	kaihi,
	kougeki,
	kougeki_1,
	kyusei,
	Lance,
	Lance2,
	Repel,
	Tackle,
	Walk,
	Walk_Short,
}
public static class AudioNameManager {
	public static Dictionary<AudioName, string> audionames = new Dictionary<AudioName, string> {
		{AudioName.bougyokamae, "bougyokamae"},
		{AudioName.bougyouke, "bougyouke"},
		{AudioName.Club, "Club"},
		{AudioName.Club_Short, "Club_Short"},
		{AudioName.Cut, "Cut"},
		{AudioName.Dash, "Dash"},
		{AudioName.Dash_Short, "Dash_Short"},
		{AudioName.Dodge, "Dodge"},
		{AudioName.Dodge_Short, "Dodge_Short"},
		{AudioName.Drawing, "Drawing"},
		{AudioName.hajiki, "hajiki"},
		{AudioName.Hipdrop, "Hipdrop"},
		{AudioName.kaihi, "kaihi"},
		{AudioName.kougeki, "kougeki"},
		{AudioName.kougeki_1, "kougeki_1"},
		{AudioName.kyusei, "kyusei"},
		{AudioName.Lance, "Lance"},
		{AudioName.Lance2, "Lance2"},
		{AudioName.Repel, "Repel"},
		{AudioName.Tackle, "Tackle"},
		{AudioName.Walk, "Walk"},
		{AudioName.Walk_Short, "Walk_Short"},
	};
	public static bool Equals(AudioName audioname, string name) {
		return audionames[audioname] == name;
	}
	public static bool Equals(string name, AudioName audioname) {
		return name == audionames[audioname];
	}
	public static bool Equals(string name1, string name2) {
		return name1 == name2;
	}
	public static bool Equals(AudioName audioname1, AudioName audioname2) {
		return audioname1 == audioname2;
	}
	public static string String(this AudioName audioname) {
		return audionames[audioname];
	}
	public static AudioName GetKeyByValue(string name) {
		return audionames.FirstOrDefault(pair => pair.Value == name).Key;
	}
}

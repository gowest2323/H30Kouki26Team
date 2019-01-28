using System.Collections.Generic;
using System.Linq;
/// <summary>
/// タグのenum
/// </summary>
public enum AudioName
{
horror_zone2,
bougyokamae,
bougyouke,
Club,
Club_Short,
Cut,
CutHit,
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
kaede_ha_attack_01,
kaede_ha_attack_02,
kaede_ha_attack_03,
kaede_ha_attack_04,
kaede_aa_die_12,
kaede_aaa_damage_07,
kaede_ha_hajiki_09,
kaede_hun_kyusei_10,
kaede_ku_guard_05,
kaede_kurae_kyusei_11,
kaede_uu_damage_06,
kaede_ya_08,
op_kaede_korosa_17_05,
op_kaede_omaewo_21_09,
op_kaede_onini_14_02,
op_kaede_oniwo_19_07,
op_kaede_ooku_15_03,
op_kaede_turesa_16_04,
op_kaede_watasi_13_01,
op_kaede_watasi_18_06,
op_kaede_yurusanai_20_08,
oni_aa_kyusei_06,
oni_aa_kyusei_take2_06,
oni_aa_taore_05,
oni_oaa_preAttack_03,
oni_oaa_sakebi_02,
oni_oaa_sakebi_take2_02,
oni_uu_damage_04,
oni_uu_damage_take2_04,
oni_uu_idle_01,
setuna_ha_niren_01,
setuna_ha_niren_take2_01,
setuna_hu_sanren_02,
setuna_hu_sanren_take2_02,
setuna_oaa_kyodai_04,
setuna_daga_talk_06,
setuna_daga_talk_take2_06,
setuna_naruhodo_talk_05_01,
setuna_oroka_talk_07,
Walk,
Walk_Short,
}
public static class AudioNameManager
{
    public static Dictionary<AudioName, string> audionames = new Dictionary<AudioName, string> 
{
    {AudioName.horror_zone2,"horror_zone2"},
    {AudioName.bougyokamae,"bougyokamae"},
    {AudioName.bougyouke,"bougyouke"},
    {AudioName.Club,"Club"},
    {AudioName.Club_Short,"Club_Short"},
    {AudioName.Cut,"Cut"},
    {AudioName.CutHit,"CutHit"},
    {AudioName.Dash,"Dash"},
    {AudioName.Dash_Short,"Dash_Short"},
    {AudioName.Dodge,"Dodge"},
    {AudioName.Dodge_Short,"Dodge_Short"},
    {AudioName.Drawing,"Drawing"},
    {AudioName.hajiki,"hajiki"},
    {AudioName.Hipdrop,"Hipdrop"},
    {AudioName.kaihi,"kaihi"},
    {AudioName.kougeki,"kougeki"},
    {AudioName.kougeki_1,"kougeki_1"},
    {AudioName.kyusei,"kyusei"},
    {AudioName.Lance,"Lance"},
    {AudioName.Lance2,"Lance2"},
    {AudioName.Repel,"Repel"},
    {AudioName.Tackle,"Tackle"},
    {AudioName.kaede_ha_attack_01,"kaede_ha_attack_01"},
    {AudioName.kaede_ha_attack_02,"kaede_ha_attack_02"},
    {AudioName.kaede_ha_attack_03,"kaede_ha_attack_03"},
    {AudioName.kaede_ha_attack_04,"kaede_ha_attack_04"},
    {AudioName.kaede_aa_die_12,"kaede_aa_die_12"},
    {AudioName.kaede_aaa_damage_07,"kaede_aaa_damage_07"},
    {AudioName.kaede_ha_hajiki_09,"kaede_ha_hajiki_09"},
    {AudioName.kaede_hun_kyusei_10,"kaede_hun_kyusei_10"},
    {AudioName.kaede_ku_guard_05,"kaede_ku_guard_05"},
    {AudioName.kaede_kurae_kyusei_11,"kaede_kurae_kyusei_11"},
    {AudioName.kaede_uu_damage_06,"kaede_uu_damage_06"},
    {AudioName.kaede_ya_08,"kaede_ya_08"},
    {AudioName.op_kaede_korosa_17_05,"op_kaede_korosa_17_05"},
    {AudioName.op_kaede_omaewo_21_09,"op_kaede_omaewo_21_09"},
    {AudioName.op_kaede_onini_14_02,"op_kaede_onini_14_02"},
    {AudioName.op_kaede_oniwo_19_07,"op_kaede_oniwo_19_07"},
    {AudioName.op_kaede_ooku_15_03,"op_kaede_ooku_15_03"},
    {AudioName.op_kaede_turesa_16_04,"op_kaede_turesa_16_04"},
    {AudioName.op_kaede_watasi_13_01,"op_kaede_watasi_13_01"},
    {AudioName.op_kaede_watasi_18_06,"op_kaede_watasi_18_06"},
    {AudioName.op_kaede_yurusanai_20_08,"op_kaede_yurusanai_20_08"},
    {AudioName.oni_aa_kyusei_06,"oni_aa_kyusei_06"},
    {AudioName.oni_aa_kyusei_take2_06,"oni_aa_kyusei_take2_06"},
    {AudioName.oni_aa_taore_05,"oni_aa_taore_05"},
    {AudioName.oni_oaa_preAttack_03,"oni_oaa_preAttack_03"},
    {AudioName.oni_oaa_sakebi_02,"oni_oaa_sakebi_02"},
    {AudioName.oni_oaa_sakebi_take2_02,"oni_oaa_sakebi_take2_02"},
    {AudioName.oni_uu_damage_04,"oni_uu_damage_04"},
    {AudioName.oni_uu_damage_take2_04,"oni_uu_damage_take2_04"},
    {AudioName.oni_uu_idle_01,"oni_uu_idle_01"},
    {AudioName.setuna_ha_niren_01,"setuna_ha_niren_01"},
    {AudioName.setuna_ha_niren_take2_01,"setuna_ha_niren_take2_01"},
    {AudioName.setuna_hu_sanren_02,"setuna_hu_sanren_02"},
    {AudioName.setuna_hu_sanren_take2_02,"setuna_hu_sanren_take2_02"},
    {AudioName.setuna_oaa_kyodai_04,"setuna_oaa_kyodai_04"},
    {AudioName.setuna_daga_talk_06,"setuna_daga_talk_06"},
    {AudioName.setuna_daga_talk_take2_06,"setuna_daga_talk_take2_06"},
    {AudioName.setuna_naruhodo_talk_05_01,"setuna_naruhodo_talk_05_01"},
    {AudioName.setuna_oroka_talk_07,"setuna_oroka_talk_07"},
    {AudioName.Walk,"Walk"},
    {AudioName.Walk_Short,"Walk_Short"},
};
    public static bool Equals(AudioName audioname, string name)
    {
        return audionames[audioname] == name;
    }
    public static bool Equals(string name, AudioName audioname)
    {
        return name == audionames[audioname];
    }
    public static bool Equals(string name1, string name2)
    {
        return name1 == name2;
    }
    public static bool Equals(AudioName audioname1, AudioName audioname2)
    {
        return audioname1 == audioname2;
    }
    public static string String(this AudioName audioname)
    {
        return audionames[audioname];
    }
    public static AudioName GetKeyByValue(string name)
    {
        return audionames.FirstOrDefault(pair => pair.Value == name).Key;
    }
}

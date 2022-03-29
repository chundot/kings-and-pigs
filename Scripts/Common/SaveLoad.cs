using System.Collections.Generic;
using Godot;
using kingsandpigs.Scripts.Common;

public class SaveLoad
{
    public static void Save()
    {
        var save = new File();
        save.Open("user://game.save", File.ModeFlags.Write);
        var dic = new Godot.Collections.Dictionary<string, int> {
            {"Unlocked", GlobalVar.UnlockedLevel},
            {"CurLevel", GlobalVar.CurLevel}
         };
        save.StoreLine(JSON.Print(dic));
        save.Close();
    }

    public static void Load()
    {
        var save = new File();
        if (!save.FileExists("user://game.save"))
            return;
        save.Open("user://game.save", File.ModeFlags.Read);
        var dic = new Godot.Collections.Dictionary<string, int>((Godot.Collections.Dictionary)JSON.Parse(save.GetLine()).Result);
        GlobalVar.UnlockedLevel = dic["Unlocked"];
        GlobalVar.CurLevel = dic["CurLevel"];
        save.Close();
    }
}
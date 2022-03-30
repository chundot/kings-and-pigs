using System;
using System.Collections.Generic;
using System.Text.Json;
using Godot;
using kingsandpigs.Scripts.Common;

public class SaveLoad
{
    public static void Save()
    {
        var save = new File();
        save.Open("user://game.save", File.ModeFlags.Write);
        var dic = new Dictionary<string, int> {
            {"Unlocked", GlobalVar.UnlockedLevel},
            {"CurLevel", GlobalVar.CurLevel}
         };
        save.StoreLine(JsonSerializer.Serialize(dic));
        save.Close();
    }

    public static void Load()
    {
        var save = new File();
        if (!save.FileExists("user://game.save"))
            return;
        save.Open("user://game.save", File.ModeFlags.Read);
        try
        {
            var dic = JsonSerializer.Deserialize<Dictionary<string, int>>(save.GetLine());
            GlobalVar.UnlockedLevel = dic["Unlocked"];
            GlobalVar.CurLevel = dic["CurLevel"];
        }
        catch (Exception)
        {
            new Directory().Remove("user://game.save");
        }
        save.Close();
    }
}
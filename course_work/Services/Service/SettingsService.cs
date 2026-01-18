using System.IO;
using System.Text.Json;
using course_work.Models;

namespace course_work.Services.Service;

public class SettingsService
{
    private const string FileName = "appsettings.json";

    public AppSettings Settings { get; private set; }

    public SettingsService()
    {
        Load();
    }

    public void Load()
    {
        if (File.Exists(FileName))
        {
            var json = File.ReadAllText(FileName);
            Settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        else
        {
            Settings = new AppSettings();
        }
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FileName, json);
    }
}
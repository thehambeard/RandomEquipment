using System.IO;
using System.Net.Http;
using static ModMenu.ModMenu;
using static WrathRandomEquipment.REModMenu.RandomEquipmentSettings;

namespace WrathRandomEquipment.RandomEquipment
{
    internal class LootListDownloader
    {
        public static readonly string path = @"https://raw.githubusercontent.com/thehambeard/RandomEquipment/main/LootList.json";

        public static async void Download(bool force = false)
        {
            if (GetSettingValue<bool>(GetKey("bool-autoupdate")) || force)
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(path))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            Main.Mod.Log("Downloaded lastest LootList.json");
                            var localPath = Path.Combine(Main.ModPath, "LootList.json");
                            using var fileStream = new FileStream(localPath, FileMode.Create);
                            await response.Content.CopyToAsync(fileStream);
                        }
                    }
                }
            }
        }
    }
}

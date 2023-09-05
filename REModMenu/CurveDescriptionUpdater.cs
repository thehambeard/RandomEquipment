using Kingmaker.UI.MVVM._VM.Settings.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ModMenu.Helpers;

namespace WrathRandomEquipment.REModMenu
{
    internal class CurveDescriptionUpdater
    {
        private readonly static string _pathMainUi = "Canvas/SettingsView/ContentWrapper/VirtualListVertical/Viewport/Content";
        private readonly static string _pathDescriptionUi = "Canvas/SettingsView/ContentWrapper/DescriptionView";
        public static void Update(Dictionary<int, double> distribution, string title, string original)

        {
            var text = new StringBuilder().Append(original).Append(BuildTable(distribution)).ToString();
            SettingsDescriptionUpdater<SettingsEntitySliderVM> sdu = new(_pathMainUi, _pathDescriptionUi);
            sdu.TryUpdate(title, text);
        }


        private static string BuildTable(Dictionary<int, double> distribution)
        {
            var sb = new StringBuilder();
            var min = distribution.Keys.Min<int>();
            var max = distribution.Keys.Max<int>();

            sb.Append("\n\nPercent chance for each modifier (approx):\n\n");

            for (int i = min; i <= max; i++)
            {
                sb.Append($"<b>{i}</b>: {distribution[i]:00}%\n");
            }
            return sb.ToString();
        }
    }
}

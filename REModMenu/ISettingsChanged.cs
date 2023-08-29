using Kingmaker.PubSubSystem;

namespace WrathRandomEquipment.REModMenu
{
    public interface ISettingsChanged : ISubscriber, IGlobalSubscriber
    {
        void HandleSettingsChanged();
    }
}

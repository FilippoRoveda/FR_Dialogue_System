
namespace Game
{
    using DS.Runtime.Enumerations;
    public interface IInterface
    {
        void Show();
        void Hide();
        void ResetInterface();
        void OnLenguageChanged(LenguageType newLenguage);
    }
}

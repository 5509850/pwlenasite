namespace Pw.Lena.Slave.Droid.Screens.Contracts
{
    interface IContentFragment
    {
        int TitleResourceId { get; }

        void Cleanup();
    }
}
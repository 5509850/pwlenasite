using Android.Content;
using Android.Views;
using System.Threading.Tasks;

namespace Pw.Lena.Slave.Droid.Screens.Contracts
{
    public interface IFragmentContainer
    {
        void ChangeCurrentFragment<T>() where T : Android.Support.V4.App.Fragment, new();

        Task ShowSaveSpendingInfoAsync(Intent data, View view);
    }
}
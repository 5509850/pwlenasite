
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content.PM;
using GalaSoft.MvvmLight.Helpers;
using pw.lena.Core.Business.ViewModels.Slave;
using System.Collections.Generic;
using Pw.Lena.Slave.Droid.UI.ViewHolders;
using Android.Views.Animations;

namespace Pw.Lena.Slave.Droid.Screens
{
    [Activity(
         //    Label = "@string/guard_login_title",
         Theme = "@style/AppTheme.Pin",
         NoHistory = true,
         WindowSoftInputMode = SoftInput.StateHidden | SoftInput.AdjustResize,
         ScreenOrientation = ScreenOrientation.Portrait)]
    public class GuardLoginActivity : BaseActivity<EntryViewModel>
    {
        //GalaSoft.MvvmLight.Helpers.Binding
        private readonly List<Binding> bindings = new List<Binding>();

        protected override int LayoutResource { get; } = Resource.Layout.activity_guard_login;

        private ActivityGuardLoginViewHolder ViewHolder { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ViewHolder = new ActivityGuardLoginViewHolder(this);
            ViewModel = App.ViewModelLocator.CreateEntryViewModel();

            Toolbar.Visibility = ViewStates.Visible;
            //  Toolbar.SetTitle(Resource.String.guard_login_title);
            SetToolbarTitle("");
            SetBindings();
        }

        public string ToolbarTitle
        {
            get
            {
                return GetToolbarTitle();
            }
            set
            {
                SetToolbarTitle(value);
            }
        }

        private void SetBindings()
        {
            bindings.Add(
                this.SetBinding(() => ViewModel.ErrorMessage, () => ViewHolder.LayoutErrorContainer.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(message => !string.IsNullOrEmpty(message) ? ViewStates.Visible : ViewStates.Invisible));

            bindings.Add(
                this.SetBinding(() => ViewModel.ErrorMessage, () => ViewHolder.TextError.Text, BindingMode.OneWay));

            ViewHolder.BtnOne.SetCommand("Click", ViewModel.SetDigitCommand, this.SetBinding(() => ViewHolder.BtnOne.Text, BindingMode.OneWay));
            ViewHolder.BtnTwo.SetCommand("Click", ViewModel.SetDigitCommand, this.SetBinding(() => ViewHolder.BtnTwo.Text, BindingMode.OneWay));
            ViewHolder.BtnThree.SetCommand("Click", ViewModel.SetDigitCommand, this.SetBinding(() => ViewHolder.BtnThree.Text, BindingMode.OneWay));
            ViewHolder.BtnFour.SetCommand("Click", ViewModel.SetDigitCommand, this.SetBinding(() => ViewHolder.BtnFour.Text, BindingMode.OneWay));
            ViewHolder.BtnFive.SetCommand("Click", ViewModel.SetDigitCommand, this.SetBinding(() => ViewHolder.BtnFive.Text, BindingMode.OneWay));
            ViewHolder.BtnSix.SetCommand("Click", ViewModel.SetDigitCommand, this.SetBinding(() => ViewHolder.BtnSix.Text, BindingMode.OneWay));
            ViewHolder.BtnSeven.SetCommand("Click", ViewModel.SetDigitCommand, this.SetBinding(() => ViewHolder.BtnSeven.Text, BindingMode.OneWay));
            ViewHolder.BtnEight.SetCommand("Click", ViewModel.SetDigitCommand, this.SetBinding(() => ViewHolder.BtnEight.Text, BindingMode.OneWay));
            ViewHolder.BtnNine.SetCommand("Click", ViewModel.SetDigitCommand, this.SetBinding(() => ViewHolder.BtnNine.Text, BindingMode.OneWay));
            ViewHolder.BtnZero.SetCommand("Click", ViewModel.SetDigitCommand, this.SetBinding(() => ViewHolder.BtnZero.Text, BindingMode.OneWay));

            ViewHolder.BtnBack.SetCommand("Click", ViewModel.BackSpaceCommand);
            ViewHolder.BtnClear.SetCommand("Click", ViewModel.ClearPinCommand);

            bindings.Add(
                this.SetBinding(() => ViewModel.TitleText, () => ToolbarTitle, BindingMode.OneWay));

            bindings.Add(
                this.SetBinding(() => ViewModel.IsLoggingIn, BindingMode.OneWay)
                    .WhenSourceChanges(
                        () =>
                        {
                            if (ViewModel.IsLoggingIn)
                            {
                                StartActivity(typeof(LandingActivity));
                            }
                        }));

            bindings.Add(
               this.SetBinding(() => ViewModel.Pin, BindingMode.OneWay)
                   .WhenSourceChanges(
                       () =>
                       {
                           if (ViewModel.Pin != null)
                           {
                               switch (ViewModel.Pin.Length)
                               {
                                   case 0:
                                       {
                                           ViewHolder.P1.Enabled = true;
                                           ViewHolder.P2.Enabled =
                                           ViewHolder.P3.Enabled =
                                           ViewHolder.P4.Enabled = false;
                                           ViewHolder.P1.Text =
                                           ViewHolder.P2.Text =
                                           ViewHolder.P3.Text =
                                           ViewHolder.P4.Text = string.Empty;
                                           break;
                                       }
                                   case 1:
                                       {
                                           ViewHolder.P1.Enabled = false;
                                           ViewHolder.P2.Enabled = true;
                                           ViewHolder.P3.Enabled =
                                           ViewHolder.P4.Enabled = false;
                                           ViewHolder.P1.Text = "x";
                                           ViewHolder.P2.Text =
                                           ViewHolder.P3.Text =
                                           ViewHolder.P4.Text = string.Empty;
                                           break;
                                       }
                                   case 2:
                                       {
                                           ViewHolder.P1.Enabled =
                                           ViewHolder.P2.Enabled = false;
                                           ViewHolder.P3.Enabled = true;
                                           ViewHolder.P4.Enabled = false;
                                           ViewHolder.P1.Text =
                                           ViewHolder.P2.Text = "x";
                                           ViewHolder.P3.Text =
                                           ViewHolder.P4.Text = string.Empty;
                                           break;
                                       }
                                   case 3:
                                       {
                                           ViewHolder.P1.Enabled =
                                           ViewHolder.P2.Enabled =
                                           ViewHolder.P3.Enabled = false;
                                           ViewHolder.P4.Enabled = true;
                                           ViewHolder.P1.Text =
                                           ViewHolder.P2.Text =
                                           ViewHolder.P3.Text = "x";
                                           ViewHolder.P4.Text = string.Empty;
                                           break;
                                       }
                                   case 4:
                                       {
                                           Shake();                                           
                                           ViewHolder.P1.Enabled =
                                           ViewHolder.P2.Enabled =
                                           ViewHolder.P3.Enabled = false;
                                           ViewHolder.P4.Enabled = true;
                                           ViewHolder.P1.Text =
                                           ViewHolder.P2.Text =
                                           ViewHolder.P3.Text =
                                           ViewHolder.P4.Text = "x";
                                           break;
                                       }
                               }
                           }
                       }));           
        }
        

        private void Shake()
        {
            //https://developer.xamarin.com/guides/android/application_fundamentals/graphics_and_animation/       
            Animation fade = AnimationUtils.LoadAnimation(ApplicationContext,
                           Resource.Animation.fade_in);

            Animation rotate = AnimationUtils.LoadAnimation(ApplicationContext,
                         Resource.Animation.rotate);

            Animation shake = AnimationUtils.LoadAnimation(ApplicationContext,
                       Resource.Animation.shake);

            ViewHolder.Pinscreen.StartAnimation(shake);
            ViewHolder.Error.StartAnimation(rotate);
            ViewHolder.Keyboard.StartAnimation(fade);           

            
        }
    }
}
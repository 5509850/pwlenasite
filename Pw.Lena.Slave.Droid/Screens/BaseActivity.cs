
using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Pw.Lena.Slave.Droid.UI.ViewHolders;

namespace Pw.Lena.Slave.Droid.Screens
{
    public abstract class BaseActivity : AppCompatActivity
    {      

        public static BaseActivity CurrentActivity { get; private set; }

        protected abstract int LayoutResource { get; }

        protected virtual bool ShowToolbarHomeButton { get; } = true;

        private ToolbarViewHolder viewHolder;

        protected Toolbar Toolbar => viewHolder.Toolbar;

        protected ProgressDialog progressDialog = null;

        public Android.Widget.ImageView iconFirstStep => viewHolder.IconFirstStep;

        public Android.Widget.ImageView iconSecondStep => viewHolder.IconSecondStep;

        #region Activity lifecycle

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            viewHolder = new ToolbarViewHolder(this);

            SetContentView(LayoutResource);
            ConfigureToolbar();
        }

        protected override void OnResume()
        {
            CurrentActivity = this;

            base.OnResume();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();

                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion

        #region Helpers

        protected void SetToolbarTitle(int titleResourceId)
        {
            SetToolbarTitle(Resources.GetString(titleResourceId));
        }

        protected void SetToolbarTitle(string title)
        {
            viewHolder.TitleTextView.Text = title;
        }

        protected virtual string GetToolbarTitle()
        {
            return viewHolder.TitleTextView.Text;
        }

        protected void Alert(string message)
        {
            RunOnUiThread(delegate
            {
                Android.Widget.Toast.MakeText(this, message, Android.Widget.ToastLength.Short).Show();
            });
        }

        private void ConfigureToolbar()
        {
            SetSupportActionBar(viewHolder.Toolbar);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(ShowToolbarHomeButton);

            //if (!ShowToolbarHomeButton)
            //{
            //    ((ViewGroup.MarginLayoutParams)viewHolder.TitleTextView.LayoutParameters).LeftMargin =
            //        (int)Resources.GetDimension(Resource.Dimension.content_horizontal_indent);
            //}

            SetToolbarTitle(SupportActionBar.Title);

            SupportActionBar.SetDisplayShowTitleEnabled(false);
        }

        #endregion

        #region progress

        protected void ShowProgress(int resId)
        {
            ShowProgress(GetText(resId));
        }

        protected void ShowProgress(string text)
        {
            progressDialog = new ProgressDialog(this);
            progressDialog.SetMessage(text);
            progressDialog.SetCancelable(false);
            progressDialog.Indeterminate = true;
            progressDialog.Show();
        }

        protected void HideProgress()
        {
            if (progressDialog != null)
            {
                progressDialog.Dismiss();
                progressDialog = null;
            }
        }
        #endregion

    }
}
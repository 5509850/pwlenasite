using Android.App;
using Android.Views;
using Android.Widget;

namespace Pw.Lena.Slave.Droid.UI.ViewHolders
{
	public class ActivityGuardLoginViewHolder : Java.Lang.Object
	{
		private readonly View view;

		private Android.Support.V7.Widget.Toolbar toolbar;
		private LinearLayout pinscreen;
		private Pw.Lena.Slave.Droid.UI.TextView p1;
		private Pw.Lena.Slave.Droid.UI.TextView p2;
		private Pw.Lena.Slave.Droid.UI.TextView p3;
		private Pw.Lena.Slave.Droid.UI.TextView p4;
		private LinearLayout error;
		private RelativeLayout layout_error_container;
		private ImageView image_error_status;
		private Pw.Lena.Slave.Droid.UI.TextView text_error;
		private LinearLayout keyboard;
		private Pw.Lena.Slave.Droid.UI.Button btn_seven;
		private Pw.Lena.Slave.Droid.UI.Button btn_eight;
		private Pw.Lena.Slave.Droid.UI.Button btn_nine;
		private Pw.Lena.Slave.Droid.UI.Button btn_four;
		private Pw.Lena.Slave.Droid.UI.Button btn_five;
		private Pw.Lena.Slave.Droid.UI.Button btn_six;
		private Pw.Lena.Slave.Droid.UI.Button btn_one;
		private Pw.Lena.Slave.Droid.UI.Button btn_two;
		private Pw.Lena.Slave.Droid.UI.Button btn_three;
		private Pw.Lena.Slave.Droid.UI.Button btn_clear;
		private Pw.Lena.Slave.Droid.UI.Button btn_zero;
		private Pw.Lena.Slave.Droid.UI.Button btn_back;

		public ActivityGuardLoginViewHolder(View view)
		{
			this.view = view;
		}

		public ActivityGuardLoginViewHolder(Activity activity)
		{
			view = activity.FindViewById(global::Android.Resource.Id.Content);
		}

		public Android.Support.V7.Widget.Toolbar Toolbar
		{
			get { return toolbar ?? (toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar)); }
		}

		public LinearLayout Pinscreen
		{
			get { return pinscreen ?? (pinscreen = view.FindViewById<LinearLayout>(Resource.Id.pinscreen)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView P1
		{
			get { return p1 ?? (p1 = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.p1)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView P2
		{
			get { return p2 ?? (p2 = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.p2)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView P3
		{
			get { return p3 ?? (p3 = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.p3)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView P4
		{
			get { return p4 ?? (p4 = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.p4)); }
		}

		public LinearLayout Error
		{
			get { return error ?? (error = view.FindViewById<LinearLayout>(Resource.Id.error)); }
		}

		public RelativeLayout LayoutErrorContainer
		{
			get { return layout_error_container ?? (layout_error_container = view.FindViewById<RelativeLayout>(Resource.Id.layout_error_container)); }
		}

		public ImageView ImageErrorStatus
		{
			get { return image_error_status ?? (image_error_status = view.FindViewById<ImageView>(Resource.Id.image_error_status)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView TextError
		{
			get { return text_error ?? (text_error = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.text_error)); }
		}

		public LinearLayout Keyboard
		{
			get { return keyboard ?? (keyboard = view.FindViewById<LinearLayout>(Resource.Id.keyboard)); }
		}

		public Pw.Lena.Slave.Droid.UI.Button BtnSeven
		{
			get { return btn_seven ?? (btn_seven = view.FindViewById<Pw.Lena.Slave.Droid.UI.Button>(Resource.Id.btn_seven)); }
		}

		public Pw.Lena.Slave.Droid.UI.Button BtnEight
		{
			get { return btn_eight ?? (btn_eight = view.FindViewById<Pw.Lena.Slave.Droid.UI.Button>(Resource.Id.btn_eight)); }
		}

		public Pw.Lena.Slave.Droid.UI.Button BtnNine
		{
			get { return btn_nine ?? (btn_nine = view.FindViewById<Pw.Lena.Slave.Droid.UI.Button>(Resource.Id.btn_nine)); }
		}

		public Pw.Lena.Slave.Droid.UI.Button BtnFour
		{
			get { return btn_four ?? (btn_four = view.FindViewById<Pw.Lena.Slave.Droid.UI.Button>(Resource.Id.btn_four)); }
		}

		public Pw.Lena.Slave.Droid.UI.Button BtnFive
		{
			get { return btn_five ?? (btn_five = view.FindViewById<Pw.Lena.Slave.Droid.UI.Button>(Resource.Id.btn_five)); }
		}

		public Pw.Lena.Slave.Droid.UI.Button BtnSix
		{
			get { return btn_six ?? (btn_six = view.FindViewById<Pw.Lena.Slave.Droid.UI.Button>(Resource.Id.btn_six)); }
		}

		public Pw.Lena.Slave.Droid.UI.Button BtnOne
		{
			get { return btn_one ?? (btn_one = view.FindViewById<Pw.Lena.Slave.Droid.UI.Button>(Resource.Id.btn_one)); }
		}

		public Pw.Lena.Slave.Droid.UI.Button BtnTwo
		{
			get { return btn_two ?? (btn_two = view.FindViewById<Pw.Lena.Slave.Droid.UI.Button>(Resource.Id.btn_two)); }
		}

		public Pw.Lena.Slave.Droid.UI.Button BtnThree
		{
			get { return btn_three ?? (btn_three = view.FindViewById<Pw.Lena.Slave.Droid.UI.Button>(Resource.Id.btn_three)); }
		}

		public Pw.Lena.Slave.Droid.UI.Button BtnClear
		{
			get { return btn_clear ?? (btn_clear = view.FindViewById<Pw.Lena.Slave.Droid.UI.Button>(Resource.Id.btn_clear)); }
		}

		public Pw.Lena.Slave.Droid.UI.Button BtnZero
		{
			get { return btn_zero ?? (btn_zero = view.FindViewById<Pw.Lena.Slave.Droid.UI.Button>(Resource.Id.btn_zero)); }
		}

		public Pw.Lena.Slave.Droid.UI.Button BtnBack
		{
			get { return btn_back ?? (btn_back = view.FindViewById<Pw.Lena.Slave.Droid.UI.Button>(Resource.Id.btn_back)); }
		}
	}

	public class ActivityLandingViewHolder : Java.Lang.Object
	{
		private readonly View view;

		private Android.Support.V4.Widget.DrawerLayout drawerLayout;
		private RelativeLayout activityContainer;
		private Android.Support.V7.Widget.Toolbar toolbar;
		private FrameLayout fragmentContainer;
		private LinearLayout appMenu;
		private ListView appMenuListView;
		private Pw.Lena.Slave.Droid.UI.TextView appMenuSyncMessage;

		public ActivityLandingViewHolder(View view)
		{
			this.view = view;
		}

		public ActivityLandingViewHolder(Activity activity)
		{
			view = activity.FindViewById(global::Android.Resource.Id.Content);
		}

		public Android.Support.V4.Widget.DrawerLayout DrawerLayout
		{
			get { return drawerLayout ?? (drawerLayout = view.FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawerLayout)); }
		}

		public RelativeLayout ActivityContainer
		{
			get { return activityContainer ?? (activityContainer = view.FindViewById<RelativeLayout>(Resource.Id.activityContainer)); }
		}

		public Android.Support.V7.Widget.Toolbar Toolbar
		{
			get { return toolbar ?? (toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar)); }
		}

		public FrameLayout FragmentContainer
		{
			get { return fragmentContainer ?? (fragmentContainer = view.FindViewById<FrameLayout>(Resource.Id.fragmentContainer)); }
		}

		public LinearLayout AppMenu
		{
			get { return appMenu ?? (appMenu = view.FindViewById<LinearLayout>(Resource.Id.appMenu)); }
		}

		public ListView AppMenuListView
		{
			get { return appMenuListView ?? (appMenuListView = view.FindViewById<ListView>(Resource.Id.appMenuListView)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView AppMenuSyncMessage
		{
			get { return appMenuSyncMessage ?? (appMenuSyncMessage = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.appMenuSyncMessage)); }
		}
	}

	public class ActivityTrackerViewHolder : Java.Lang.Object
	{
		private readonly View view;

		private Android.Support.V7.Widget.Toolbar toolbar;
		private Pw.Lena.Slave.Droid.UI.TextView txtLocation;
		private Pw.Lena.Slave.Droid.UI.TextView textView1;
		private Pw.Lena.Slave.Droid.UI.TextView txtAddress;
		private Pw.Lena.Slave.Droid.UI.TextView textView2;
		private Pw.Lena.Slave.Droid.UI.TextView txtRemarks;
		private Pw.Lena.Slave.Droid.UI.TextView textView3;

		public ActivityTrackerViewHolder(View view)
		{
			this.view = view;
		}

		public ActivityTrackerViewHolder(Activity activity)
		{
			view = activity.FindViewById(global::Android.Resource.Id.Content);
		}

		public Android.Support.V7.Widget.Toolbar Toolbar
		{
			get { return toolbar ?? (toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView TxtLocation
		{
			get { return txtLocation ?? (txtLocation = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.txtLocation)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView TextView1
		{
			get { return textView1 ?? (textView1 = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.textView1)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView TxtAddress
		{
			get { return txtAddress ?? (txtAddress = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.txtAddress)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView TextView2
		{
			get { return textView2 ?? (textView2 = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.textView2)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView TxtRemarks
		{
			get { return txtRemarks ?? (txtRemarks = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.txtRemarks)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView TextView3
		{
			get { return textView3 ?? (textView3 = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.textView3)); }
		}
	}

	public class AppMenuItemViewHolder : Java.Lang.Object
	{
		private readonly View view;

		private Pw.Lena.Slave.Droid.UI.TextView appMenuItemText;
		private ImageButton appMenuItemButton;

		public AppMenuItemViewHolder(View view)
		{
			this.view = view;
		}

		public AppMenuItemViewHolder(Activity activity)
		{
			view = activity.FindViewById(global::Android.Resource.Id.Content);
		}

		public Pw.Lena.Slave.Droid.UI.TextView AppMenuItemText
		{
			get { return appMenuItemText ?? (appMenuItemText = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.appMenuItemText)); }
		}

		public ImageButton AppMenuItemButton
		{
			get { return appMenuItemButton ?? (appMenuItemButton = view.FindViewById<ImageButton>(Resource.Id.appMenuItemButton)); }
		}
	}

	public class FragmentLandingViewHolder : Java.Lang.Object
	{
		private readonly View view;

		private LinearLayout takePhotoLayout;
		private Pw.Lena.Slave.Droid.UI.TextView landing_TakePhoto;
		private LinearLayout spendingsLayout;
		private Pw.Lena.Slave.Droid.UI.TextView spendingCountTextView;
		private Pw.Lena.Slave.Droid.UI.TextView landing_Spendings;
		private Pw.Lena.Slave.Droid.UI.TextView takeCareSpendingCountTextView;
		private Pw.Lena.Slave.Droid.UI.TextView landing_takeCare;
		private LinearLayout layout_attachments;
		private Pw.Lena.Slave.Droid.UI.TextView attachmentCountTextView;
		private Pw.Lena.Slave.Droid.UI.TextView landing_attachments;
		private Pw.Lena.Slave.Droid.UI.TextView takeCareAttachmentCountTextView;
		private Pw.Lena.Slave.Droid.UI.TextView landing_takeCare2;

		public FragmentLandingViewHolder(View view)
		{
			this.view = view;
		}

		public FragmentLandingViewHolder(Activity activity)
		{
			view = activity.FindViewById(global::Android.Resource.Id.Content);
		}

		public LinearLayout TakePhotoLayout
		{
			get { return takePhotoLayout ?? (takePhotoLayout = view.FindViewById<LinearLayout>(Resource.Id.takePhotoLayout)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView LandingTakePhoto
		{
			get { return landing_TakePhoto ?? (landing_TakePhoto = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.Landing_TakePhoto)); }
		}

		public LinearLayout SpendingsLayout
		{
			get { return spendingsLayout ?? (spendingsLayout = view.FindViewById<LinearLayout>(Resource.Id.spendingsLayout)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView SpendingCountTextView
		{
			get { return spendingCountTextView ?? (spendingCountTextView = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.spendingCountTextView)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView LandingSpendings
		{
			get { return landing_Spendings ?? (landing_Spendings = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.Landing_Spendings)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView TakeCareSpendingCountTextView
		{
			get { return takeCareSpendingCountTextView ?? (takeCareSpendingCountTextView = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.takeCareSpendingCountTextView)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView LandingTakeCare
		{
			get { return landing_takeCare ?? (landing_takeCare = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.landing_takeCare)); }
		}

		public LinearLayout LayoutAttachments
		{
			get { return layout_attachments ?? (layout_attachments = view.FindViewById<LinearLayout>(Resource.Id.layout_attachments)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView AttachmentCountTextView
		{
			get { return attachmentCountTextView ?? (attachmentCountTextView = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.attachmentCountTextView)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView LandingAttachments
		{
			get { return landing_attachments ?? (landing_attachments = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.landing_attachments)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView TakeCareAttachmentCountTextView
		{
			get { return takeCareAttachmentCountTextView ?? (takeCareAttachmentCountTextView = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.takeCareAttachmentCountTextView)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView LandingTakeCare2
		{
			get { return landing_takeCare2 ?? (landing_takeCare2 = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.landing_takeCare2)); }
		}
	}

	public class ToolbarViewHolder : Java.Lang.Object
	{
		private readonly View view;

		private Android.Support.V7.Widget.Toolbar toolbar;
		private Pw.Lena.Slave.Droid.UI.TextView titleTextView;
		private ImageView icon_first_step;
		private ImageView icon_second_step;

		public ToolbarViewHolder(View view)
		{
			this.view = view;
		}

		public ToolbarViewHolder(Activity activity)
		{
			view = activity.FindViewById(global::Android.Resource.Id.Content);
		}

		public Android.Support.V7.Widget.Toolbar Toolbar
		{
			get { return toolbar ?? (toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar)); }
		}

		public Pw.Lena.Slave.Droid.UI.TextView TitleTextView
		{
			get { return titleTextView ?? (titleTextView = view.FindViewById<Pw.Lena.Slave.Droid.UI.TextView>(Resource.Id.titleTextView)); }
		}

		public ImageView IconFirstStep
		{
			get { return icon_first_step ?? (icon_first_step = view.FindViewById<ImageView>(Resource.Id.icon_first_step)); }
		}

		public ImageView IconSecondStep
		{
			get { return icon_second_step ?? (icon_second_step = view.FindViewById<ImageView>(Resource.Id.icon_second_step)); }
		}
	}
}

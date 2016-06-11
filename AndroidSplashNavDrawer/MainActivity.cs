using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Support.V7.App;
using Android.Views;
using Android.App;
using System.Collections.Generic;

namespace SplashNavigationDrawer
{
    [Activity (Label = "Memerator.Droid", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        //Fragments
        HomeFragment homeFragment;
        AboutFragment aboutFragment;
        DevicesFragment devicesFragment;
        PostFragment postFragment;
        ReviewFragment reviewFragment;
        WritePostFragment writePostFragment;

        private SupportFragment mCurrentFragment = new SupportFragment();
        private Stack<SupportFragment> mStackFragments;

        DrawerLayout drawerLayout;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            // Init toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            //Load Fragment
            homeFragment = new HomeFragment();
            aboutFragment = new AboutFragment();
            devicesFragment = new DevicesFragment();
            postFragment = new PostFragment();
            reviewFragment = new ReviewFragment();
            writePostFragment = new WritePostFragment();

            //Fragment helper
            mStackFragments = new Stack<SupportFragment>();

            // Attach item selected handler to navigation view
            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            // Create ActionBarDrawerToggle button and add it to the toolbar
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            //load default home screen
            //var ft = FragmentManager.BeginTransaction();
            //ft.AddToBackStack(null);
            //ft.Add(Resource.Id.HomeFrameLayout, new HomeFragment());
            //ft.Commit();
            Android.Support.V4.App.FragmentTransaction tx = SupportFragmentManager.BeginTransaction();

            tx.Add(Resource.Id.HomeFrameLayout, homeFragment);
            tx.Add(Resource.Id.HomeFrameLayout, devicesFragment);
            tx.Add(Resource.Id.HomeFrameLayout, postFragment);
            tx.Add(Resource.Id.HomeFrameLayout, reviewFragment);
            tx.Add(Resource.Id.HomeFrameLayout, writePostFragment);
            tx.Add(Resource.Id.HomeFrameLayout, aboutFragment);
            tx.Hide(aboutFragment);
            tx.Hide(devicesFragment);
            tx.Hide(postFragment);
            tx.Hide(reviewFragment);
            tx.Hide(writePostFragment);

            mCurrentFragment = homeFragment;
            tx.Commit();
        }


        private void ShowFragment(SupportFragment fragment)
        {

            if (fragment.IsVisible)
            {
                return;
            }

            var trans = SupportFragmentManager.BeginTransaction();


            fragment.View.BringToFront();
            mCurrentFragment.View.BringToFront();

            trans.Hide(mCurrentFragment);
            trans.Show(fragment);

            trans.AddToBackStack(null);
            mStackFragments.Push(mCurrentFragment);
            trans.Commit();

            mCurrentFragment = fragment;

        }
        //define custom title text
        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.app_name);
            base.OnResume();
        }
        //define action for navigation menu selection
        void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.nav_home):
                    Toast.MakeText(this, "Home selected!", ToastLength.Short).Show();
                    ShowFragment(homeFragment);
                    break;
                case (Resource.Id.nav_write_post):
                    Toast.MakeText(this, "Write Post selected!", ToastLength.Short).Show();
                    ShowFragment(writePostFragment);
                    break;
                case (Resource.Id.nav_post):
                    ShowFragment(postFragment);
                    break;
                case (Resource.Id.nav_devices):
                    ShowFragment(devicesFragment);
                    break;
                case (Resource.Id.nav_about):
                    ShowFragment(aboutFragment);
                    break;
            }
            // Close drawer
            drawerLayout.CloseDrawers();
        }

        //add custom icon to tolbar
        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            if (menu != null)
            {
                menu.FindItem(Resource.Id.action_refresh).SetVisible(true);
                menu.FindItem(Resource.Id.action_attach).SetVisible(false);
            }
            return base.OnCreateOptionsMenu(menu);
        }
        //define action for tolbar icon press
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    //this.Activity.Finish();
                    return true;
                case Resource.Id.action_attach:
                    //FnAttachImage();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        //to avoid direct app exit on backpreesed and to show fragment from stack
        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount != 0)
            {
                FragmentManager.PopBackStack();// fragmentManager.popBackStack();
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}



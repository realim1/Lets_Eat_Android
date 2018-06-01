using Android.App;
using Android.Widget;
using Android.OS;

using Firebase;
using Firebase.Auth;
using Firebase.Database;

namespace Lets_Eat_Android
{
    [Activity(Label = "Let's Eat", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static FirebaseApp app;
        private FirebaseAuth auth;
        private FirebaseUser currentUser;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource

            InitFirebase();
            StartActivity(typeof(Views.Login.Login));
            Finish();

        }

        private void InitFirebase()
        {
            var options = new FirebaseOptions.Builder()
                                             .SetApplicationId("1:100312271278:android:3d79789aa8f833cd")
                                             .SetApiKey("AIzaSyAH9j_K2-wcnrmxj9hEarZaFvCh6dMsOWw")
                                             .SetDatabaseUrl("https://fir-database-ec02e.firebaseio.com/")
                                             .Build();
            if (app == null)
                app = FirebaseApp.InitializeApp(this, options);

            auth = FirebaseAuth.GetInstance(app);
            auth.AuthState += (sender, e) =>
            {
                currentUser = e?.Auth?.CurrentUser;

                if (currentUser != null)
                {

                }
                else
                {

                }
            };

        }
    }
}




using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Java.Util;
using Android.Speech.Tts;
using Android.Runtime;
using Android.Content;

namespace Android_OS
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, TextToSpeech.IOnInitListener
    {
        private const int LanguageNeeded = 103;
        private Locale _lang;
        private TextToSpeech _textToSpeech;

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            if(status == OperationResult.Success)
            {
                _textToSpeech.SetLanguage(_lang);
            }
            else
            {
                _textToSpeech.SetLanguage(Locale.Default);
            }

        }

        protected override void OnActivityResult(int req, Result res, Intent data)
        {
            if(req == LanguageNeeded)
            {
                //we need a language installed
                var installTextEngine = new Intent();
                installTextEngine.SetAction(TextToSpeech.Engine.ActionInstallTtsData);
                StartActivity(installTextEngine);
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Get our button from the layout reference,
            // and attach an event to it 
            Button button = FindViewById<Button>(Resource.Id.button1);
            var textEdit = FindViewById<EditText>(Resource.Id.editText1);

            _textToSpeech = new TextToSpeech(this, this, "com.google.androd.tts");
            _lang = Locale.Default;

            _textToSpeech.SetPitch(.5f);
            _textToSpeech.SetSpeechRate(.8f);




            button.Click += delegate
            {
                if(!string.IsNullOrEmpty(textEdit.Text))
                {
                    _textToSpeech.Speak(textEdit.Text, QueueMode.Flush, null);
                }

            };
        }
    }
}


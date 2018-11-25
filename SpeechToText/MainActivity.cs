/*******************TEST THIS ON LIVE DEVICE*************/

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Content.PM;
using Android.Speech;
using Java.Util;

namespace SpeechToText
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private const int Voice = 10;
        private bool _isRecording;
        private Button _recordButton;
        private TextView _textBox;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //UI
            _recordButton = FindViewById<Button>(Resource.Id.btnRecord);
            _textBox = FindViewById<TextView>(Resource.Id.textResult);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            var canRecord = PackageManager.FeatureMicrophone;

            //detect recording
            if(canRecord != "android.hardware.microphone")
            {
                // no microphone was found
                var alert = new Android.App.AlertDialog.Builder(_recordButton.Context);
                alert.SetTitle("No");
                alert.SetPositiveButton("Ok", (sender, e) =>
                {
                    _textBox.Text = "No microphone present";
                    _recordButton.Enabled = false;

                });

                alert.Show();

            }
            else
            {
                _recordButton.Click += delegate
                {
                    _recordButton.Text = "Stop Listening";
                    _isRecording  = !_isRecording;

                    if (_isRecording)
                    {
                        var speechIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                        speechIntent.PutExtra(RecognizerIntent.ExtraLanguageModel,
                            RecognizerIntent.LanguageModelFreeForm);
                        speechIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 2000);
                        speechIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 2000);
                        speechIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                        speechIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                        speechIntent.PutExtra(RecognizerIntent.ExtraLanguage, Locale.Default);

                        //Start Activity
                        StartActivityForResult(speechIntent, Voice);





                    }



                };
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            if (requestCode == Voice)
            {
                if (resultVal == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        var textInput = _textBox.Text + matches[0];

                        if (textInput.Length > 500)
                            textInput = textInput.Substring(0, 500);
                        _textBox.Text = textInput;
                    }
                    else
                    {
                        _textBox.Text = "I don't understand!";
                        _recordButton.Text = "Start Listening";
                    }
                }
            }
        }
    }
}


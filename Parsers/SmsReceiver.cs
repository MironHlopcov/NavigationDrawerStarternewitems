using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Telephony;
using System.Text.RegularExpressions;
using static Android.Provider.CalendarContract;
using Android.Util;
using Android.Provider;
using MyFinToControl.Configs.ManagerCore;

namespace MyFinToControl.Parsers
{
    [BroadcastReceiver(Enabled = true, Exported = false, Label = "SMS Receiver")]
    [IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED", Intent.CategoryDefault })]
    public class SmsReceiver : BroadcastReceiver
    {
        private const string IntentAction = "android.provider.Telephony.SMS_RECEIVED";
        private static readonly string Sender = "SMS Sender number here";
        private static readonly string[] OtpMessageBodyKeywordSet = { "Keyword1", "Keyword2" }; //You must define your own Keywords
        public override async void OnReceive(Context context, Intent intent)
        {
            try
            {
                Bundle bundle = intent.Extras;
                if (bundle != null)
                {
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
                    {
                        ConfigurationManager configManager = ConfigurationManager.ConfigManager;
                        var configuration = configManager.BankConfigurationFromJson;
                        
                        SmsMessage[] msgs = Telephony.Sms.Intents.GetMessagesFromIntent(intent);
                        var smstext = new StringBuilder();
                        foreach (var msg in msgs)
                        {
                            if (configuration.Banks.Any(x => x.SmsNumber == msg.OriginatingAddress))
                            {
                                var sms = new Sms();
                                sms.setMsg(msg.MessageBody.ToString());
                                sms.setTime(msg.TimestampMillis.ToString());//
                                sms.setId("1");
                                sms.setAddress(msg.OriginatingAddress.ToString());
                                Parser parser = new Parser(sms, configuration.Banks);
                                var data = await parser.GetData();
                                if (data != null)
                                {
                                    await DatesRepositorio.AddDatas(data);//This operation took 10825
                                }
                            }
                        }
                    }
                    else
                    {
                        var smsArray = (Java.Lang.Object[])bundle.Get("pdus");
                        SmsMessage[] messages = new SmsMessage[smsArray.Length];
                        for (int i = 0; i < smsArray.Length; i++)
                        {
                            messages[i] = SmsMessage.CreateFromPdu((byte[])smsArray[i]);
                        }
                        StringBuilder content = new StringBuilder();
                        if (messages.Length > 0)
                        {
                            foreach (var message in messages)
                            {
                                content.Append(message.DisplayMessageBody.ToString());
                            }
                        }
                    }
                }
             
            }
            catch (System.Exception ex)
            {
                //Toast.MakeText(context, ex.Message, ToastLength.Long).Show();
            }
        }

        private static string ExtractNumber(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            var regPattern = @"\d+";
            var number = Regex.Match(text, regPattern).Value;
            return number;
        }

    }
}
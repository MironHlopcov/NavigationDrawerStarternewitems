using Android.Content;
using MyFinToControl.Configs.ManagerCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinToControl
{
    public class SmsReader
    {
        public List<Sms> LstSms { get; private set; }
        private Context _context;
        public SmsReader(Context context)
        {
            _context = context;
        }
        public async Task<List<Sms>> GetAllSmsAsync(List<BankConfiguration> adressFilter)
        {
            ContentResolver contentResolver = _context.ContentResolver;
            LstSms = new List<Sms>();

            Sms objSms = new Sms();
            Android.Net.Uri message = Android.Net.Uri.Parse("content://sms/");
            await Task.Run(() => {

                using (var c = contentResolver.Query(message, null, null, null, null))
                {
                    //StartManagingCursor(c);
                    int totalSMS = c.Count;

                    if (c.MoveToFirst())
                    {
                        for (int i = 0; i < totalSMS; i++)
                        {
                            if (adressFilter.Select(x => x.SmsNumber).Contains(c.GetString(c.GetColumnIndexOrThrow("address"))))
                            {
                                objSms = new Sms();
                                objSms.setId(c.GetString(c.GetColumnIndexOrThrow("_id")));
                                objSms.setAddress(c.GetString(c.GetColumnIndexOrThrow("address")));
                                objSms.setMsg(c.GetString(c.GetColumnIndexOrThrow("body")));
                                objSms.setReadState(c.GetString(c.GetColumnIndex("read")));
                                objSms.setTime(c.GetString(c.GetColumnIndexOrThrow("date")));
                                if (c.GetString(c.GetColumnIndexOrThrow("type")).Contains("1"))
                                {
                                    objSms.setFolderName("inbox");
                                }
                                else
                                {
                                    objSms.setFolderName("sent");
                                }
                                LstSms.Add(objSms);
                            }
                            c.MoveToNext();
                        }
                    }
                    else
                    {
                        LstSms.Add(new Sms());
                        // return LstSms;
                    }
                    c.Close();
                    //return LstSms;
                }
            });
            return LstSms;
        }
    }
}
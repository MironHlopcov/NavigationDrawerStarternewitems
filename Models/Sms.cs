using System;

namespace MyFinToControl
{
    public class Sms
    {
        private String Id;
        private String Address;
        private String Msg;
        private String ReadState;//"0" for have not read sms and "1" for have read sms
        private String Time;
        private String FolderName;

        public String getId()
        {
            return Id;
        }
        public String getAddress()
        {
            return Address;
        }
        public String getMsg()
        {
            return Msg;
        }
        public String getReadState()
        {
            return ReadState;
        }
        public String getTime()
        {
            return Time;
        }
        public String getFolderName()
        {
            return FolderName;
        }


        public void setId(String id)
        {
            Id = id;
        }
        public void setAddress(String address)
        {
            Address = address;
        }
        public void setMsg(String msg)
        {
            Msg = msg;
        }
        public void setReadState(String readState)
        {
            ReadState = readState;
        }
        public void setTime(String time)
        {
            Time = time;
        }
        public void setFolderName(String folderName)
        {
            FolderName = folderName;
        }

    }
}
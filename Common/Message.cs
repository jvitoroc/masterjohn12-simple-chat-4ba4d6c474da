using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    public class Message
    {
        private Dictionary<string, string> headers;
        public string message;

        public string Request { get { return GetHeaderValue("request"); } set { SetHeaderValue("request", value); } }
        public Guid RequestId { get { return Guid.Parse(GetHeaderValue("request_id")); } set { SetHeaderValue("request_id", value.ToString()); } }
        public int ResponseCode { get { return GetHeaderValueAsInt("response_code", 0); } set { SetHeaderValue("response_code", value.ToString()); } }

        public Message()
        {
            headers = new Dictionary<string, string>();
        }

        public static Message CreateTimeoutResponse()
        {
            Message m = new Message();
            m.SetHeaderValue("response_code", "20");

            return m;
        }

        public static Message CreateUnfulfilledResponse()
        {
            Message m = new Message();
            m.SetHeaderValue("response_code", "21");

            return m;
        }

        public static Message Create404Response()
        {
            Message m = new Message();
            m.SetHeaderValue("response_code", "22");

            return m;
        }
      
        public static Message CreateSucessResponse()
        {
            Message m = new Message();
            m.SetHeaderValue("response_code", "10");

            return m;
        }
        public void SetAsResponse(Guid requestId)
        {
            SetHeaderValue("type", "response");
            SetRequestId(requestId);
        }

        public void SetAsRequest(Guid requestId)
        {
            SetHeaderValue("type", "request");
            SetRequestId(requestId);
        }

        public void SetRequestId(Guid requestId)
        {
            SetHeaderValue("request_id", requestId.ToString());
        }

        public void SetAsNotification()
        {
            SetHeaderValue("type", "notification");
        }

        public string GetHeaderValue(string key)
        {
            string value;
            if (!headers.TryGetValue(key, out value))
                value = "";

            return value;
        }

        public int GetHeaderValueAsInt(string key, int defaultValue)
        {
            string value = GetHeaderValue(key);
            if (value != "")
                return Int32.Parse(value);
            else
                return defaultValue;
        }

        public void SetHeaderValue(string key, string value)
        {
            if (!headers.ContainsKey(key))
                headers.Add(key, value);
            else
                headers[key] = value;
        }

        public bool IsResponse()
        {
            return (GetHeaderValue("type") == "response");
        }

        public bool IsRequest()
        {
            return (GetHeaderValue("type") == "request");
        }

        public bool IsNotification()
        {
            return (GetHeaderValue("type") == "notification");
        }

        public static Message Convert(byte[] message)
        {
            StringReader stringReader;
            Message m = new Message();
            string line, key, value;
            int assignPos;

            string _message = Encoding.UTF8.GetString(message);
            stringReader = new StringReader(_message);

            while ((line = stringReader.ReadLine()) != "-")
            {
                assignPos = line.IndexOf("=");
                key = line.Substring(0, assignPos);
                value = line.Substring(assignPos + 1);
                m.headers.Add(key, value);
            }

            m.message = stringReader.ReadToEnd();

            return m;
        }

        public static byte[] Convert(Message message)
        {
            string buffer = "";

            foreach(KeyValuePair<string, string> header in message.headers){
                buffer = buffer + header.Key + "=" + header.Value + '\n';
            }

            buffer = buffer + "-" + '\n';
            buffer = buffer + message.message;

            return Encoding.UTF8.GetBytes(buffer);
        }
    }
}
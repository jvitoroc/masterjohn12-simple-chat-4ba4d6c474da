using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    public class Message
    {
        public Dictionary<string, string> headers;
        public string message;

        public Message()
        {
            headers = new Dictionary<string, string>();
        }

        public string GetHeaderValue(string key)
        {
            string value = "";
            headers.TryGetValue("define_id", out value);

            return value;
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
                key = line.Substring(0, assignPos + 1);
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
            buffer = buffer + message;

            return Encoding.UTF8.GetBytes(buffer);
        }
    }
}
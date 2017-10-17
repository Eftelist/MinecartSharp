using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MinecartSharp.Utils.Mojang
{
    class MojangApi
    {
        public string GetUUID(string username)
        {
            if (string.IsNullOrEmpty(username))
                return "";

            using (WebClient webClient = new WebClient())
            {

                string result = "";
                try
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    result = webClient.DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username);

                }
                catch (WebException e)
                {
                    Globals.Logger.Log(LogType.Error, "Couldn't receive UUID from '" + username + "'");
                    return "";
                }

                dynamic json = JsonConvert.DeserializeObject(result);
                if (json.Count > 0)
                {
                    return Guid.Parse(json.id).ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}

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
        public string getUUID(string username)
        {
            if (string.IsNullOrEmpty(username))
                return "";

            using (WebClient webClient = new WebClient())
            {

                string result = "";
                try
                {
                    result = webClient.UploadString("https://api.mojang.com/profiles/minecraft", $"[\"{username}\"]");

                }
                catch (WebException e)
                {
                    Globals.Logger.Log(LogType.Error, "Couldn't retrieve uuid for username" + " " + username);
                    Globals.Logger.Log(LogType.Error, e.Message);
                    return "";
                }

                dynamic json = JsonConvert.DeserializeObject(result);
                if (json.Count > 0)
                {
                    string UUID = json[0].id;
                    Globals.Logger.Log(LogType.Info, "UUID = " + Guid.Parse(UUID).ToString());
                    return Guid.Parse(UUID).ToString();
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

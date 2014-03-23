using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository.Base;

namespace Business.Base
{
    public class Util
    {
        public static ResponseInformation GetUserLanguageInformation()
        {
            var user = User.Instance.GetLoggedUser();
            if (HttpContext.Current.Session == null)
            {
                return new ResponseInformation
                {
                    Language = (int)ELanguage.PtBr
                };

            }
            var lang = HttpContext.Current.Session["Language"];
            var respInfo = new ResponseInformation();

            if (lang != null)
            {
                respInfo.Language = (int)lang;
            }
            else
            {
                respInfo.Language = (int)ELanguage.PtBr;
            }
            if (user != null)
            {
                respInfo.UserId = user.Id;
                respInfo.UserName = user.Name;
            }

            return respInfo;
        }

        public static ResponseInformation GetUserLanguageInformation(int id)
        {
            var user = User.Instance.GetById(id);
            var respInfo = new ResponseInformation
            {
                Language = (int) ELanguage.PtBr,
                UserId = user.Id,
                UserName = user.Name
            };

            return respInfo;
        }


        public static ResponseInformation GetUserLanguageInformation(HttpCookie cookie)
        {
            var user = User.Instance.GetLoggedUser(cookie);
            var lang = HttpContext.Current.Session["Language"];
            var respInfo = new ResponseInformation();

            if (lang != null)
            {
                respInfo.Language = (int)lang;
            }
            else
            {
                respInfo.Language = (int)ELanguage.PtBr;
            }

            respInfo.UserId = user.Id;
            respInfo.UserName = user.Name;

            return respInfo;
        }

        public static ResponseObject JoinResponse(List<ResponseObject> responses)
        {
            var messages = new List<String>();
            foreach (var response in responses)
            {
                messages.AddRange(response.Messages);
            }

            return new ResponseObject
                {
                    Status = responses.All(a => a.Status),
                    Messages = messages,

                };
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            var hex = BitConverter.ToString(bytes);
            return hex.Replace("-", "").ToLower();
        }

        public static byte[] StringToByteArray(string str)
        {
            var numberChars = str.Length;
            var bytes = new byte[numberChars / 2];

            for (var i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);

            return bytes;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Base;
using Repository.Base;
using Twilio;

namespace Business
{
    public class SMS : BaseBussines<SMS>
    {
        string AccountSid = "AC0598a6f1b096913eb098e56ec1981b07";
        string AuthToken = "7b18f2e37af064474d43f64fa498a77a";
        string fromNumber = "+1 612-255-4548";

        private SMS()
        {
        }

        public ResponseObject SendSMS(string message, string phone)
        {

            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            var sms = twilio.SendSmsMessage(fromNumber, phone, message);

            if (sms.RestException != null)
            {
                var error = sms.RestException.Message;
                return new ResponseObject(true, error); 
            }

            return new ResponseObject(true, "Mensagem enviada com sucesso!");    
        }

    }
}

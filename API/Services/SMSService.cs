namespace API.Services
{
    public class SMSService
    {
        public void SendSMS()
        {
            
            string UserName = "MyUserName";
            string Password = "MyPassword";
            DateTime SendDateTime = DateTime.Now;
            string SMSMessageText = "پیامک تستی من";
            string LineNumber = "public";
            string[] Mobiles = new string[]
            {
                "9120000000",
                "9150000000",
            };

            // SMS.WebService2SoapClient client = new SMS.WebService2SoapClient("WebService2Soap12");

            // SMS.SendResult result = client.SendSimple(UserName, Password, SendDateTime, SMSMessageText, LineNumber, Mobiles);

            // if (result.Status == SMS.Status.Success)
            // {
            //     //خروجی
            // }
            
        }
    }
}

using System;

namespace Com.BigWin.Frontend.Data
{
    public class LoginScreenData
    {
    }

    [Serializable]
    public class LoginForm
    {
        public string retailer_id;
        public string password;
        public LoginForm(string user_id, string password)
        {
            this.retailer_id = user_id;
            this.password = password;
        }
    }

    [Serializable]
    public class LoginResponseData
    {
        public int status;
        public string message;
        public LoginResponce data;
    }

    [Serializable]
    public class LoginResponce
    {
        public string retailer_id;
        public string distributor_id;
        public string username;
        public string cash_balance;
    }



    [Serializable]
    public class LogoutForm
    {
        public string user_id;
        public LogoutForm(string user_id)
        {
            this.user_id = user_id;
        }
    }
}
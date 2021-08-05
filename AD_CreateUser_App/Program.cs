using System;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AD_CreateUser_App
{
    public class Program
    {

        public static void Main(string[] args)
        {
            //Start Program - Fetch required information
            Console.WriteLine("Welcome User!");
            CreateUser(ConfigurationManager.AppSettings["DisplayName"], 
                ConfigurationManager.AppSettings["MailNickName"],
                ConfigurationManager.AppSettings["UserPrincpalName"],
                ConfigurationManager.AppSettings["Password"],
                ConfigurationManager.AppSettings["AccountEnabled"],
                ConfigurationManager.AppSettings["ForceChangePasswordNextSignIn"]);
        }

        private static void CreateUser(string sDisplayName, string sMailNickName, string sUserPrincpalName, string sPassword, string bAccountEnabled, string bForceChangePasswordNextSignIn)
        {
            try
            {
                //Adding data to password profile collection
                var passwordProfile = new PasswordProfile
                {
                    forceChangePasswordNextSignIn = Convert.ToBoolean(bForceChangePasswordNextSignIn),
                    password = sPassword
                };

                //Creating Json object
                JObject jObjectbody = new JObject();
                jObjectbody.Add("accountEnabled", Convert.ToBoolean(bAccountEnabled));
                jObjectbody.Add("displayName", sDisplayName);
                jObjectbody.Add("mailNickname", sMailNickName);
                jObjectbody.Add("userPrincipalName", sUserPrincpalName);
                jObjectbody.Add("passwordProfile", JsonConvert.SerializeObject(passwordProfile));

                //Creating client with URL
                var client = new RestClient("https://graph.microsoft.com/v1.0/users");
                client.Timeout = -1;

                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Bearer " + ConfigurationManager.AppSettings["AccessToken"]);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }

    public class PasswordProfile
    {
        public bool forceChangePasswordNextSignIn { get; set; }
        public string password { get; set; }
    }
}

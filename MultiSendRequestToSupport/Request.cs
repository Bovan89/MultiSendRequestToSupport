using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultiSendRequestToSupport
{
    class Request
    {
        static string JsonTemplate =
@"{
    'Text' : 'requestText'
}";        

        public string Text { get; set; }

        public Uri SupportUri { get; set; }
        
        public string Content
        {
            get
            {
                return CreateRequestJSON();
            }
        }

        public void Client_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            try
            {            
                string answer = System.Text.Encoding.UTF8.GetString(e.Result);
                Console.WriteLine($"<--- Запрос '{Text}' обработан" + '\n' + "Ответ сервера:" + '\n' + answer + '\n');
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Запрос '{Text}' выдал исключение" + '\n' + ex.Message + '\n' + ex.InnerException?.Message + '\n');
            }
        }

        public string CreateRequestJSON()
        {
            if (Text == null || Text == "")
            {
                Text = "Тестовая заявка " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK");
            }

            return JsonTemplate.Replace("requestText", Text);
        }

        public void SendRequest()
        {
            WebClient client = new WebClient
            {
                UseDefaultCredentials = true
            };

            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            client.UploadDataCompleted += this.Client_UploadDataCompleted;
            client.UploadDataAsync(SupportUri, "POST", Encoding.UTF8.GetBytes(Content));

            Console.WriteLine($"---> Запрос '{Text}' послан");
        }
    }
}

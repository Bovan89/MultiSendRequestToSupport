using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiSendRequestToSupport
{
    class Program
    {
        static int i1;
        static int i2;
        static int rc;
        static string createReqUrl;

        static void Main(string[] args)
        {
            i1 = Convert.ToInt32(ConfigurationManager.AppSettings["I1"]);
            i2 = Convert.ToInt32(ConfigurationManager.AppSettings["I2"]);
            rc = -1;
            createReqUrl = ConfigurationManager.AppSettings["SupportUrl"];

            if (args != null && args.Length != 0)
            { 
                if (args.Length > 0)
                {                    
                    i1 = Convert.ToInt32(args[0]);
                }
                if (args.Length > 1)
                {
                    i2 = Convert.ToInt32(args[1]);
                }
                if (args.Length > 2)
                {
                    rc = Convert.ToInt32(args[2]);
                }
            }

            Console.WriteLine("Программа запущена с параметрами");
            Console.WriteLine();
            Console.WriteLine($"Url-создания заявки {createReqUrl}");
            Console.WriteLine($"Нижняя граница интервала отправки запроса {i1}");
            Console.WriteLine($"Верхняя граница интервала отправки запроса {i2}");
            Console.WriteLine($"Количество запросов {rc}");
            Console.WriteLine();
            Console.WriteLine();

            //Поток в котором будет отправка
            var th = new Thread(new ThreadStart(StartSending));
            th.Start();

            Console.ReadLine();

            if (th != null && th.ThreadState == ThreadState.Running)
            {
                th.Abort();
            }
        }


        public static void StartSending()
        {
            while (rc < 0 || rc > 0)
            {
                Request req = new Request() { SupportUri = new Uri(createReqUrl) };
                req.SendRequest();
                                
                //Thread.Sleep(1000 * new Random().Next(Math.Min(i1, i2), Math.Max(i1, i2)));
                Sleep();

                rc--;
            }
        }

        private static void Sleep()
        {
            DateTime s1 = DateTime.Now;
            int sleepTime = new Random().Next(Math.Min(i1, i2), Math.Max(i1, i2));
            while ((DateTime.Now - s1).Seconds < sleepTime)
            {
                //sleep                    
            }
        }
    }
}

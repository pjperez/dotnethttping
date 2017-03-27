using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace dotnethttping
{
    class Program
    {
        private static HttpClient httpClient = new HttpClient();
        static void Main(string[] args)
        {
            string uriParam = "";
            int iters = 5;
            httpClient.Timeout = TimeSpan.FromMilliseconds(1000);

            switch (args.Length)
            {
                case 0:
                    Console.WriteLine("Not enough arguments");
                    Environment.Exit(1);
                    break;
                case 1:
                    uriParam = args[0];
                    Ping(uriParam, iters);
                    break;
                case 2:
                    uriParam = args[0];
                    iters = Convert.ToInt32(args[1]);
                    Ping(uriParam, iters);
                    break;
            }

        }

        public static void Ping(string uriParam, int iters)
        {

            Console.WriteLine("Requesting {0} {1} times", uriParam, iters);

            for (int i = 1; i <= iters; i++)
            {
                try
                {
                    Uri uri = new Uri(uriParam);

                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    var result = httpClient.GetAsync(uri).Result;
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    Console.WriteLine("Request #{0} - Status Code={1} - RTT={2}ms", i, result.StatusCode, elapsedMs);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("The provided parameter is empty or not a valid URI. Only http and https are allowed: {0}", uriParam);
                    Environment.Exit(1);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("HTTPRequestException: {0}", e);
                }
                catch (UriFormatException)
                {
                    Console.WriteLine("The provided parameter is empty or not a valid URI. Only http and https are allowed: {0}", uriParam);
                    Environment.Exit(1);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error requesting site {0}", uriParam);
                    Console.WriteLine(e);
                }
                System.Threading.Thread.Sleep(1000);

            }

            httpClient.Dispose();
        }
    }
}

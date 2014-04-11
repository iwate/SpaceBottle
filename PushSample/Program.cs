using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PushSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //MobileService.InvokeApiAsync("bottle", Newtonsoft.Json.Linq.JToken.FromObject(bottle), HttpMethod.Post, null).Wait();
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create("https://spacebottle.azure-mobile.net/api/bottle");
                // Set the Method property of the request to POST.
                request.Method = "POST";
                request.Headers = new WebHeaderCollection { { "X-ZUMO-MASTER", "pQOEWCXgGfQUOmkyupwuJDtzDfrjsO79" } };
                // Create POST data and convert it to a byte array.
                string postData = "userId=Facebook:100002886440572&message=This is test message";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }
    }
}
/*
            var data = {
                message: request.body.message
            }*/
           // request.respond();
            /*push.gcm.send(registrationId, data, {
                success: function(response) {
                   console.log('Push notification sent: ', response);
                }, error: function(error) {
                   console.log('Error sending push notification: ', error);
                }
            });*/
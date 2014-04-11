using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Data.Entity.Core.Objects;

namespace CalculationWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private static string MasterKey = "pQOEWCXgGfQUOmkyupwuJDtzDfrjsO79";
        private spacebottle_dbEntities context;
        private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private static double Threshold = 50.0; // [km]
        private static double ThresholdSquare = Threshold * Threshold;

        private List<Satellite> getSatelliteFromTLE(string strUrl)
        {
            var client = new WebClient();
            var stream = client.OpenRead(strUrl);
            var reader = new StreamReader(stream);
            List<Satellite> satellites = new List<Satellite>();
           List<string> line0 = new List<string>(), line1 = new List<string>(), line2 = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var listLine = line.Split(new Char[] { ' ', '\t' }).Where(s => s.Trim() != "").Select(s => s).ToList();
                if (listLine[0] == "1")
                {
                    line1 = listLine;
                }
                else if (listLine[0] == "2")
                {
                    line2 = listLine;
                    if (line2.Count < 9) 
                    {
                        line2.Add(line2[7].Substring(11, 6));
                        line2[7] = line2[7].Substring(0, 11);
                    }
                    satellites.Add(new Satellite
                    {
                        id = Guid.NewGuid().ToString(),
                        name = String.Join(" ", line0),
                        satellite_number = line1[1],
                        international_designator = line1[2],
                        epoch_date = line1[3],
                        d_mean_motion = line1[4],
                        dd_mean_motion = line1[5],
                        drag_term = line1[6],
                        ephemeris_type = line1[7],
                        element_number = line1[8],
                        inclination  = line2[2],
                        ascension = line2[3],
                        eccentricity = line2[4],
                        arg_of_perigee = line2[5],
                        mean_anomaly = line2[6],
                        mean_motion = line2[7],
                        revolution_number = line2[8],
                        C__createdAt = DateTimeOffset.UtcNow,
                        C__updatedAt = DateTimeOffset.UtcNow
                    });
                }
                else
                {
                    line0 = listLine;
                }
            }
            return satellites;
        }
        private void Seed()
        {
            var satellites = getSatelliteFromTLE("https://portalvhds4wdrc7dngb89n.blob.core.windows.net/satellites/TLE.TXT");
            satellites.ForEach(delegate(Satellite satellite)
            {
                var count = context.Satellite.Count(s => s.satellite_number == satellite.satellite_number);
                if (count ==  0)
                {
                    context.Satellite.Add(satellite);
                }
            });
            context.SaveChanges();
        }

        private async void PostBottle(Bottle bottle)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://spacebottle.azure-mobile.net/api/bottle");
                request.Method = "POST";
                request.Headers = new WebHeaderCollection { { "X-ZUMO-MASTER", MasterKey } };
                string postData = bottle.ToString();
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = await request.GetRequestStreamAsync();
                await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = await  request.GetResponseAsync();
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.StackTrace);
            }

        }
        public override void Run()
        {
            // これはワーカーの実装例です。実際のロジックに置き換えてください。
            Trace.TraceInformation("CalculationWorkerRole entry point called", "Information");
            context = new spacebottle_dbEntities();
            //Seed();
            var satellites = context.Satellite.Select(s => new CalculationableSatellite { context = s }).ToList();
            while (true)
            {
                var users = context.Position.Select(p => new User { UserId = p.user_id, Position = p }).ToList();
                var now = DateTime.UtcNow;
                satellites.ForEach(delegate(CalculationableSatellite satellite)
                {
                    var position = satellite.getPositionAt(now);
                    users.ForEach(delegate(User user)
                    {
                        var distanceSquare = user.calcDistanceSquare(position) / 1000000;  // [m^2] -> [km^2]
                        if (distanceSquare < ThresholdSquare)
                        {
                            if (context.Ticket.Count(t => t.user_id == user.UserId && t.satellite_id == satellite.context.id && EntityFunctions.DiffHours(DateTimeOffset.UtcNow, t.C__createdAt) < 1 )== 0)
                            {
                                context.Ticket.Add(new Ticket { 
                                    user_id = user.UserId,
                                    satellite_id = satellite.context.id,
                                    is_used = false,
                                    limit = DateTimeOffset.UtcNow.AddMinutes(10),
                                    id = Guid.NewGuid().ToString(),
                                    C__createdAt = DateTimeOffset.UtcNow,
                                    C__updatedAt = DateTimeOffset.UtcNow
                                });
                                context.SaveChangesAsync();
                                PostBottle(new Bottle { userId = user.UserId, message = satellite.context.name + " is near (" + Math.Sqrt(distanceSquare) + "[km])"});
                            }
                        }
                    });
                });
                Thread.Sleep(10000);
            }
        }

        public override bool OnStart()
        {
            // 同時接続の最大数を設定します
            ServicePointManager.DefaultConnectionLimit = 12;

            // 構成の変更を処理する方法については、
            // MSDN トピック (http://go.microsoft.com/fwlink/?LinkId=166357) を参照してください。

            return base.OnStart();
        }
    }
}

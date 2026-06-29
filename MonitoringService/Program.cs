using ERP_BL.Databases;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;


namespace MonitoringService
{
    public class Program
    {
        static string keyValue;
        static string currentTime;
        static string currentUserId;
        static string lastTime = "0000001000000002";
        static bool isFirstTime = true;
        static User user;
        //static void RT(Action action, int seconds, System.Threading.CancellationToken token)
        //{
        //    if (action == null)
        //        return;
        //    Task.Run(async () => {
        //        while (!token.IsCancellationRequested)
        //        {
        //            action();
        //            await Task.Delay(TimeSpan.FromSeconds(seconds), token);
        //        }
        //    }, token);
        //}
        public static void Main()
        {

            Console.WriteLine("Init Service", DateTime.Now);
            var aTimer = new Timer();
            aTimer.Interval = ((1000 * 60) * 5);
            aTimer.Elapsed += TimerElapsed1;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;


            Console.ReadLine();
        }

        public static void TimerElapsed1(object source, System.Timers.ElapsedEventArgs e)
        {


            Console.WriteLine("Srtarting Service", DateTime.Now);

            Task.Run(() =>
            {
                Console.WriteLine("The Elapsed event A was raised at {0}", DateTime.Now);


                //  string sysUser = Environment.UserDomainName + "\\" + Environment.UserName;

                RegistryKey rb = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey key = rb.OpenSubKey(@"SOFTWARE\MicroKosm\ZAS ERP", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);



                {
                    //try
                    {
                        keyValue = (string)key.GetValue("type");

                        int keyLen = keyValue.Length;
                        currentUserId = keyValue.Substring(0, 8);
                        currentTime = keyValue.Substring(8, keyLen - 8);
                        key.Close();

                    }

                    {
                        currentUserId = null;
                        currentTime = null;
                    }

                    if (currentTime == lastTime)
                    {
                        int _curUserId;
                        currentUserId = currentUserId.TrimStart('0');
                        _curUserId = Convert.ToInt32(currentUserId);
                        UsersRepo repo = new UsersRepo();
                        try
                        {
                            user = repo.getuser(_curUserId);

                        }
                        catch (Exception ex)
                        {
                            user = null;
                        }
                        if (user != null)
                        {
                            if (user.isLoggedIn == true)
                            {
                                user.isLoggedIn = false;
                                repo.updateuser(user);
                                lastTime = currentTime;
                            }
                        }




                        else
                        {
                            lastTime = currentTime;

                        }
                    }
                }
            });

        }



    }
}

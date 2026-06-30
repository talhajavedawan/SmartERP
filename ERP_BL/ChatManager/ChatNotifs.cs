using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ChatManager
{
    
    class ChatNotifs
    {
        const string TICKER_NOTIF_PATH = "TickerNotif/";
        const string SPLASH_NOTIF_PATH = "SplashNotif/";
        const string GENERAL_NOTIF_PATH = "GeneralNotif/";


        public async Task<bool> pushTickerAsync(Ticker ticker, IFirebaseClient client)
        {
            try
            {
                var response = await client.SetAsync(TICKER_NOTIF_PATH + ticker.id, ticker);
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> pushSplashScreenAsync(SplashScreen screen, IFirebaseClient client)
        {
            try
            {
                var response = await client.SetAsync(SPLASH_NOTIF_PATH , screen);
                return true;
            }
            catch { return false; }
        }

        public SplashScreen GetSplashScreen(IFirebaseClient client)
        {
            try
            {
                var response = client.Get(SPLASH_NOTIF_PATH);
                if (string.IsNullOrEmpty(response.Body))
                    throw new Exception("New Splashscreen found");

                var screen = (response.ResultAs<SplashScreen>());
                return screen;
            }
            catch { throw new Exception("Unable to load Tickers data"); }
        }


        public bool DeleteSplashScreen(IFirebaseClient client)
        {
            try
            {
                var response = client.Delete(SPLASH_NOTIF_PATH);
                if (string.IsNullOrEmpty(response.Body))
                    throw new Exception("New Splashscreen found");
                return false;
            }
            catch { throw new Exception("Unable to load Tickers data"); }
        }

        public List<Ticker> GetTickers(IFirebaseClient client)
        {
            List < Ticker > tickers = null;
            try
            {
                string body = "";
                var response = client.Get(TICKER_NOTIF_PATH);
                if (string.IsNullOrEmpty(response.Body))
                    throw new Exception("New Tickers found");
                if (!response.Body.Trim().StartsWith("["))
                    body = ("[" + response.Body + "]");
                else
                    body = response.Body;


                tickers = (response.ResultAs<List<Ticker>>());
                if (tickers[0] == null)
                    tickers.RemoveAt(0);
                //if(tickers.Count>2

                DeleteExpiredTickers(tickers, client);
                
            }
            catch (Exception exception) { /*throw new Exception("Unable to load Tickers data"); */}

            return tickers;
        }

        public bool DeleteTicker(int tickerId, IFirebaseClient client)
        {
            try
            {
                client.Delete(TICKER_NOTIF_PATH + tickerId);
            }
            catch
            { return false; }
            return true;
        }

        private void DeleteExpiredTickers(List<Ticker> tickers, IFirebaseClient client)
        {
            foreach (var ticker in tickers)
            {
                if (ticker.validTill < DateTime.Today)
                {
                    client.Delete(TICKER_NOTIF_PATH + ticker.id);
                }
            }
            tickers.RemoveAll(x => x.validTill > DateTime.Today);
        }
    }

    public class Ticker
    {
        public int id { get; set; }
        public string newsString { get; set; }
        public DateTime validTill { get; set; }
        public int initUserId { get; set;}
        public string tickerColor { get; set; }
    }

    public class SplashScreen
    {
        public string ImagePath { get; set; }
        public int timeoutSecond { get; set; }
    }

}

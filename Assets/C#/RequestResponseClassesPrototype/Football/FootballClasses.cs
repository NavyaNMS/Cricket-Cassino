using System.Collections.Generic;

namespace ApiPrototype
{

    public class LastDrawReport
    {
        public string draw_time;
        public string win_no_A;
        public string win_no_B;
        public string win_no_C;
        public string win_no_D;
        public string win_no_E;
        public string win_no_F;
        public string win_no_G;
        public string win_no_H;
        public string win_no_I;
        public string win_no_J;
    }

    public class CurrentRoundData
    {
        public string current_draw;
        public string retailer_bal;
        public List<LastDrawReport> last_draw_report = new List<LastDrawReport>();

    }

    public class CurrentRound
    {
        public int status;
        public string message;
        public CurrentRoundData data;
    }


    public class CurrentTimer
    {
        public string message;
        public string status;
        public string timer;
        public int timer_unix;
    }


    public class Retailer
    {
        public string retailer_id;
        public string game_id;
    }

    public class Bet
    {
        public string series;
        public string bet_no;
        public string points;
    }

    public class BetsDetail
    {
        public string retailer_id;
        public string game_id;
        public string draw_time;
        public List<Bet> bet;
        public string total_points;
    }



    public class ClaimData
    {
        public string retailer_id;
        public int retailer_balance;
    }

    public class Claim
    {
        public int status;
        public string message;
        public ClaimData data;
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class TicketData
    {
        public string retailer_id   ;
        public int retailer_balance;
    }

    public class Ticket
    {
        public int status      ;
        public string message  ;
        public TicketData data;
    }

    public class PoolPrizeData
    {
        public int pool_prize_percentage;
    }

    public class PoolPrize
    {
        public int status;
        public string message;
        public PoolPrizeData data;
    }



}
namespace FootballPrototype2
{
    public class BetData
    {
        public string retailer_id;
        public string barcode_no;
        public int cash_balance;
    }

    public class BetValidation
    {
        public int status;
        public string message;
        public BetData data;
    }


}
namespace FootballPrototype3
{
    public class Data
    {
        public int game_id;
        public string draw_time;
        public int win_no_A;
        public int win_no_B;
        public int win_no_C;
        public int win_no_D;
        public int win_no_E;
        public int win_no_F;
        public int win_no_G;
        public int win_no_H;
        public int win_no_I;
        public int win_no_J;
    }

    public class WinNo
    {
        public int status;
        public string message;
        public Data data;
    }
    public class RoundInfo
    {
        public string game_id;
        public string draw_time;
    }


}
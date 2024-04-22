public class Constant
{
    // URLs and APIs
    public static readonly string BASE_URL = "http://playpoints.in/FunCricketGameplan/public/api";
    public static readonly string LOGIN_URL = BASE_URL + "/login";
    public static readonly string LOGOUT_URL = BASE_URL + "/logout";
    public static readonly string CHANGE_PASSWORD_URL = BASE_URL + "/change_password";
    public static readonly string CURRENT_ROUND_URL = BASE_URL + "/current_round";
    public static readonly string GET_TIMER_URL = BASE_URL + "/game_timer";
    public static readonly string SHOWCURRENT_BET_URL = BASE_URL + "/add_showcurrent_bet";
    public static readonly string WIN_NO_URL = BASE_URL + "/GameWinNo";
    public static readonly string CANCLE_TICKET_URL = BASE_URL + "/cancelTicket";
    public static readonly string CLAIM_TICKET_URL = BASE_URL + "/claimTicket";
    public static readonly string GAME_HISTORY_URL = BASE_URL + "/GameHistory";
    public static readonly string REPORT_URL = BASE_URL + "/RetailerSaleReport";
    public static readonly string REPRINT_URL = BASE_URL + "/reprintTickets";
    public static readonly string POOL_PRIZE_URL = BASE_URL + "/gameSevices";


    public static readonly string PROFILE_URL = BASE_URL + "/profile";
    public static readonly string FORCE_LOGIN_TRANSFER_URL = BASE_URL + "/update_device_id";
    public static readonly string RECEIVABLES_URL = BASE_URL + "/notification ";
    public static readonly string TRANSFERABLES_URL = BASE_URL + "/sender_notification";
    public static readonly string ACCEPT_POINTS_URL = BASE_URL + "/accept_points";
    public static readonly string SEND_POINTS_URL = BASE_URL + "/send_point_to_user";
    public static readonly string REJECT_POINTS_URL = BASE_URL + "/reject_points";
    public static readonly string CHANGE_PIN_URL = BASE_URL + "/logout";
    public static readonly string ADD_SHOWCURRENT_BET_URL = BASE_URL + "/add_showcurrent_bet";
    public static readonly string LAST_BET_DATA_URL = BASE_URL + "/lastBetUser";
    public static readonly string ADD_WIN_AMOUNT_URL = BASE_URL + "/addWinAmount";


    // Data Keys
    public const string EMAIL_DATA_KEY = "EMAIL";
    public const string PASSWORD_DATA_KEY = "PASSWORD";
    public static readonly string IS_INVALID_USER = "401";

    public const string PASSWORD_NOT_MATCHED = "Password not matched!\nPlease enter same Password and retry.";
    public const string PASSWORD_UPDATE_COMPLETE = "Password Changed Successfully.";

}

using System;

namespace TUMCampusAppAPI
{
    public class Consts
    {
        public const string CALENDAR_NAME = "TUM Online";

        public const string JSON_ADDRESS = "address";
        public const string JSON_LOCATION = "location";
        public const string JSON_CANTEEN_ID = "canteen_id";
        public const string JSON_LINK = "link";
        public const string JSON_NAME = "name";
        public const string JSON_INGREDIENTS = "ingredients";
        public const string JSON_DISH_TYPE= "dish_type";
        public const string JSON_LATITUDE = "latitude";
        public const string JSON_LONGITUDE = "longitude";
        public const string JSON_NEWS = "news";
        public const string JSON_DATE = "date";
        public const string JSON_TITLE = "title";
        public const string JSON_IMAGE = "image";
        public const string JSON_CREATED = "created";
        public const string JSON_SRC = "src";
        public const string JSON_ICON = "icon";
        public const string JSON_SOURCE = "source";
        public const string JSON_RAEUME = "raeume";
        public const string JSON_GRUPPEN = "gruppen";

        public const string NEWS_URL = "https://tumcabe.in.tum.de/Api/news/";
        public const string CANTEENS_URL = "https://home.in.tum.de/~sauterf/dist/canteens.json";
        public const string MENUS_URL = "https://home.in.tum.de/~sauterf/dist/all_ref.json";
        public const string NEWS_SOURCES_URL = NEWS_URL + "sources";
        public const string GOOGLE_IMAGES_SEARCH_STRING_START = "https://www.google.com/search?hl=en&as_st=y&site=imghp&tbm=isch&source=hp&biw=1502&bih=682&q=";
        public const string STUDYROOM_URL = "https://www.devapp.it.tum.de/iris/ris_api.php?format=json";

        public const string GITHUB_ISSUES = "https://github.com/COM8/UWP-TUM-Campus-App/issues";
        public const string GITHUB_PROJECT = "https://github.com/COM8/UWP-TUM-Campus-App";
        public const string GITHUB_LICENSE = "https://github.com/COM8/UWP-TUM-Campus-App/blob/master/LICENSE";
        public const string GITHUB_PRIVACY_POLICY = "https://github.com/COM8/UWP-TUM-Campus-App/blob/master/PRIVACY_POLICY.md";

        public const string PREFERENCE_SCREEN = "preference_screen";
        public const string P_TOKEN = "pToken";
        public const string P_MONTH_AHEAD = "pMonateVor";
        public const string P_MONTH_BACK = "pMonateNach";
        public const string P_TOKEN_NAME = "pTokenName";
        public const string P_USER_NAME = "pUsername";
        public const string P_SEARCH = "pSuche";
        public const string P_LV_NR = "pLVNr";

        // Validity's for entries in seconds:
        public const int VALIDITY_DO_NOT_CACHE = 0;
        public const int VALIDITY_ONE_HOUR = 360;
        public const int VALIDITY_THREE_HOURS = VALIDITY_ONE_HOUR * 3;
        public const int VALIDITY_ONE_DAY = VALIDITY_ONE_HOUR * 24;
        public const int VALIDITY_TWO_DAYS = VALIDITY_ONE_DAY * 2;
        public const int VALIDITY_FIFE_DAYS = VALIDITY_ONE_DAY * 5;
        public const int VALIDITY_ONE_WEEK = VALIDITY_ONE_DAY * 7;
        public const int VALIDITY_TEN_DAYS = VALIDITY_ONE_DAY * 10;
        public const int VALIDITY_ONE_MONTH = 30 * 86400;

        public static readonly string[] EDUROAM_NAME_EXTENSIONS = { "@campus.lmu.de", "@eduroam.mwn.de" };

        public static readonly Uri Square44x44Logo = new Uri("ms-appx:///Assets/Square44x44Logo.png");
        public static readonly Uri Square150x150Logo = new Uri("ms-appx:///Assets/Square150x150Logo.png");
        public static readonly Uri Wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.png");
        public static readonly Uri Square310x310Logo = new Uri("ms-appx:///Assets/Square310x310Logo.png");

        public const string TILE_ID_CANTEEN = "canteenTile";
    }
}

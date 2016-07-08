
namespace CAM.Core.MVC
{
    public partial class _Controller_ForOnlineStatus
    {
        
        #region 常量定义


        
        public enum EM_LOGIN_FROM
        {
            PC,
            MOBILE_IOS,
            MOBILE_ANDROID,
            PAD_IOS,
            PAD_ANDROID,
        }

        public enum EM_LOGIN_PASSTYPE
        {
            PASSWORD,
            SMSPASSWORD,
        }

        #endregion






        #region 通用函数：转换字符串和ENUM值

        private string convertEMLoginFromToString(EM_LOGIN_FROM from)
        {
            string str = "";
            switch (from)
            {
                case EM_LOGIN_FROM.PC:
                    str = "pc";
                    break;
                case EM_LOGIN_FROM.MOBILE_IOS:
                    str = "mobile_ios";
                    break;
                case EM_LOGIN_FROM.MOBILE_ANDROID:
                    str = "mobile_android";
                    break;
                case EM_LOGIN_FROM.PAD_IOS:
                    str = "pad_ios";
                    break;
                case EM_LOGIN_FROM.PAD_ANDROID:
                    str = "pad_android";
                    break;
                default:
                    str = "pc";
                    break;

            }
            return str;
        }
        private EM_LOGIN_FROM convertStringToEMLoginFrom(string value)
        {
            value = value.Trim().ToLower();
            EM_LOGIN_FROM from;
            switch (value)
            {
                case "pc":
                    from = EM_LOGIN_FROM.PC;
                    break;
                case "mobile_ios":
                    from = EM_LOGIN_FROM.MOBILE_IOS;
                    break;
                case "mobile_android":
                    from = EM_LOGIN_FROM.MOBILE_ANDROID;
                    break;
                case "pad_ios":
                    from = EM_LOGIN_FROM.PAD_IOS;
                    break;
                case "pad_android":
                    from = EM_LOGIN_FROM.PAD_ANDROID;
                    break;
                default:
                    from = EM_LOGIN_FROM.PC;
                    break;
            }
            return from;
        }

        #endregion

    }
}

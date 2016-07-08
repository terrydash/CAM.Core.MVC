
namespace CAM.Core.MVC
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using CAM.Common.Cache;
    using CAM.Common.Net.Cookie;
    using CAM.Common.Error;
    
    using CAuth.Model.Entity;
    using CAuth.Business.Interface;
    using CAuth.Business.Aggregate;

    

    public partial class _Controller_ForOnlineStatus
    {


        #region 登陆、状态检测、退出的三个接口函数。派生Controller中通过这三个函数来处理登陆在线退出事件


        /// <summary>
        /// 用户登陆操作
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="Password"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        protected UserIdentity loginCheck(string LoginName, string Password, EM_LOGIN_FROM from)
        {
            try
            {
                /*
                 * 尝试使用账号密码登陆，如果成功，则返回用户身份结构
                 * 身份信息里包含Passport部分的信息
                 */
                string oldToken = "", newToken = "";
                UserIdentity identity = formatUserIdentity(LoginName, Password, from, EM_LOGIN_PASSTYPE.PASSWORD, ref oldToken, ref newToken);

                /*
                 * 根据当前登陆的不同系统，格式化不同的身份信息
                 */
                fillUserIdentityInfoBySys(identity);

                /*
                 * 将当前用户身份信息存入缓存服务器，用于保持用户的在线状态
                 */
                insertIdentityIntoCache(identity, oldToken, newToken);

                /*
                 * 将用户在线状态判定所需的信息存入cookie，确保客户端不会因为超时掉线
                 */
                CookieHandler.set("token", newToken);
                CookieHandler.set("from", convertEMLoginFromToString(from));

                return identity;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
                return null;
            }
        }

        /// <summary>
        /// 用户登陆操作：利用短信验证码
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="SMSPassword"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        protected UserIdentity loginCheckWithSMS(string LoginName, string SMSPassword, EM_LOGIN_FROM from)
        {
            try
            {
                /*
                 * 尝试使用账号密码登陆，如果成功，则返回用户身份结构
                 * 身份信息里包含Passport部分的信息
                 */
                string oldToken = "", newToken = "";
                UserIdentity identity = formatUserIdentity(LoginName, SMSPassword, from, EM_LOGIN_PASSTYPE.SMSPASSWORD, ref oldToken, ref newToken);

                /*
                 * 根据当前登陆的不同系统，格式化不同的身份信息
                 */
                fillUserIdentityInfoBySys(identity);

                /*
                 * 将当前用户身份信息存入缓存服务器，用于保持用户的在线状态
                 */
                insertIdentityIntoCache(identity, oldToken, newToken);

                /*
                 * 将用户在线状态判定所需的信息存入cookie，确保客户端不会因为超时掉线
                 */
                CookieHandler.set("token", newToken);
                CookieHandler.set("from", convertEMLoginFromToString(from));

                return identity;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
                return null;
            }
        }

        /// <summary>
        /// 检测在线状态，如果发现不在线，尝试利用cookie自动重新登陆
        /// </summary>
        /// <returns></returns>
        protected UserIdentity checkUserOnline()
        {
            try
            {
                UserIdentity identity = reFormatUserIdentity();
                fillUserIdentityInfoBySys(identity);
                return identity;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
                return null;
            }
        }

        /// <summary>
        /// 退出系统时清空缓存和cookie
        /// </summary>
        protected void clearUserIdentityAndLogOff()
        {
            string token = CookieHandler.read("token");
            if (string.IsNullOrWhiteSpace(token)) token = Request["token"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                clearIdentityInCache(token);
            }

            CookieHandler.remove("token");
            CookieHandler.remove("from");

        }


        #endregion










        #region 进入数据层检查账号合法性

        private long loginWithPassport(string LoginName, string Password, EM_LOGIN_FROM from, ref string oldToken, ref string newToken)
        {
            try
            {
                ICAuthPassportCommand ic = new CAuth.Business.Aggregate.Aggregate();
                long passportId = 0;

                switch (from)
                {
                    case EM_LOGIN_FROM.PC:
                        passportId = ic.loginFromPC(LoginName, Password, ref oldToken, ref newToken);
                        break;
                    case EM_LOGIN_FROM.MOBILE_IOS:
                        passportId = ic.loginFromMobileIOS(LoginName, Password, ref oldToken, ref newToken);
                        break;
                    case EM_LOGIN_FROM.MOBILE_ANDROID:
                        passportId = ic.loginFromMobileAndroid(LoginName, Password, ref oldToken, ref newToken);
                        break;
                    case EM_LOGIN_FROM.PAD_IOS:
                        passportId = ic.loginFromPadIOS(LoginName, Password, ref oldToken, ref newToken);
                        break;
                    case EM_LOGIN_FROM.PAD_ANDROID:
                        passportId = ic.loginFromPadAndroid(LoginName, Password, ref oldToken, ref newToken);
                        break;
                    default:
                        passportId = 0;
                        break;
                }

                return passportId;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
                return 0;
            }
        }

        private long loginWithToken(string token)
        {
            try
            {
                ICAuthPassportCommand ic = new CAuth.Business.Aggregate.Aggregate();
                long passportId = ic.loginByToken(token);
                return passportId;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
                return 0;
            }
        }

        private long loginWithSMSPassport(string LoginName, string SMSPassword, EM_LOGIN_FROM from, ref string oldToken, ref string newToken)
        {
            try
            {
                ICAuthPassportCommand ic = new CAuth.Business.Aggregate.Aggregate();
                long passportId = 0;

                switch (from)
                {
                    case EM_LOGIN_FROM.PC:
                        passportId = ic.loginFromPCWithSMSPassword(LoginName, SMSPassword, ref oldToken, ref newToken);
                        break;
                    case EM_LOGIN_FROM.MOBILE_IOS:
                        passportId = ic.loginFromMobileIOSWithSMSPassword(LoginName, SMSPassword, ref oldToken, ref newToken);
                        break;
                    case EM_LOGIN_FROM.MOBILE_ANDROID:
                        passportId = ic.loginFromMobileAndroidWithSMSPassword(LoginName, SMSPassword, ref oldToken, ref newToken);
                        break;
                    case EM_LOGIN_FROM.PAD_IOS:
                        passportId = ic.loginFromPadIOSWithSMSPassword(LoginName, SMSPassword, ref oldToken, ref newToken);
                        break;
                    case EM_LOGIN_FROM.PAD_ANDROID:
                        passportId = ic.loginFromPadAndroidWithSMSPassword(LoginName, SMSPassword, ref oldToken, ref newToken);
                        break;
                    default:
                        passportId = 0;
                        break;
                }

                return passportId;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
                return 0;
            }
        }


        #endregion






        



    }
}



namespace CAM.Core.MVC
{

    using System;
    using System.Web;
    using CAM.Common.Error;
    using CAM.Common.Net.Cookie;

    using CAuth.Model.Entity;
    using CAuth.Business.Interface;
    using CAuth.Business.Aggregate;

    using CUser.Model.Entity;
    using CUser.Business.Interface;
    using CUser.Business.Aggregate;

    using CRole.Model.Entity;
    using CRole.Business.Interface;
    using CRole.Business.Aggregate;

    public partial class _Controller_ForOnlineStatus
    {


        private const string _C_ERR_LOGINFAILD = "登录失败";
        private const string _C_ERR_USERNOTLOGINED = "请登录系统后再进行操作";
        private const string _C_ERR_USERIDENTITYLOST = "身份信息丢失，需要重新登录";

        /// <summary>
        /// 尝试使用账号密码登陆，成功后返回身份信息
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="Password"></param>
        /// <param name="from"></param>
        /// <param name="passType"></param>
        /// <param name="oldToken"></param>
        /// <param name="newToken"></param>
        /// <returns></returns>
        private UserIdentity formatUserIdentity(string LoginName,
                                                string Password,
                                                EM_LOGIN_FROM from,
                                                EM_LOGIN_PASSTYPE passType,
                                                ref string oldToken,
                                                ref string newToken)
        {
            try
            {
                long passportId = 0;
                switch (passType)
                {
                    case EM_LOGIN_PASSTYPE.PASSWORD:
                        passportId = loginWithPassport(LoginName, Password, from, ref oldToken, ref newToken);
                        break;
                    case EM_LOGIN_PASSTYPE.SMSPASSWORD:
                        passportId = loginWithSMSPassport(LoginName, Password, from, ref oldToken, ref newToken);
                        break;
                }

                if (passportId == 0)
                {
                    ErrorHandler.ThrowException(_C_ERR_LOGINFAILD);
                    return null;
                }

                UserIdentity identity = new UserIdentity()
                {
                    PassportInfo = new UserIdentity_PassportInfo()
                    {
                        LoginName = LoginName,
                        PassportId = passportId,
                        Token = newToken,
                    }
                };
                return identity;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
                return null;
            }
        }

        /// <summary>
        /// 尝试通过token从cache中提取身份信息
        /// 如果cache中提取不到，则利用token尝试
        /// 自动重新登录过程提取身份信息
        /// </summary>
        /// <returns></returns>
        private UserIdentity reFormatUserIdentity()
        {
            try
            {
                string token = CookieHandler.read("token");
                if (string.IsNullOrWhiteSpace(token)) token = Request["token"];
                if (string.IsNullOrWhiteSpace(token))
                {
                    ErrorHandler.ThrowException(_C_ERR_USERNOTLOGINED);
                    return null;
                }

                string fromStr = CookieHandler.read("from");
                if (string.IsNullOrWhiteSpace(fromStr)) fromStr = Request["from"];

                EM_LOGIN_FROM from = convertStringToEMLoginFrom(fromStr);

                object identityCache = readIdentityFromCache(token);
                UserIdentity identity = null;

                if (identityCache == null)
                {
                    long passportId = loginWithToken(token);
                    if (passportId == 0)
                    {
                        //ErrorHandler.ThrowException("身份信息丢失，需要重新登录");
                        ErrorHandler.ThrowException(_C_ERR_USERIDENTITYLOST);
                        return null;
                    }
                    identity = new UserIdentity()
                    {
                        PassportInfo = new UserIdentity_PassportInfo()
                        {
                            PassportId = passportId,
                        },
                    };
                    formatUserIdentity_PassportInfo(identity, passportId, from);
                    return identity;
                }
                else
                {
                    identity = (UserIdentity)identityCache;
                    return identity;
                }

            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
                return null;
            }
        }







        private void formatUserIdentity_PassportInfo(UserIdentity identity, long passportId, EM_LOGIN_FROM from)
        {
            try
            {

                ICAuthPassportCommand ic = new CAuth.Business.Aggregate.Aggregate();
                CAuthPassport passport = ic.readPassport(passportId);

                UserIdentity_PassportInfo info = new UserIdentity_PassportInfo()
                {
                    LoginName = passport.LoginName,
                    PassportId = passport.Id,
                };

                switch (from)
                {
                    case EM_LOGIN_FROM.PC:
                        info.Token = passport.Token.PC.ToString();
                        break;
                    case EM_LOGIN_FROM.MOBILE_IOS:
                        info.Token = passport.Token.MobileIOS.ToString();
                        break;
                    case EM_LOGIN_FROM.MOBILE_ANDROID:
                        info.Token = passport.Token.MobileAndroid.ToString();
                        break;
                    case EM_LOGIN_FROM.PAD_IOS:
                        info.Token = passport.Token.PadIOS.ToString();
                        break;
                    case EM_LOGIN_FROM.PAD_ANDROID:
                        info.Token = passport.Token.PadAndroid.ToString();
                        break;
                    default:
                        info.Token = "";
                        break;
                }

                identity.PassportInfo = info;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
            }
        }


        
        private void formatUserIdentity_UserInfo(UserIdentity identity)
        {
            try
            {
                if (identity.UserInfo != null) return;
                if (identity.PassportInfo.PassportId == 0) return;
                ICUserInfoCommand ic = new CUser.Business.Aggregate.Aggregate();
                CUserInfo user = ic.readUserInfoByPassport(identity.PassportInfo.PassportId);

                UserIdentity_UserInfo info = new UserIdentity_UserInfo()
                {
                    UserId = user.Id,
                    Name = user.NameStruct.Name,
                };
                identity.UserInfo = info;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
            }
        }





    }
}

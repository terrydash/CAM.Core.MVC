

namespace CAM.Core.MVC
{
    using System;
    using CAM.General.SessionModel;
    using CAM.Common.Cache;
    using CAM.Common.Error;

    public partial class _Controller_ForOnlineStatus
    {

        private const string _C_Identity_Cache_ConfigFile = "IdentityOnline";

        /// <summary>
        /// 登陆后将身份信息存放到cache中
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="oldToken"></param>
        /// <param name="newToken"></param>
        private void insertIdentityIntoCache(UserIdentity identity, string oldToken, string newToken)
        {
            try
            {
                ICache ic = CacheFactory.createMemcached(_C_Identity_Cache_ConfigFile);
                ic.delete(oldToken);
                ic.add(newToken, identity);
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
            }

            
        }


        private object readIdentityFromCache(string token)
        {
            try
            {
                ICache icc = CacheFactory.createMemcached(_C_Identity_Cache_ConfigFile);
                object identityCache = icc.get(token);
                return identityCache;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
                return null;
            }
        }


        private void clearIdentityInCache(string token)
        {
            try
            {
                ICache ic = CacheFactory.createMemcached(_C_Identity_Cache_ConfigFile);
                ic.delete(token);
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
            }
        }
    }
}

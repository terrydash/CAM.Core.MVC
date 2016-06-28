
namespace CAM.Core.MVC
{
    using System;
    using System.Web;
    using CAM.General.SessionModel;
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


        #region 格式化用户身份信息：需要不断扩展

        /// <summary>
        /// 这个函数可以根据当前用户登陆的不同模块，自动格式化其所需的不同身份结构
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="sys"></param>
        private void fillUserIdentityInfoBySys(UserIdentity identity)
        {
            try
            {
                formatUserIdentity_UserInfo(identity);
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
            }
        }

        #endregion


       
    }
}

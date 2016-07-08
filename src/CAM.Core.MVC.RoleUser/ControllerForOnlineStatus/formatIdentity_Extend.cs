
namespace CAM.Core.MVC
{
    using System;
    using System.Web;
    using CAM.Common.Error;
    using CAM.Common.Net.Cookie;

    using CRole.Model.Entity;
    using CRole.Business.Interface;
    using CRole.Business.Aggregate;

    using CMapping.Model.Entity;
    using CMapping.Business.Interface;
    using CMapping.Business.Aggregate;

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
                formatUserIdentity_RoleInfo(identity);
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
            }
        }

        #endregion







        /// <summary>
        /// 格式化 CRole.Model.Entity.CRoleClass 类型信息
        /// 针对具备角色的用户
        /// </summary>
        /// <param name="identity"></param>
        private void formatUserIdentity_RoleInfo(UserIdentity identity)
        {
            try
            {
                ICMappingRoleUserCommand icmap = new CMapping.Business.Aggregate.Aggregate();
                CMappingRoleUser rumap = icmap.readMappingRoleUserByUser(identity.UserInfo.UserId);

                identity.RoleInfo = new UserIdentity_RoleInfo()
                {
                    Id = rumap.RoleId,
                };
                
                ICRoleCommand ic = new CRole.Business.Aggregate.Aggregate();
                CRoleRole role = ic.readRole(identity.RoleInfo.Id);

                if (string.IsNullOrWhiteSpace(role.PopedomIdList))
                {
                    ErrorHandler.ThrowException("用户所属角色没有被授予任何权限");
                }

                identity.RoleInfo.Name = role.Name;
                identity.RoleInfo.PopedomKeys = ic.readRolePopedomKeys(role.PopedomIdList);
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowException(ex);
            }
        }


    }
}

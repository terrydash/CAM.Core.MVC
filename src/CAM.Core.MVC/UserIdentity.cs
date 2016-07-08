using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Core.MVC
{
    /// <summary>
    /// 用户身份信息
    /// </summary>
    [Serializable]
    public class UserIdentity
    {
        public UserIdentity_PassportInfo PassportInfo { get; set; }

        public UserIdentity_UserInfo UserInfo { get; set; }

        public UserIdentity_RoleInfo RoleInfo { get; set; }

        public UserIdentity_OrganizationInfo OrgInfo { get; set; }
    }

    [Serializable]
    public class UserIdentity_PassportInfo
    {
        public long PassportId { get; set; }
        public string LoginName { get; set; }
        public string Token { get; set; }
        public UserIdentity_PassportInfo()
        {
            PassportId = 0;
            LoginName = "";
            Token = "";
        }
    }

    [Serializable]
    public class UserIdentity_UserInfo
    {
        public long UserId { get; set; }
        public string Name { get; set; }

        public UserIdentity_UserInfo()
        {
            UserId = 0;
            Name = "";
        }
    }

    [Serializable]
    public class UserIdentity_RoleInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PopedomKeys { get; set; }

        public UserIdentity_RoleInfo()
        {
            Id = 0;
            Name = "";
            PopedomKeys = "";
        }
    }

    [Serializable]
    public class UserIdentity_OrganizationInfo
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public UserIdentity_OrganizationInfo()
        {
            OrganizationId = 0;
            OrganizationName = "";
            BranchId = 0;
            BranchName = "";
            DepartmentId = 0;
            DepartmentName = "";
        }
    }


}

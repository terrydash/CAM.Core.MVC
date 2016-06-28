/*
    这个文件只是作为Controller虚基类的派生流程设定，
    本身不包含任何功能代码，利用partial标识，将每个层次的派生类进行拆分，
    派生过程在这里描述，具体各层实现的功能，在相应的代码文件中去实现。
*/

namespace CAM.Core.MVC
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    #region Controller基类，继承出一个全新的Controller，本身不实现任何方法
    public abstract partial class _BaseController : Controller
    {
        //一个抽象的虚基类，不要在这里实现任何东西。
        //所有需要实现的特征及方法，应该在特定的层次下去具体实现。
    }
    #endregion

    #region 用户在线状态维持及检测层
    public abstract partial class _Controller_ForOnlineStatus : _BaseController
    {
        /*
            检测用户在线状态，尝试利用cookie自动重登陆及各个子站点之间的一站式登陆
        */
    }
    #endregion

    #region 用户权限检测层
    public abstract partial class _Controller_ForPopedomCheck : _Controller_ForOnlineStatus
    {
        /*
            检测用户操作API的具体权限，确保用户不会操作到非授权数据
        */
    }
    #endregion

    #region 数据筛选适配层
    public abstract partial class _Controller_ForFilter : _Controller_ForPopedomCheck
    {
        /*
            针对Model.Filter的定义，实现C端回传参数的Filter化过程 
        */
    }
    #endregion




    #region Controller派生终结层
    public abstract class _Controller_EndPoint : _Controller_ForFilter
    {
        /*
            这一层只是一个标志，表明Controller的派生过程到此结束，所有需要增加的层次均需要添加到这一层之前。
            在CAMController对象上，不用知道基类具体的派生流程，只需要从此节点层继承即可。
        */
    }
    #endregion
}

using System.ComponentModel;

namespace Jg.wpf.core.Extensions.Types.RoiTypes
{
    /// <summary>
    /// 限制移动
    /// </summary>
    public enum RoiRestrictedTypes
    {
        [Description("NoRestrict")]
        None,

        /// <summary>
        /// 限制 x 方向移动
        /// </summary>
        [Description("RestrictX")]
        X,

        /// <summary>
        /// 限制 y 方向移动
        /// </summary>
        [Description("RestrictY")]
        Y,
    }
}

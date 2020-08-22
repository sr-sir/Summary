using System;

namespace Sino.LogisticsWorkOrderManage.Core
{
    public static class ErrorCode
    {
        public const string E10000 = "用户没有登陆";
        public const string E10002 = "没有访问权限";
        public const string E10003 = "该用户被禁用";
        public const string E10004 = "不存在";
        public const string E10005 = "不能为空";
        public const string E10006 = "已经存在";
        public const string E10007 = "验证Token出错";
        public const string E10012 = "密码错误或用户不存在";
        public const string E10013 = "该用户不存在";
        public const string E10014 = "原密码错误";
        public const string E10015 = "请检查参数是否正确";
        

        public const string E30001 = "数据操作异常";

        public const string E30003 = "地址信息错误";
        public const string E30016 = "该工单不存在";
        public const string E30017 = "该工单处于“{0}”状态，不能执行指派操作";
        public const string E30018 = "该工单处于“{0}”状态，不能执行取消工单操作";
        public const string E30019 = "该工单处于“{0}”状态，不能执行领单操作";
        public const string E30020 = "该工单处于“{0}”状态，不能执行转派操作";
        public const string E30021 = "该工单处于“{0}”状态，不能执行受理操作";
    }
}

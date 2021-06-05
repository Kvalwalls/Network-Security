namespace CommonUser.Entity
{
    //服务类型枚举
    public class EnumServiceType
    {
        //AS服务类型
        public const byte AS = 0;
        //TGS服务类型
        public const byte TGS = 1;
        //管理员应用服务
        public const byte AUV = 2;
        //用户应用服务
        public const byte CUV = 3;
    }

    //Kerberos服务具体类型枚举
    public class EnumKerberos
    {
        //错误
        public const byte Error = 0;
        //请求
        public const byte Request = 1;
        //回复
        public const byte Reply = 2;
        //结束
        public const byte End = 3;
    }

    //CommonUser服务具体类型枚举
    public class EnumCUV
    {
        //登录
        public const byte Login = 0;
        //注册
        public const byte Register = 1;
        //找回密码
        public const byte Refind = 2;
        //修改名称
        public const byte ModifyName = 3;
        //修改密码
        public const byte ModifyPassword = 4;
    }

    //加密码枚举
    public class EnumCryptCode
    {
        //明文
        public const byte NoCrypt = 0;
        //密文
        public const byte Crypt = 1;
    }

    //错误码枚举
    public class EnumErrorCode
    {
        //无错误
        public const byte NoError = 0;
        //有错误
        public const byte Error = 1;
    }

    //用户权限枚举
    public class EnumUserAccess
    {
        //超级管理员权限
        public const byte A_Root = 0;
        //普通管理员权限
        public const byte A_Comm = 1;
        //普通用户权限
        public const byte U_Comm = 2;
        //VIP用户权限
        public const byte U_VIP = 3;
        //SVIP用户权限
        public const byte U_SVIP = 4;
    }

    //座位状态枚举
    public class EnumSeatStatus
    {
        //未选中状态
        public const byte Unselected = 0;
        //已选中状态
        public const byte Selected = 1;
        //选中未支付状态
        public const byte Selecting = 2;
    }

    //影厅类型枚举
    public class EnumTheaterType
    {
        //普通影厅
        public const byte Comm = 0;
        //VIP影厅
        public const byte VIP = 1;
        //SVIP影厅
        public const byte SVIP = 2;
    }

    //购票状态枚举
    public class EnumRecordStatus
    {
        //等待支付状态
        public const byte Waiting = 0;
        //购票成功状态
        public const byte Success = 1;
        //购票失败状态
        public const byte Failed = 2;
    }
}
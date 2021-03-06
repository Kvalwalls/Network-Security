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
        //获取个人信息
        public const byte GetUser = 3;
        //修改名称
        public const byte ModifyName = 4;
        //修改密码
        public const byte ModifyPassword = 5;
        //升级权限
        public const byte UpgradeAccess = 6;
        //金额充值
        public const byte Recharge = 7;
        //获取所有影片信息
        public const byte GetMovies = 8;
        //获取所有影片图片
        public const byte GetMoviePictures = 9;
        //获取所有购票记录
        public const byte GetRecords = 10;
        //获取某影片所有场次信息
        public const byte GetOnMovies = 11;
        //获取某场次所有座位信息
        public const byte GetSeats = 12;
        //获取某场次影厅信息
        public const byte GetTheater = 13;
        //选择座位
        public const byte SelectSeat = 14;
        //支付金额
        public const byte PayMoney = 15;
        //支付超时
        public const byte PayTimeout = 16;
        //退票
        public const byte RefundRecord = 17;
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
        //选中未支付状态
        public const byte Selecting = 1;
        //已选中状态
        public const byte Selected = 2;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashBoxTest
{
    public enum RequestCommand
    {
        SYNC = 0XFC,
        STATUSREQUEST = 0X11,
        ACK = 0x50,

        RESET = 0x40,
        STACK_1 = 0x41,
        STACK_2 = 0x42,
        RETURN = 0x43,
        HOLD = 0x44,
        WAIT = 0x45,

        ENABLEMONEY = 0xC0,//规定币种
        SECURITY = 0xC1,
        COMMUNICATION_MODB = 0xC2,
        INHIBIT = 0xC3,//启动收入纸币状态 00启动 01关闭
        DIRECTION = 0xC4,
        OPTIONAL_FUNCTION = 0xC4,


        GETENABLEMONEY = 0x80,
        GETSECURITY = 0x81,
        GETCOMMUNICATION_MODB = 0x82,
        GETINHIBIT = 0x83,
        GETDIRECTION = 0x84,
        GETVERSION_REQUEST = 0x88,
        GETBOOT_VERSION_REQUEST = 0x89,
        GETOPTIONAL_FUNCTION = 0x85,
        GET_CURRENCY_ASSIGN_REQUEST = 0x84,
    }
    public enum ResponseCommand
    {
        //返回
        ACK = 0x50,
        IDLING = 0x11,
        ACCEPTING = 0x12,
        ESCROW = 0x13,
        STACKING = 0x14,
        VEND_VALID = 0x15,
        STACKED = 0x16,
        REJECTING = 0x17,
        RETURNING = 0x18,
        HOLDING = 0x19,
        DISABLE = 0x1A,//不收钱（inhibit）状态
        INHIBIT = 0xC3,//设置收钱状态返回之后
        INITIALIZB = 0x1B,
        POWER_UP = 0x40,
        POWERUP_WITH_BILL_IN_ACCEPTOR = 0x41,
        POWERUP_WITH_BILL_IN_STACKER = 0x42,
        STACKER_FULL = 0x43,//钱箱满了
        STACKER_OPEN = 0x44,//钱箱未打开
        JAM_IN_ACCEPTOR = 0x45,//acceptor内部阻塞
        JAM_IN_STACKER = 0x46,//钱箱堵塞
        PAUSE = 0x47,
        CHEATED = 0x48,
        FAILURE = 0x49,
        COMMUNICATION_ERRO = 0x4A,//通信错误
        INVALID_COMMAND = 0x4B,//不是有效指令
    }
    public enum MoneyCommand
    {
        Five = 0x63,
        Ten = 0x64,
        Twenty = 0x65,
        Fifty = 0x66,
        Hundred = 0x67,
    }
}

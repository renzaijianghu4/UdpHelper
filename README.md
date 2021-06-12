#  UdpHelper
## （UDP简单封装，简单调用）

## 水平线
---
***


其实UDP在.NET中已经比较简单，且并不区分服务端和客户端，
但为了便于实际使用开发，特此做出以下封装。（业余作娱乐，诸公多鞭策）
## Server

+ Start()			启动监听，返回监听结果：false为失败(建议更换端口号)

+ Stop()			停止监听，不返回结果

+ IsListenRun		当前端口监听状态，true:监听中，false:未监听

+ ReceiveEvent    监听数据委托处理事件，使用方法：ReceiveEvent += func;   格式为 private[隐私类别] [返回类型] func(byte[] data)


#####   服务端启用举例：(示意)
```
    using UdpHelper;

    Server server = new Server(12345);
    server.Start();
    ReceiveEvent+= DealData;//同Start()的顺序不严格要求

    //简单转化为文本并显示，如果处理控件，请检查或添加CheckForIllegalCrossThreadCalls=false;
    private void DealData(byte[] data)
    { MessageBox.Show( Encoding.Default.GetString(data));}
```
## Client
+ static Send(IPEndPoint dstIep, byte[] data)
+ static Send(IPEndPoint dstIep, string Msg)
+ static bool Send(string dstIep, byte[] data)
+ static bool Send(string dstIep, string msg)

以上均为发送UDP数据，区别主要为目的主机的格式和发送内容的不同，dstIep使用string类型时格式为"127.0.0.1:12345"

Client不需要实例化，直接调用即可

#####   客户端启用举例：(示意)
```
    using UdpHelper;

    Client.Send("127.0.0.1:12345","This is a test message!);

```
如果觉得对你有帮助，我会感到十分荣幸，如果感到无用或有其他建议可联系867189791@qq.com



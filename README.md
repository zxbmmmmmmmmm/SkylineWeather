# FluentWeather

一个UWP天气应用，用于替代系统自带的MSN Weather


## 屏幕截图
![image](https://github.com/zxbmmmmmmmmm/FluentWeather/assets/96322503/89c40c71-69ed-441b-b3f2-84eaceadef42)

![image](https://github.com/zxbmmmmmmmmm/FluentWeather/assets/96322503/342a414d-1634-45ae-bea5-8c87e76da025)

![image](https://github.com/zxbmmmmmmmmm/FluentWeather/assets/96322503/c08ccb55-b4fd-4985-a258-45828975f812)

![image](https://github.com/zxbmmmmmmmmm/FluentWeather/assets/96322503/ce991533-fc53-435f-aad4-f0511da90432)


## 功能

免费KEY支持的功能有:

- 实时天气
- 每日预报(7天)
- 每小时预报(24时)
- 分钟级降水预报(2小时)
- 天气预警
- 14项生活指数
- 日出日落时间
- 每日天气/预警通知
- 动态磁贴

如果你有付费KEY，还可以使用以下功能:

- 天气预报(30天)
- 每小时预报(192时)
- 台风追踪

## 获取KEY

默认情况下，FluentWeather需要手动输入KEY以从数据提供商获取数据。

目前可用的数据提供商有: 

- 和风天气(详情可见[和风天气开发服务](https://dev.qweather.com/))


### 免费KEY

*每天1000次请求次数，应该够用了*

1. 打开[和风天气开发服务](https://dev.qweather.com/)，点击**免费注册**，注册一个账户并登录控制台。

2. 点击侧边栏的**项目管理**，点击**创建项目**，随便输一个项目名称，选择**免费订阅**。在**设置KEY**一栏的**适用平台**中选择**Web API**，同样随便输入一个项目名称。完成后，点击**创建**。
   ![image](https://github.com/zxbmmmmmmmmm/FluentWeather/assets/96322503/f1350f8c-d77e-49e7-a67d-e2dbe56aa9ea)
   ![image](https://github.com/zxbmmmmmmmmm/FluentWeather/assets/96322503/d830dcc0-0896-4d3f-8a07-101da84be937)


4. 在项目管理中找到你刚刚创建的项目，点击**KEY**一栏中的**查看**，将弹出窗口中的KEY复制。
   ![image](https://github.com/zxbmmmmmmmmm/FluentWeather/assets/96322503/d0fbdfa2-ba2d-4060-890f-e9a07eb5d31a)
   ![image](https://github.com/zxbmmmmmmmmm/FluentWeather/assets/96322503/038e2e73-79b6-408f-9f0c-829f9edee561)


   > **注意**:不要随意泄露你的KEY！

6. 打开应用，转到**侧边栏-设置-和风天气**，将你获取到的KEY粘贴在KET 和GeoApiKEY中。
   ![image](https://github.com/zxbmmmmmmmmm/FluentWeather/assets/96322503/c0996547-cac2-4d3b-be6e-a60a0b980ab6)


   完成后重启应用，就可以正常使用FluentWeather了。

### 付费KEY

如果还需要更多高级功能，你还可以购买和风天气的付费KEY

> 将域名更改为 api.qweather.com 以使用付费KEY获取数据
> 
> ![image](https://github.com/zxbmmmmmmmmm/FluentWeather/assets/96322503/32bf04bc-f858-4615-961e-423df4e514e8)

KEY获取教程同上

需要充值一定金额并在创建项目时选择“标准订阅(按量计费)”

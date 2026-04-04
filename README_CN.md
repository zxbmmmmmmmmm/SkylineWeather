简体中文 | [English](README.md)

# Skyline 天气

Skyline 是一款UWP天气应用程序

![image](https://github.com/zxbmmmmmmmmm/FluentWeather/assets/96322503/2daf38d4-df5e-45c3-bd30-7f72cf0924e4)


<a href="https://apps.microsoft.com/detail/Skyline%20%E5%A4%A9%E6%B0%94/9n33pk9646x9?launch=true
	&mode=mini">
	<img src="https://get.microsoft.com/images/zh-cn%20dark.svg" width="200"/>
</a>


## 功能:

- 实时天气
- 每日预报 (7-30 天)
- 每小时预报 (24 小时)
- 分钟级降水预报 (2 小时)
- 历史天气
- 天气预警
- 14项生活指数（仅限和风天气）
- 日出与日落时间
- 每日天气 / 预警通知
- 动态磁铁
- 游戏栏小组件
- 锁屏天气

## 本地构建

你可以在 Windows 上使用提供的 PowerShell 脚本在本地构建发布包：

```powershell
# 构建所有架构（x86、x64、ARM64），并指定版本号
.\scripts\build-release.ps1 -Version 1.0.0.0

# 仅构建 x64 和 ARM64
.\scripts\build-release.ps1 -Version 1.0.0.0 -Architectures x64,ARM64

# 不修改 manifest 版本，构建所有架构
.\scripts\build-release.ps1
```

**环境要求：**
- Windows，已安装 Visual Studio 2022（或 2019），并包含**通用 Windows 平台开发**工作负荷。
- `msbuild` 已加入 `PATH`（例如在 *Visual Studio 开发者 PowerShell* 中运行），或安装在 Visual Studio 默认路径下。

输出包位于仓库根目录下的 `Package\<arch>\`。

## 帮助我们翻译
![Crowdin](https://badges.crowdin.net/fluent-weather/localized.svg)

你可以在这里翻译:

[![Crowdin](https://img.shields.io/badge/Crowdin-2E3340.svg?style=plastic&logo=Crowdin&logoColor=white)](https://zh.crowdin.com/project/fluent-weather)

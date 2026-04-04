[简体中文](README_CN.md) | English

# Skyline Weather

Skyline is a UWP weather application.

![image](https://github.com/zxbmmmmmmmmm/FluentWeather/assets/96322503/2daf38d4-df5e-45c3-bd30-7f72cf0924e4)


<a href="https://apps.microsoft.com/detail/Skyline%20%E5%A4%A9%E6%B0%94/9n33pk9646x9?launch=true
	&mode=mini">
	<img src="https://get.microsoft.com/images/en-us%20dark.svg" width="200"/>
</a>


## Features:

- Real time weather
- Daily forecast (7-30 days)
- Hourly forecast (24 hours)
- Minute level precipitation forecast (2 hours)
- Historical weather
- Weather warning
- 14 life indices (only Qweather)
- Sunrise and sunset time
- Daily weather/warning notifications
- Live tiles
- Game Bar widget
- Lock screen weather

## Local Build

You can build release packages locally on Windows using the provided PowerShell script:

```powershell
# Build all architectures (x86, x64, ARM64) with a specific version
.\scripts\build-release.ps1 -Version 1.0.0.0

# Build only x64 and ARM64
.\scripts\build-release.ps1 -Version 1.0.0.0 -Architectures x64,ARM64

# Build all architectures without changing the manifest version
.\scripts\build-release.ps1
```

**Requirements:**
- Windows with Visual Studio 2022 (or 2019) installed, including the **Universal Windows Platform development** workload.
- `msbuild` available on `PATH` (e.g. open a *Developer PowerShell for VS*), or installed at the default Visual Studio location.

Output packages are placed under `Package\<arch>\` in the repository root.

## Help us translate
![Crowdin](https://badges.crowdin.net/fluent-weather/localized.svg)

You can help us translate here:

[![Crowdin](https://img.shields.io/badge/Crowdin-2E3340.svg?style=plastic&logo=Crowdin&logoColor=white)](https://crowdin.com/project/fluent-weather)

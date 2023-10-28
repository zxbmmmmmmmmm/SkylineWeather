using System;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public class WindScaleToDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is not string scale ? null : GetWindDescription(scale);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
    public static string GetWindDescription(string scale)
    {
        if(scale.Contains("-"))
        {
            var s = scale.Split("-");
            scale = s[1];
        }
        if (scale == "0") return "静，烟直向上";
        if (scale == "1") return "烟能表示风向，风向标不转动";
        if (scale == "2") return "人面感觉有风，树叶有微响，风向标转动";
        if (scale == "3") return "树叶及小树枝摇动不息，旗展开";
        if (scale == "4") return "吹起地面灰尘和纸张，小树枝摇动";
        if (scale == "5") return "有叶的小树整棵摇摆；内陆水面有波纹";
        if (scale == "6") return "大树枝摇摆，持伞有困难，电线有呼呼声";
        if (scale == "7") return "全树摇动，人迎风前行有困难";
        if (scale == "8") return "小树枝折断，人向前行阻力甚大";
        if (scale == "9") return "烟囱顶部移动，木屋受损";
        if (scale == "10") return "大树连根拔起，建筑物损毁";
        if (scale == "11") return "陆上少见，建筑物普遍损毁";
        if (scale == "12") return "陆上少见，建筑物普遍严重损毁";
        if (scale == "13") return "陆上难以出现，如有必成灾祸";
        if (scale == "14") return "陆上难以出现，如有必成灾祸";
        if (scale == "15") return "陆上难以出现，如有必成灾祸";
        if (scale == "16") return "陆上难以出现，如有必成灾祸";
        if (scale == "17") return "陆上难以出现，如有必成灾祸";
        if (scale == ">17") return "陆上极难出现，毁灭性破坏";
        throw new ArgumentOutOfRangeException();
    }
}
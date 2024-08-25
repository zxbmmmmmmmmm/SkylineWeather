using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FluentWeather.Abstraction.Models;
public class Announcement(int id,string name)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public bool IsAvailable => DateTime.Now <= ExpiredAt && AvailableRegions.Contains(RegionInfo.CurrentRegion.Name);
    public bool IsVisible { get; set; }
    public bool CloseWhenView { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? ImageUrl { get; set; }
    public string? Link { get; set; }
    public string[]? AvailableRegions { get; set; }
    public bool IsClosable { get; set; } = true;
    public DateTime ExpiredAt { get; set; }
    public string LinkText { get; set; } = "View more";

    

    public static List<Announcement> GetAllAnnouncements()
    {
        return [
            new Announcement(0,"ms-awards-2024_zh")
            {
                IsVisible = true,
                Title = "喜欢此应用?",
                Content = "在微软商店年度应用评选上为我们投票！",
                LinkText = "详情",
                IsClosable = true,
                CloseWhenView = true,
                CreatedAt = new DateTime(2024,8,24),
                Link = "https://mp.weixin.qq.com/s?__biz=MzkwNTcyMDI3MA==&mid=2247484284&idx=1&sn=28e021c734cf47ab10aeee2fd4442ce4&chksm=c126079c4681a54b45a82c453501202835b632a7b29a1d40efb166247ab1d73ed6a6c1ddf639",
                AvailableRegions = ["CN","HK"],
                ExpiredAt= new DateTime(2024,9,15),
            }
            ];
    }
    public static List<Announcement> GetAvailableAnnouncements()
    {
        return GetAllAnnouncements().Where(p => p.IsAvailable).ToList();
    }
}

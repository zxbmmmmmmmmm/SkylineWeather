using System;
using System.Globalization;
using System.Text;
using Windows.Foundation;
using Microsoft.UI.Xaml;

namespace SkylineWeather.WinUI.Controls.Helpers;

internal struct ThicknessF
{
    private float _Left;
    private float _Top;
    private float _Right;
    private float _Bottom;

    public ThicknessF(float uniformLength)
    {
        this._Left = this._Top = this._Right = this._Bottom = uniformLength;
    }

    public ThicknessF(float left, float top, float right, float bottom)
    {
        this._Left = left;
        this._Top = top;
        this._Right = right;
        this._Bottom = bottom;
    }

    public float Left
    {
        get => this._Left;
        set => this._Left = value;
    }

    public float Top
    {
        get => this._Top;
        set => this._Top = value;
    }

    public float Right
    {
        get => this._Right;
        set => this._Right = value;
    }

    public float Bottom
    {
        get => this._Bottom;
        set => this._Bottom = value;
    }

    public override string ToString() => this.ToString(CultureInfo.InvariantCulture);

    internal string ToString(CultureInfo cultureInfo)
    {
        char numericListSeparator = TokenizerHelper.GetNumericListSeparator((IFormatProvider)cultureInfo);
        StringBuilder stringBuilder = new StringBuilder(64 /*0x40*/);
        stringBuilder.Append(this.InternalToString(this._Left, cultureInfo));
        stringBuilder.Append(numericListSeparator);
        stringBuilder.Append(this.InternalToString(this._Top, cultureInfo));
        stringBuilder.Append(numericListSeparator);
        stringBuilder.Append(this.InternalToString(this._Right, cultureInfo));
        stringBuilder.Append(numericListSeparator);
        stringBuilder.Append(this.InternalToString(this._Bottom, cultureInfo));
        return stringBuilder.ToString();
    }

    internal string InternalToString(float l, CultureInfo cultureInfo)
    {
        return float.IsNaN(l) ? "Auto" : Convert.ToString(l, (IFormatProvider)cultureInfo);
    }

    public override bool Equals(object obj) => obj is ThicknessF thickness && this == thickness;

    public bool Equals(ThicknessF thickness) => this == thickness;

    public override int GetHashCode()
    {
        return this._Left.GetHashCode() ^ this._Top.GetHashCode() ^ this._Right.GetHashCode() ^ this._Bottom.GetHashCode();
    }

    public static bool operator ==(ThicknessF t1, ThicknessF t2)
    {
        return t1._Left == t2._Left && t1._Top == t2._Top && t1._Right == t2._Right && t1._Bottom == t2._Bottom;
    }

    public static ThicknessF operator +(ThicknessF t1, ThicknessF t2)
    {
        return new ThicknessF(t1._Left + t2._Left, t1._Top + t2._Top, t1._Right + t2._Right, t1._Bottom + t2._Bottom);
    }

    public static bool operator !=(ThicknessF t1, ThicknessF t2) => !(t1 == t2);

    public static explicit operator ThicknessF(Thickness thickness)
    {
        return new ThicknessF((float)thickness.Left, (float)thickness.Top, (float)thickness.Right, (float)thickness.Bottom);
    }
}

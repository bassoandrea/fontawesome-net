using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace FontAwesome.Net.Wpf
{
    public static class FontsManager
    {
        
        private static readonly Dictionary<IFontAwesomeIconStyle, FontData> _fonts = new Dictionary<IFontAwesomeIconStyle, FontData>();

        public static void RegisterFont(IFontAwesomeIconStyle style, FontFamily fontFamily)
        {
            if (_fonts.ContainsKey(style))
                throw new InvalidOperationException($"A font is already registered for the style {style}");
         
            _fonts[style] = new FontData(fontFamily);
        }

        public static FontFamily GetFontFamily(this IFontAwesomeIconStyle style)
            => GetFontData(style).FontFamily;

        public static Typeface GetFontTypeface(this IFontAwesomeIconStyle style)
            => GetFontData(style).Typeface;

        private static FontData GetFontData(IFontAwesomeIconStyle style)
        {
            return _fonts.TryGetValue(style, out var font) 
                ? font
                : throw new ArgumentOutOfRangeException(nameof(style), style, "No font data associated with the style passed as a parameter");
        }
    }

    public class FontData
    {
        public FontData(FontFamily fontFamily)
        {
            FontFamily = fontFamily;
            Typeface = new Typeface(fontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        }

        public FontFamily FontFamily { get; }
        public Typeface Typeface { get; }
    }
}

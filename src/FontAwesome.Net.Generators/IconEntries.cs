using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace FontAwesome.Net.Generators;

internal class IconEntries
    : Dictionary<string, IconEntry>
{
    public static IconEntries ParseFromJson(string json)
    {
        return JsonSerializer.Deserialize<IconEntries>(json) ?? new IconEntries();
    }

    public string GenerateIconStylesCode()
    {
        var styles = this.Values
            .SelectMany(e => e.Styles)
            .Distinct()
            .ToArray();

        var sb = new StringBuilder();
        sb.AppendLine("""
                      namespace FontAwesome.Net.Generators
                      {
                          public partial class FontAwesomeIconStyle
                              : Enumeration
                          {
                      """);

        var i = 0;
        foreach (var style in styles)
        {
            var name = ConvertToValidVariableName(style);
            sb.AppendLine($"        public static FontAwesomeIconStyle {name} = new FontAwesomeIconStyle({++i}, nameof({name}));");
        }
        
        sb.AppendLine("""
                          }
                      }
                      """);
        
        return sb.ToString();
    }

    public string GenerateIconsCode()
    {
        var sb = new StringBuilder();

        sb.AppendLine("""
                      namespace FontAwesome.Net.Generators
                      {

                          public partial class FontAwesomeIcon
                              : Enumeration
                          {
                      """);

        foreach (var item in this)
        {
            sb.AppendLine(GenerateMember(item.Key, item.Value));
        }

        sb.AppendLine("""
                          }
                      }
                      """);

        return sb.ToString();
    }

    private static string GenerateMember(string iconId, IconEntry icon)
    {
        var name = ConvertToValidVariableName(iconId);
        var styles = icon.Styles.Select(s => $"FontAwesomeIconStyle.{ConvertToValidVariableName(s)}").ToArray();
        var stylesArgs = string.Join(", ", styles);

        return $$"""
                         /// <summary>
                         /// {{icon.Label}} [<c>{{icon.Unicode}}</c>]
                         /// <br/>Styles: <i>{{string.Join(", ", styles)}}</i>
                         /// <para><see href="https://fontawesome.com/icons/{{iconId}}/"/></para>
                         /// </summary>
                         public static FontAwesomeIcon {{name}} = new FontAwesomeIcon(0x{{icon.Unicode}}, nameof({{name}}), new[] { {{stylesArgs}} });

                 """;
    }

    private static string ConvertToValidVariableName(string name)
    {
        var textInfo = CultureInfo.InvariantCulture.TextInfo;
        var words = Regex.Split(name, @"[^a-zA-Z0-9]+");

        var result = string.Join("", words.Select(textInfo.ToTitleCase).ToArray());

        if (!char.IsLetter(result.FirstOrDefault()))
            result = $"_{result}";
        return result;
    }
}

internal class IconEntry
{
    [JsonPropertyName("label")]
    public string Label { get; set; }

    [JsonPropertyName("unicode")]
    public string Unicode { get; set; }

    [JsonPropertyName("styles")]
    public List<string> Styles { get; set; }
}
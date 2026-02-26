using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Text;

namespace FontAwesome.Net.Generators;

[Generator]
public class FontAwesomeIconGenerator
    : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var files = context.AdditionalTextsProvider
            .Where(at => at.Path.EndsWith("icons.json", StringComparison.CurrentCultureIgnoreCase))
            .Select((file, ct) =>
            {
                var content = file.GetText(ct)?.ToString() ?? string.Empty;
                return (path: file.Path, content: content);
            });

        context.RegisterPostInitializationOutput(spc =>
        {
            var source = """
                            using System;
                            using System.Reflection;
                            
                            namespace FontAwesome.Net.Generators
                            {
                                public partial class FontAwesomeIconStyle
                                    : Enumeration, IFontAwesomeIconStyle
                                {
                                    private FontAwesomeIconStyle(int id, string name) : base(id, name)
                                    {
                                    }
                                }
                                
                                public partial class FontAwesomeIcon
                                    : Enumeration, IFontAwesomeIcon
                                {
                                    private FontAwesomeIconStyle[] _styles;
                                
                                    private FontAwesomeIcon(int id, string name, FontAwesomeIconStyle[] styles = null) : base(id, name)
                                    {
                                        _styles = styles ?? Array.Empty<FontAwesomeIconStyle>();
                                    }
                                    
                                    public IFontAwesomeIconStyle[] Styles 
                                        => _styles;
                                    
                                    public static FontAwesomeIcon None = new FontAwesomeIcon(0x0, nameof(None));
                                }
                            }
                         """;

            spc.AddSource("FontAwesome.Core.g.cs", SourceText.From(source, Encoding.UTF8));
        });

        context.RegisterSourceOutput(files, (spc, file) =>
        {
            var icons = IconEntries.ParseFromJson(file.content);
            
            var sourceStyles = icons.GenerateIconStylesCode();
            spc.AddSource($"FontAwesome.IconStyles.g.cs", SourceText.From(sourceStyles, Encoding.UTF8));

            var sourceIcons = icons.GenerateIconsCode();
            spc.AddSource($"FontAwesome.Icons.g.cs", SourceText.From(sourceIcons, Encoding.UTF8));
        });
    }
}
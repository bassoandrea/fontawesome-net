<div align="center">
<h1>fontawesome-net</h1>

_Font Awesome icons for .NET WPF applications — with spin, rotation & flip built right in_

[![FontAwesome](https://img.shields.io/badge/FontAwesome-gray?style=flat&logo=fontawesome)](https://github.com/FortAwesome/Font-Awesome)
[![NuGet](https://img.shields.io/nuget/v/FontAwesome.Net.Wpf.svg)](https://www.nuget.org/packages/FontAwesome.Net.Wpf/)
[![NuGet](https://img.shields.io/nuget/v/FontAwesome.Net.Generators.svg)](https://www.nuget.org/packages/FontAwesome.Net.Generators/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
</div>

**fontawesome-net** is a lightweight, developer-friendly library for integrating [Font Awesome](https://fontawesome.com/) icons into your .NET WPF applications. Featuring a Roslyn source generator for compile-time icon generation and WPF controls with built-in support for spin animations, rotation, and flipping - no more messy Unicode strings or resource dictionaries.

## Features

- **Strongly-typed icons** — Compile-time generated icon classes from Font Awesome metadata
- **WPF controls** — `FontAwesomeImage` (vector-based) and `FontAwesomeBlock` (text-based)
- **Animations out of the box** — Spin, rotate, and flip icons with simple dependency properties
- **Data binding friendly** — All properties are dependency properties for MVVM scenarios
- **Style support** — Works with Solid, Regular, Brands, and other Font Awesome styles
- **.NET Framework 4.8+ & .NET 5+**

## Repository Structure

```
src/
├── FontAwesome.Net/            # Core library (interfaces & base classes)
├── FontAwesome.Net.Generators/ # Roslyn source generator
└── FontAwesome.Net.Wpf/        # WPF controls
examples/
├── FontAwesome.Net.Wpf.ExplorerApp/ # Example WPF application
└── resources/                       # Font Awesome resources for examples
```

## Installation

### 1. Install NuGet Packages

```bash
# WPF controls package
dotnet add package FontAwesome.Net.Wpf

# Source generator (automatically included as a dependency)
dotnet add package FontAwesome.Net.Generators
```

Or via Package Manager Console:

```powershell
Install-Package FontAwesome.Net.Wpf
Install-Package FontAwesome.Net.Generators
```

### 2. Add Font Awesome Font Files

Download the Font Awesome font files (`.otf`) from [fontawesome.com](https://fontawesome.com/download) and add them to your project as resources.

### 3. Register Fonts at Application Startup

```csharp
using FontAwesome.Net.Wpf;
using FontAwesome.Net.Generators;

// Register your Font Awesome fonts with their corresponding styles
FontsManager.RegisterFont(FontAwesomeIconStyle.Solid, new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/Font Awesome 7 Free-Solid-900.otf#Font Awesome 7 Free"));
FontsManager.RegisterFont(FontAwesomeIconStyle.Regular, new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/Font Awesome 7 Free-Regular-400.otf#Font Awesome 7 Free"));
FontsManager.RegisterFont(FontAwesomeIconStyle.Brands, new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/Font Awesome 7 Brands-Regular-400.otf#Font Awesome 7 Brands"));
```

### 4. Add icons.json to Your Project

Include the Font Awesome `icons.json` metadata file as an additional file for the source generator:

```xml
<ItemGroup>
  <AdditionalFiles Include="path\to\icons.json" />
</ItemGroup>
```

## Usage Examples

### FontAwesomeBlock — TextBlock-based Control

Perfect for inline text scenarios where icons flow naturally with text.

```xml
<Window x:Class="MyApp.MainWindow"
        xmlns:fa="clr-namespace:FontAwesome.Net.Wpf;assembly=FontAwesome.Net.Wpf"
        xmlns:icons="clr-namespace:FontAwesome.Net.Generators;assembly=FontAwesome.Net.Generators">

    <StackPanel>
        <!-- Basic icon -->
        <fa:FontAwesomeBlock Icon="{x:Static icons:FontAwesomeIcon.Github}"
                             FontSize="24" />

        <!-- With specific style -->
        <fa:FontAwesomeBlock Icon="{x:Static icons:FontAwesomeIcon.FontAwesome}"
                             IconStyle="{x:Static icons:FontAwesomeIconStyle.Regular}"
                             FontSize="32"
                             Foreground="#418fde" />

        <!-- Spinning loader -->
        <fa:FontAwesomeBlock Icon="{x:Static icons:FontAwesomeIcon.Spinner}"
                             Spin="True"
                             SpinDuration="1"
                             FontSize="24" />

        <!-- Rotated icon -->
        <fa:FontAwesomeBlock Icon="{x:Static icons:FontAwesomeIcon.ArrowRight}"
                             Rotation="45"
                             FontSize="24" />

        <!-- Flipped icon -->
        <fa:FontAwesomeBlock Icon="{x:Static icons:FontAwesomeIcon.ArrowLeft}"
                             FlipOrientation="Horizontal"
                             FontSize="24" />

        <!-- Reversed spin direction -->
        <fa:FontAwesomeBlock Icon="{x:Static icons:FontAwesomeIcon.Sync}"
                             Spin="True"
                             SpinDuration="2"
                             ReverseSpinDirection="True"
                             FontSize="24" />
    </StackPanel>
</Window>
```

### FontAwesomeImage — Image-based Control

Renders icons as vector images with customizable foreground brush.

```xml
<Window x:Class="MyApp.MainWindow"
        xmlns:fa="clr-namespace:FontAwesome.Net.Wpf;assembly=FontAwesome.Net.Wpf"
        xmlns:icons="clr-namespace:FontAwesome.Net.Generators;assembly=FontAwesome.Net.Generators">

    <StackPanel>
        <!-- Basic image icon -->
        <fa:FontAwesomeImage Icon="{x:Static icons:FontAwesomeIcon.User}"
                             Width="48" Height="48"
                             Foreground="SteelBlue" />

        <!-- Gradient foreground -->
        <fa:FontAwesomeImage Icon="{x:Static icons:FontAwesomeIcon.Heart}"
                             Width="64" Height="64">
            <fa:FontAwesomeImage.Foreground>
                <LinearGradientBrush StartPoint="0,0" EndPoint="1,1">
                    <GradientStop Color="Red" Offset="0" />
                    <GradientStop Color="Pink" Offset="1" />
                </LinearGradientBrush>
            </fa:FontAwesomeImage.Foreground>
        </fa:FontAwesomeImage>

        <!-- Animated loading indicator -->
        <fa:FontAwesomeImage Icon="{x:Static icons:FontAwesomeIcon.CircleNotch}"
                             Width="32" Height="32"
                             Foreground="#333333"
                             Spin="True"
                             SpinDuration="0.8" />

        <!-- Rotated and styled -->
        <fa:FontAwesomeImage Icon="{x:Static icons:FontAwesomeIcon.Rocket}"
                             Width="48" Height="48"
                             Foreground="Orange"
                             Rotation="-45" />

        <!-- Data binding example -->
        <fa:FontAwesomeImage Icon="{Binding CurrentIcon}"
                             IconStyle="{Binding CurrentStyle}"
                             Foreground="{Binding IconColor}"
                             Spin="{Binding IsLoading}"
                             Width="24" Height="24" />
    </StackPanel>
</Window>
```

## Properties Reference

### Common Properties (Both Controls)

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Icon` | `IFontAwesomeIcon` | `null` | The Font Awesome icon to display |
| `IconStyle` | `IFontAwesomeIconStyle` | `null` | The style variant (Solid, Regular, Brands) |
| `Spin` | `bool` | `false` | Enable/disable spin animation |
| `SpinDuration` | `double` | `1.0` | Duration of one full rotation in seconds |
| `ReverseSpinDirection` | `bool` | `false` | Reverse the spin direction (counter-clockwise) |
| `Rotation` | `double` | `0` | Rotation angle in degrees (0-360) |
| `FlipOrientation` | `FlipOrientation` | `None` | Flip horizontally, vertically, or none |

### FontAwesomeImage Additional Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Foreground` | `Brush` | `Brushes.Black` | The brush used to render the icon |

## Available Icon Styles

The source generator creates the following style classes based on Font Awesome metadata:

- `FontAwesomeIconStyle.Solid`
- `FontAwesomeIconStyle.Regular`
- `FontAwesomeIconStyle.Brands`
- And any other styles present in your `icons.json`

## Supported Targets

| Package | Target Frameworks |
|---------|-------------------|
| FontAwesome.Net.Generators | netstandard2.0 (Analyzer) |
| FontAwesome.Net.Wpf | net48, net5.0-windows |

## NuGet Packages

| Package | Link |
|---------|------|
| FontAwesome.Net.Wpf | [![NuGet](https://img.shields.io/nuget/v/FontAwesome.Net.Wpf.svg)](https://www.nuget.org/packages/FontAwesome.Net.Wpf/) |
| FontAwesome.Net.Generators | [![NuGet](https://img.shields.io/nuget/v/FontAwesome.Net.Generators.svg)](https://www.nuget.org/packages/FontAwesome.Net.Generators/) |

## Contributing

Contributions are welcome! Feel free to submit issues and pull requests.

## License

This project is licensed under the [MIT License](LICENSE).

Font Awesome icons are licensed under their own [license](https://fontawesome.com/license).

---

<div align="center">
Made by <a href="https://github.com/bassoandrea">bassoandrea</a>
</div>
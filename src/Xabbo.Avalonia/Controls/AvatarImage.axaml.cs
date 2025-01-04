using Avalonia;
using Avalonia.Controls.Primitives;
using ReactiveUI.Fody.Helpers;
using Xabbo.Utility;

namespace Xabbo.Avalonia.Controls;

public class AvatarImage : TemplatedControl
{
    // Define the scaling factor
    public static readonly StyledProperty<double> ScalingProperty =
        AvaloniaProperty.Register<AvatarImage, double>(nameof(Scaling), defaultValue: 1.0);

    // Define properties for dynamically computed dimensions
    public static readonly DirectProperty<AvatarImage, double> AvatarBodyWidthProperty =
        AvaloniaProperty.RegisterDirect<AvatarImage, double>(nameof(AvatarBodyWidth), x => x.AvatarBodyWidth);

    public static readonly DirectProperty<AvatarImage, double> AvatarBodyHeightProperty =
        AvaloniaProperty.RegisterDirect<AvatarImage, double>(nameof(AvatarBodyHeight), x => x.AvatarBodyHeight);

    public static readonly DirectProperty<AvatarImage, double> AvatarHeadWidthProperty =
        AvaloniaProperty.RegisterDirect<AvatarImage, double>(nameof(AvatarHeadWidth), x => x.AvatarHeadWidth);

    public static readonly DirectProperty<AvatarImage, double> AvatarHeadHeightProperty =
        AvaloniaProperty.RegisterDirect<AvatarImage, double>(nameof(AvatarHeadHeight), x => x.AvatarHeadHeight);

    // Base dimensions
    private const double BaseBodyWidth = 64;
    private const double BaseBodyHeight = 110;
    private const double BaseHeadWidth = 54;
    private const double BaseHeadHeight = 62;

    [Reactive]
    double Scaling
    {
        get => GetValue(ScalingProperty);
        set => SetValue(ScalingProperty, value);
    }

    [Reactive] public double AvatarBodyWidth => BaseBodyWidth * Scaling;
    [Reactive] public double AvatarBodyHeight => BaseBodyHeight * Scaling;
    [Reactive] public double AvatarHeadWidth => BaseHeadWidth * Scaling;
    [Reactive] public double AvatarHeadHeight => BaseHeadHeight * Scaling;

    public static readonly StyledProperty<string?> FigureStringProperty =
        AvaloniaProperty.Register<AvatarImage, string?>(nameof(FigureString));

    public static readonly StyledProperty<string?> UserNameProperty =
        AvaloniaProperty.Register<AvatarImage, string?>(nameof(UserName));

    public static readonly StyledProperty<int> DirectionProperty =
        AvaloniaProperty.Register<AvatarImage, int>(nameof(Direction), defaultValue: 2);

    public static readonly StyledProperty<bool> HeadOnlyProperty =
        AvaloniaProperty.Register<AvatarImage, bool>(nameof(HeadOnly));

    public static readonly DirectProperty<AvatarImage, string> PlaceholderProperty =
        AvaloniaProperty.RegisterDirect<AvatarImage, string>(nameof(Placeholder),
            x => x.Placeholder);

    public static readonly DirectProperty<AvatarImage, string?> AvatarImageUrlProperty =
        AvaloniaProperty.RegisterDirect<AvatarImage, string?>(nameof(AvatarImageUrl),
            x => x.AvatarImageUrl);

    public string? FigureString
    {
        get => GetValue(FigureStringProperty);
        set => SetValue(FigureStringProperty, value);
    }

    public string? UserName
    {
        get => GetValue(UserNameProperty);
        set => SetValue(UserNameProperty, value);
    }

    public int Direction
    {
        get => GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    public bool HeadOnly
    {
        get => GetValue(HeadOnlyProperty);
        set => SetValue(HeadOnlyProperty, value);
    }

    private string _placeholder = "avares://Xabbo.Avalonia/Assets/Images/Avatar/body-2.png";
    public string Placeholder
    {
        get => _placeholder;
        private set => SetAndRaise(PlaceholderProperty, ref _placeholder, value);
    }

    private string? _avatarImageUrl;
    public string? AvatarImageUrl
    {
        get => _avatarImageUrl;
        private set => SetAndRaise(AvatarImageUrlProperty, ref _avatarImageUrl, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == FigureStringProperty ||
            change.Property == DirectionProperty ||
            change.Property == HeadOnlyProperty ||
            change.Property == UserNameProperty ||
            change.Property == ScalingProperty)
        {
            Placeholder = $"avares://Xabbo.Avalonia/Assets/Images/Avatar/{(HeadOnly ? "head" : "body")}-{Direction}.png";

            if (string.IsNullOrWhiteSpace(FigureString) &&
                string.IsNullOrWhiteSpace(UserName))
            {
                AvatarImageUrl = null;
            }
            else
            {
                AvatarImageUrl = UrlHelper.AvatarImageUrl(
                    figure: FigureString,
                    direction: Direction,
                    headOnly: HeadOnly,
                    name: UserName
                );
            }
        }

        base.OnPropertyChanged(change);
    }
}

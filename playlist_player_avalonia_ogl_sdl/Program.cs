using Avalonia;

namespace OpenGLAvalonia
{
    class Program
    {
        public static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new X11PlatformOptions { RenderingMode = [X11RenderingMode.Egl] })
                .With(new Win32PlatformOptions { RenderingMode = [Win32RenderingMode.AngleEgl] })
                .LogToTrace();
        }
    }
}

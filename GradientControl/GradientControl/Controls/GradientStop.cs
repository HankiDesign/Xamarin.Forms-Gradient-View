using SkiaSharp;

namespace GradientControl.Controls
{
	public struct GradientStop
    {
		public float Location { get; set; }
		public SKColor Color { get; set; }
		public float Opacity { get; set; }

	    public GradientStop(float location, SKColor color, float opacity)
	    {
			Location = location;
			Color = color;
			Opacity = opacity;
	    }
    }
}
using SkiaSharp;

namespace GradientControl.Controls
{
	public struct GradientStop
    {
		public int Location { get; set; }
		public SKColor Color { get; set; }
		public float Opacity { get; set; }

	    public GradientStop(int location, SKColor color, float opacity)
	    {
			Location = location;
			Color = color;
			Opacity = opacity;
	    }
    }
}
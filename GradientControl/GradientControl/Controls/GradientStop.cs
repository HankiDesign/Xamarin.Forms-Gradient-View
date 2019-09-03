using GradientControl.ViewModel;
using SkiaSharp;

namespace GradientControl.Controls
{
	public struct GradientStop : BaseViewModel
    {
        private float location;
        public float Location
        {
            get
            {
                return location;
            }
            set
            {
                if (location != value)
                {
                    location = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private float opacity;
        public float Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                if (opacity != value)
                {
                    opacity = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private SKColor color;
        public SKColor Color
        {
            get
            {
                return color;
            }
            set
            {
                if (color != value)
                {
                    color = value;
                    NotifyPropertyChanged();
                }
            }
        }

	    public GradientStop(float location, SKColor color, float opacity)
	    {
			Location = location;
			Color = color;
			Opacity = opacity;
	    }
    }
}
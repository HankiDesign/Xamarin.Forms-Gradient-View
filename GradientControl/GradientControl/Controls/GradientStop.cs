using GradientControl.ViewModel;
using SkiaSharp;
using Xamarin.Forms;

namespace GradientControl.Controls
{
	public class GradientStop : BaseViewModel
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

        private Color color;
        public Color Color
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

	    public GradientStop(float location, Color color, float opacity)
	    {
			Location = location;
			Color = color;
			Opacity = opacity;
	    }

        public GradientStop()
        {

        }
    }
}
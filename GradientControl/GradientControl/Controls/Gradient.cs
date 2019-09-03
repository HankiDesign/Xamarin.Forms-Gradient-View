using System.Collections.Generic;
using GradientControl.ViewModel;

namespace GradientControl.Controls
{
	public class Gradient : BaseViewModel
    {
        private List<GradientStop> stops;
        public List<GradientStop> Stops
        {
            get
            {
                return stops;
            }
            set
            {
                if (stops != value)
                {
                    stops = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Gradient(List<GradientStop> stops)
	    {
		    Stops = new List<GradientStop>();
		    Stops.AddRange(stops);
	    }

		public Gradient() { }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GradientControl.ViewModel;

namespace GradientControl.Controls
{
	public class Gradient : BaseViewModel
    {
        private ObservableCollection<GradientStop> stops;
        public ObservableCollection<GradientStop> Stops
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
		    Stops = new ObservableCollection<GradientStop>();
		    //Stops.AddRange(stops);
	    }

		public Gradient()
        {
            Stops = new ObservableCollection<GradientStop>();
        }
    }
}
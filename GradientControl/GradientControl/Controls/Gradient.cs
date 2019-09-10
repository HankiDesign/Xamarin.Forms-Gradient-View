using System.Collections.Generic;

namespace GradientControl.Controls
{
	public class Gradient
    {
		public List<GradientStop> Stops { get; set; }

	    public Gradient(List<GradientStop> stops)
	    {
		    Stops = new List<GradientStop>();
		    Stops.AddRange(stops);
	    }

		public Gradient() { }
    }
}
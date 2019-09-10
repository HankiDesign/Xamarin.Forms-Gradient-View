using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Xamarin.Forms;

namespace GradientControl.Controls
{
	public class GradientView : SKCanvasView
	{
		private SKColor[] colors;
		private float[] locations;
		private int index;
		private int counter;
		private Timer animationTimer;
		private List<GradientStop> currentStops = new List<GradientStop>();
		private Gradient oldGradient;
		private Gradient newGradient;

		public static readonly BindableProperty GradientsProperty = BindableProperty.Create(nameof(Gradients), typeof(IList<Gradient>), typeof(GradientView));
		public static readonly BindableProperty AnimatingProperty = BindableProperty.Create(nameof(Animating), typeof(bool), typeof(GradientView), true, propertyChanged: OnAnimatingChanged);
		public static readonly BindableProperty FpsProperty = BindableProperty.Create(nameof(Fps), typeof(int), typeof(GradientView), 60, propertyChanged: OnFpsChanged);
		public static readonly BindableProperty FramesPerTransitionProperty = BindableProperty.Create(nameof(FramesPerTransition), typeof(int), typeof(GradientView), 3000);

		public IList<Gradient> Gradients
		{
			get => (IList<Gradient>)GetValue(GradientsProperty);
			set => SetValue(GradientsProperty, value);
		}

		public bool Animating
		{
			get => (bool)GetValue(AnimatingProperty);
			set => SetValue(AnimatingProperty, value);
		}

		public int Fps
		{
			get => (int)GetValue(FpsProperty);
			set => SetValue(FpsProperty, value);
		}

		public int FramesPerTransition
		{
			get => (int)GetValue(FramesPerTransitionProperty);
			set => SetValue(FramesPerTransitionProperty, value);
		}

		public GradientView()
		{
			Gradients = new List<Gradient>
			{
				new Gradient(new List<GradientStop>
				{
					new GradientStop(0, SKColor.Parse("#2c3e50"), 1),
					new GradientStop(1, SKColor.Parse("#4ca1af"), 1)
				}),

				new Gradient(new List<GradientStop>
				{
					new GradientStop(0, SKColor.Parse("#ff5f6d"), 1),
					new GradientStop(1, SKColor.Parse("#ffc371"), 1)
				}),

				new Gradient(new List<GradientStop>
				{
					new GradientStop(0, SKColor.Parse("#e96443"), 1),
					new GradientStop(1, SKColor.Parse("#904e95"), 1)
				}),
			};

			animationTimer = new Timer(1000 / (double)Fps);
			animationTimer.Elapsed += AnimationTimer_Elapsed;
			ResetAnimation();
			StartAnimation();
		}

		private void AnimationTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			animationTimer.Stop();
			currentStops.Clear();
			for (int i = 0; i < oldGradient.Stops.Count; i++)
			{
				currentStops.Add(LerpStop(oldGradient.Stops[i], newGradient.Stops[i], Math.Min((float)counter / FramesPerTransition, 1)));
			}

			colors = currentStops.Select(x => x.Color).ToArray();
			locations = currentStops.Select(x => (float)x.Location).ToArray();

			counter++;

			// Check if we have animated a full gradient change
			if (counter >= FramesPerTransition)
			{
				// Move to the next gradient index or to the start if we went through all of them
				index = index + 1 == Gradients.Count ? 0 : index + 1;

				oldGradient = Gradients[index];
				newGradient = Gradients[index < Gradients.Count - 1 ? index + 1 : 0];

				// Start over
				counter = 0;
			}

			InvalidateSurface();
			animationTimer.Start();
		}

		private void ResetAnimation()
		{
			index = 0;
			oldGradient = Gradients[index];
			newGradient = Gradients[index < Gradients.Count - 1 ? index + 1 : 0];
			currentStops.Clear();
		}

		public void StartAnimation()
		{
			animationTimer.Start();
		}

		public void StopAnimation()
		{
			animationTimer.Stop();
		}

		public void UpdateFps()
		{
			animationTimer.Stop();
			animationTimer.Interval = 1000/(double)Fps;
			animationTimer.Start();
		}

		private GradientStop LerpStop(GradientStop first, GradientStop second, float percentage)
		{
			return new GradientStop(first.Location, LerpRGB(first.Color, second.Color, percentage), 1);
		}

		public static SKColor LerpRGB(SKColor a, SKColor b, float t)
		{
			var r = a.Red + (b.Red - a.Red) * t;
			var g = a.Green + (b.Green - a.Green) * t;
			var bl = a.Blue + (b.Blue - a.Blue) * t;

			return new SKColor
			(
				Convert.ToByte(r),
				Convert.ToByte(g),
				Convert.ToByte(bl),
				255 // Ignore alpha channel for now
			);
		}

		protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			var canvas = e.Surface.Canvas;
			canvas.Clear();

			if (colors != null)
			{
				var startPoint = new SKPoint(0, 0);
				var endPoint = new SKPoint(0, e.Info.Height);

				var shader = SKShader.CreateLinearGradient(startPoint, endPoint, colors, locations, SKShaderTileMode.Clamp);

				SKPaint paint = new SKPaint
				{
					Style = SKPaintStyle.Fill,
					Shader = shader
				};

				canvas.DrawRect(new SKRect(0, 0, e.Info.Width, e.Info.Height), paint);
			}
		}

		private static void OnAnimatingChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if ((bool)newValue)
			{
				((GradientView)bindable).StartAnimation();
			}

			else
			{
				((GradientView)bindable).StopAnimation();
			}
		}

		private static void OnFpsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((GradientView)bindable).UpdateFps();
		}
	}
}

using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace GradientControl.Controls
{
	public class ComplexGradientView : SKCanvasView
	{
		private Color topLeftColor;
		private Color topRightColor;
		private Color bottomLeftColor;
		private Color bottomRightColor;
		private SKBitmap backgroundGradient;

		public static readonly BindableProperty TopLeftColorProperty = BindableProperty.Create(nameof(TopLeftColor), typeof(Color), typeof(GradientView), defaultValue: Color.Blue);
		public static readonly BindableProperty TopRightColorProperty = BindableProperty.Create(nameof(TopRightColor), typeof(Color), typeof(GradientView), defaultValue: Color.Red);
		public static readonly BindableProperty BottomLeftColorProperty = BindableProperty.Create(nameof(BottomLeftColor), typeof(Color), typeof(GradientView), defaultValue: Color.Yellow);
		public static readonly BindableProperty BottomRightColorProperty = BindableProperty.Create(nameof(BottomRightColor), typeof(Color), typeof(GradientView), defaultValue: Color.Green);

		public Color TopLeftColor
		{
			get => (Color)GetValue(TopLeftColorProperty);
			set => SetValue(TopLeftColorProperty, value);
		}

		public Color TopRightColor
		{
			get => (Color)GetValue(TopRightColorProperty);
			set => SetValue(TopRightColorProperty, value);
		}

		public Color BottomLeftColor
		{
			get => (Color)GetValue(BottomLeftColorProperty);
			set => SetValue(BottomLeftColorProperty, value);
		}

		public Color BottomRightColor
		{
			get => (Color)GetValue(BottomRightColorProperty);
			set => SetValue(BottomRightColorProperty, value);
		}

		public ComplexGradientView()
		{
			topLeftColor = Color.FromRgb(215, 240, 189);
			topRightColor = Color.FromRgb(83, 174, 219);
            bottomLeftColor = Color.FromRgb(237, 147, 161);
            bottomRightColor = Color.FromRgb(252, 223, 202);
        }

		protected override void OnSizeAllocated(double width, double height)
		{
            //backgroundGradient = FillBitmapSetPixel(25, 50); // Hardcoded for now for S10+
            backgroundGradient = FillBitmapSetPixel(1080, 2280); // Hardcoded for now for S10+
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
            Stopwatch start = new Stopwatch();
            start.Start();
            var canvas = e.Surface.Canvas;
			canvas.Clear();

			if (backgroundGradient != null)
			{
                /* using (SKPaint paint = new SKPaint())
                 {
                     paint.ImageFilter = SKImageFilter.CreateBlur(50, 50);*/
                //canvas.DrawBitmap(backgroundGradient, new SKRect(0, 0, backgroundGradient.Width, backgroundGradient.Height), new SKRect(0, 0, 828, 1792), paint);
                canvas.DrawBitmap(backgroundGradient, 0,0);
                //}
            }
            start.Stop();
            var dur = start.Elapsed;
		}

		uint MakePixel(byte alpha, byte red, byte green, byte blue) =>
		(uint)((alpha << 24) | (blue << 16) | (green << 8) | red);

		SKBitmap FillBitmapSetPixel(int width, int height)
		{			
			SKBitmap bitmap = new SKBitmap(width, height);
			IntPtr basePtr = bitmap.GetPixels();

            double tlR = topLeftColor.R * 255;
			double tlG = topLeftColor.G * 255;
			double tlB = topLeftColor.B * 255;
			double trR = topRightColor.R * 255;
			double trG = topRightColor.G * 255;
			double trB = topRightColor.B * 255;
			double blR = bottomLeftColor.R * 255;
			double blG = bottomLeftColor.G * 255;
			double blB = bottomLeftColor.B * 255;
			double brR = bottomRightColor.R * 255;
			double brG = bottomRightColor.G * 255;
			double brB = bottomRightColor.B * 255;
            /*var elapsed1 = new TimeSpan();
            var elapsed2 = new TimeSpan();
            var elapsed3 = new TimeSpan();
            var elapsed4 = new TimeSpan();*/

            Stopwatch start = new Stopwatch();
            start.Start();

            for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
                    //start.Restart();
					var newColor = BilinearInterpolateColor(tlR, tlG, tlB, trR, trG, trB, blR, blG, blB, brR, brG, brB, (double)x / width, (double)y / height);
                    //elapsed1 += start.Elapsed;
                    //start.Restart();
                    var pixel = MakePixel(0xFF, (byte)newColor.r, (byte)newColor.g, (byte)newColor.b);
                    //elapsed2 += start.Elapsed;
                    //start.Restart();
                    IntPtr pixelPtr = basePtr + 4 * (y * width + x);
                    //elapsed3 += start.Elapsed;
                    //start.Restart();

                    unsafe
					{
						*(uint*)pixelPtr.ToPointer() = pixel;
					}
                    //elapsed4 += start.Elapsed;
                }
			}

			start.Stop();
			var duration = start.Elapsed;
			return bitmap;
		}

		private (double r, double g, double b) BilinearInterpolateColor(double tlR, double tlG, double tlB, double trR, double trG, double trB,
			double blR, double blG, double blB, double brR, double brG, double brB, double fractionX, double fractionY)
		{
			var col1 = InterpolateColor(tlR, tlG, tlB, trR, trG, trB, fractionX * fractionX);
			var col2 = InterpolateColor(blR, blG, blB, brR, brG, brB, fractionX * fractionX);
			return InterpolateColor(col1.r, col1.g, col1.b, col2.r, col2.g, col2.b, fractionY * fractionY);
		}

		private (double r, double g, double b) InterpolateColor(double r1, double g1, double b1, double r2, double g2, double b2, double fraction)
		{
			fraction = fraction < 1D ? fraction : 1D;
            fraction = fraction > 0D ? fraction : 0D;

            double red = r1 + ((r2 - r1) * fraction);
			double green = g1 + ((g2 - g1) * fraction);
			double blue = b1 + ((b2 - b1) * fraction);

            red = red < 255D ? red : 255D;
            red = red > 0D ? red : 0D;
            green = green < 255D ? green : 255D;
            green = green > 0D ? green : 0D;
            blue = red < 255D ? blue : 255D;
            blue = red > 0D ? blue : 0D;

			return (red, green, blue);
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
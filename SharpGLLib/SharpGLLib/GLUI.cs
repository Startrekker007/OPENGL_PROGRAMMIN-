using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace SharpGLLib
{
	class GLUI
	{
		Bitmap imgBuffer;
		SharpGLForm appForm;
		public static bool formStarted = false;
		private Color backColor = Color.White;
		private bool immediateRepaint = true;
		private Color mainCol = Color.Black;
		int Width; int Height;
		public GLUI(int width, int height)
		{
			imgBuffer = new Bitmap(width,height,PixelFormat.Format32bppArgb);
			for(int i = 0; i<width;i++){
				for(int j=0;j<height;j++){
					imgBuffer.SetPixel(i,j,backColor);
				}
			}
			Width=width; Height=height;
			Thread formThread = new Thread(new ThreadStart(startApplication));
			formThread.Start();
			while (!formStarted)
			{
				
			}
			repaintGraphics();
			
		}

		public void repaintGraphics()
		{
			appForm.updateFrameBuffer(imgBuffer);
			appForm.repaintGraphics();
		}

		public void startApplication()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			appForm = new SharpGLForm(Width,Height);
			Application.Run(appForm);
			
		}
		private void checkRepaint()
		{
			if (immediateRepaint)
			{
				repaintGraphics();
			}
		}
		public void fillRectangle(Rectangle rect, Color col)
		{
			for (int i = rect.Y; i < (rect.Y + rect.Height); i++)
			{
				for (int j = rect.X; j < (rect.X + rect.Width); j++)
				{
					if (checkBounds(new Point(i,j)))
						imgBuffer.SetPixel(j,i,col);
				}
			}
			checkRepaint();

		}
		public void fillRectangle(Rectangle rect)
		{
			int R = mainCol.R;
			int G = mainCol.G;
			int B = mainCol.B;
			R+=2;
			G+=2;
			B+=2;
			mainCol=Color.FromArgb(255,R,G,B);
			for (int i = rect.Y; i < (rect.Y + rect.Height); i++)
			{
				for (int j = rect.X; j < (rect.X + rect.Width); j++)
				{
					if (checkBounds(new Point(j, i)))
						imgBuffer.SetPixel(j, i, mainCol);
				}
			}
			checkRepaint();

		}
		/*COMPLETE THIS use pointers to manually edit colour values in bitmap data at memory location yo g ow bay cuz homie solle
		public void fillEllipse(Rectangle sizepos, Color col)
		{
			double radiusx = sizepos.Width/2;
			double radiusy = sizepos.Height/2;

			double angle=0;
			double angleIncrement = 1/(2*radiusx*radiusy);
			double pixelIncrement = 0.5;
			BitmapData imgData = imgBuffer.LockBits(new Rectangle(0,0,imgBuffer.Width,imgBuffer.Height),ImageLockMode.ReadWrite,PixelFormat.Format32bppArgb);
			int stride = imgData.Stride;
			unsafe { 
			byte* ptr = (byte*)imgData.Scan0;
				for (double t = 0; t < (2 * Math.PI); t+=angleIncrement)
				{
					double radiusHere = (radiusx*radiusy)/(Math.Sqrt((Math.Pow(radiusx,2)*Math.Pow(Math.Sin(t),2))+(Math.Pow(radiusy,2)*Math.Pow(Math.Cos(t),2))));
					for (double d = 0; d < radiusHere; d += pixelIncrement)
					{
					
						double x = sizepos.X+(d*Math.Cos(t))+sizepos.Width;
						double y = sizepos.Y + (d * Math.Sin(t)) + sizepos.Height;
						if (checkBounds(new Point((int)x, (int)y))) { 
						imgBuffer.SetPixel((int)x,(int)y,col);
						}

					}
				}
			}
			checkRepaint();
		}
		 * */
		public void fillEllipseP(Rectangle sizepos, Color col)//SLOW AND IT WILL BE DEPRECIATED SOON, PREPARE TO CHANGE UR CODE
		{
			double radiusx = sizepos.Width / 2;
			double radiusy = sizepos.Height / 2;

			double angle = 0;
			double permutations = Math.PI*(3*(radiusx+radiusy)-Math.Sqrt(((3*radiusx)+radiusy)*(radiusx+(3*radiusy))));
			double angleIncrement = ((2*Math.PI)/permutations)/2;
			double pixelIncrement = 0.5;

			for (double t = 0; t < (2 * Math.PI); t += angleIncrement)
			{
				double radiusHere = (radiusx * radiusy) / (Math.Sqrt((Math.Pow(radiusx, 2) * Math.Pow(Math.Sin(t), 2)) + (Math.Pow(radiusy, 2) * Math.Pow(Math.Cos(t), 2))));
				for (double d = 0; d < radiusHere; d += pixelIncrement)
				{

					double x = sizepos.X+(d*Math.Cos(t))+sizepos.Width;
					double y = sizepos.Y + (d * Math.Sin(t)) + sizepos.Height;
					if (checkBounds(new Point((int)x, (int)y))) { 
						imgBuffer.SetPixel((int)x,(int)y,col);
						}
				}
			}
			checkRepaint();
		}
		public void drawImage(Rectangle rect, String path)
		{
			Bitmap loaded = new Bitmap(path);
			drawBitmap(rect,loaded);
			
			checkRepaint();

		}

		private void drawBitmap(Rectangle rect,Bitmap img)
		{
			Bitmap toDraw = new Bitmap(img,rect.Width,rect.Height);
			for (int i = rect.Y; i < (rect.Y + rect.Height); i++)
			{
				for (int j = rect.X; j < (rect.X + rect.Width); j++)
				{
					if(checkBounds(new Point(i,j))){
						imgBuffer.SetPixel(j,i,toDraw.GetPixel(j-rect.X,i-rect.Y));
					}

				}
			}
		}

		
		private bool checkBounds(Point point)
		{
			if (point.Y < imgBuffer.Height && point.Y >= 0 && point.X < imgBuffer.Width && point.X >= 0)
			{
				return true;
			}
			return false;
		}



	}
}

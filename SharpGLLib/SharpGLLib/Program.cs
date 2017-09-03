using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace SharpGLLib
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// 
		public static GLUI glApp;
		[STAThread]
		static void Main()
		{
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new SharpGLForm());
			glApp = new GLUI(500,500);
			glApp.fillRectangle(new System.Drawing.Rectangle(0, 0, 20, 20), Color.Blue);
			glApp.fillEllipseP(new System.Drawing.Rectangle(40,40,200,150),Color.Red);
			glApp.fillEllipseP(new System.Drawing.Rectangle(80,80,20,20),Color.Green);
			glApp.drawImage(new System.Drawing.Rectangle(100,100,90,90),"test.bmp");

		}
	}
}

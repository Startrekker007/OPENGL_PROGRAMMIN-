using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GL2DTest.Menu_Classes.MenuControls;
using System.Diagnostics;

namespace GL2DTest.Menu_Classes
{
	class MainMenu : Menus
	{
		Rectangle menuBounds;
		List<Rectangle> panels = new List<Rectangle>();
		List<Color> panelColors = new List<Color>();
		List<UIControl> controls = new List<UIControl>();
		private bool active = false;
		Action onClick;
		GLUI ui;
		public MainMenu(Rectangle r,Action clicked, GLUI uics)
		{
			menuBounds=r;
			ui=uics;
			panels.Add(new Rectangle(perToPix(0)[0],perToPix(0)[1],perToPix(100)[0],perToPix(20)[1]));
			panelColors.Add(Color.Gray);
			controls.Add(new Button("testbutton.png",new Point(perToPix(1)[0],perToPix(10)[1]),new Size(100,50),new Font(FontFamily.GenericSansSerif,10),"TESTING",()=>{Debug.Print("Hello World");}));
			active=true;
			onClick=clicked;
			redraw();
		}
		private int[] perToPix(double percent)
		{
			double p = percent/100;
			int[] vals = new int[2]{(int)(p*menuBounds.Width+menuBounds.X),(int)(p*menuBounds.Height+menuBounds.Y)};
			return vals;
		}
		private void redraw()
		{
			ui.setImmediateRepaint(false);
			drawAll();
			ui.repaintGraphics();
			
		}
		public void addControl(UIControl c)
		{
			controls.Add(c);
			redraw();
		}
		private void drawAll()
		{
			int count = 0;
			foreach(Rectangle r in panels){
				ui.fillRectangle(r,panelColors[count]);
			}
			foreach (UIControl c in controls)
			{
				c.draw();
			}
			
		}
		public bool checkOn(int x, int y, String action)
		{
			if (checkOnRectangle(x, y, menuBounds))
			{

			}
		}
		private bool checkOnRectangle(int x, int y, Rectangle rekt){
			if ((x > rekt.X) && (x < (rekt.X + rekt.Width)) && (x > rekt.Y) && (x < (rekt.Y + rekt.Height)))
			{
				return true;
			}
			return false;
		}
		
		
	}
}

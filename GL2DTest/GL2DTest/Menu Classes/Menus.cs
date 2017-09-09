using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GL2DTest.Menu_Classes.MenuControls;

namespace GL2DTest.Menu_Classes
{
	interface Menus
	{
		bool checkCursorOn();
		void drawAll();
		void addControl(UIControl c);
		void cursorCheck(int x, int y, String action);


	}
}

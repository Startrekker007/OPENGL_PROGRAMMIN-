using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;
using System.Runtime.InteropServices;
using System.IO;
//using AForge.Math;



namespace SharpGLLib
{
	/// <summary>
	/// The main form class.
	/// </summary>
	public partial class SharpGLForm : Form
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SharpGLForm"/> class.
		/// </summary>
		private bool TexturesInitialised = false;
		private float rotation = 0.0f;
		private float Scale = 1;

		private Bitmap gImage1;
		private System.Drawing.Imaging.BitmapData gbitmapdata;
		private uint[] gtexture = new uint[1];
		double aspectRatio = 0.5;
		private Bitmap toWrite;
		float[] projection_matrix = new float[16];

		/// <summary>
		/// Initializes a new instance of the <see cref="SharpGLForm"/> class.
		/// </summary>
		public SharpGLForm(int w,int h)
		{
			InitializeComponent();
			this.Width=w;
			this.Height=h;
			aspectRatio = (double)this.Width / (double)this.Height;
			gImage1 = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);
			for (int i = 0; i < gImage1.Width; i++)
			{
				for (int j = 0; j < gImage1.Height; j++)
				{
					if ((j / 10) % 2 == 0)
					{
						if ((i / 10) % 2 == 0)
						{
							gImage1.SetPixel(i, j, Color.Red);
						}
						else
						{
							gImage1.SetPixel(i, j, Color.Green);
						}
					}
					else
					{
						if ((i / 10) % 2 == 0)
						{
							gImage1.SetPixel(i, j, Color.Blue);
						}
						else
						{
							gImage1.SetPixel(i, j, Color.Yellow);
						}
					}
				}
			}
			GLUI.formStarted=true;
		}

		private void InitialiseTexture(ref OpenGL gl)
		{
			//gImage1 = new Bitmap(@"test.bmp");// Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
			
			
			
			
			//Bitmap resized = new Bitmap(gImage1,this.Width,this.Height);
			gbitmapdata = null;
			Rectangle rect = new Rectangle(0, 0, gImage1.Width, gImage1.Height);
			gbitmapdata = gImage1.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			gImage1.UnlockBits(gbitmapdata);
			gl.GenTextures(1, gtexture);
			gl.BindTexture(OpenGL.GL_TEXTURE_2D, gtexture[0]);
			gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, (int)OpenGL.GL_RGBA8, gImage1.Width, gImage1.Height, 0, OpenGL.GL_BGRA_EXT, OpenGL.GL_UNSIGNED_BYTE, gbitmapdata.Scan0);
			uint[] array = new uint[] { OpenGL.GL_LINEAR };
			uint[] array3 = new uint[] { OpenGL.GL_LINEAR };
			gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, array);
			gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, array3);
			uint[] array2 = new uint[]{OpenGL.GL_CLAMP_READ_COLOR};
			//uint[] array3 = new uint[]{Convert.ToUInt32(-1000)};
			//gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_R, array2);
			//gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, array2);
			//gl.TexParameter(OpenGL.GL_TEXTURE_2D,OpenGL.GL_TEXTURE_MIN_LOD,-10000);
			TexturesInitialised = true;
			
		}
		
		private void refreshTexture()
		{
			//OpenGL gl = openGLControl.OpenGL;
			//gl.GenTextures(1,gtexture);

		}

		private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
		{
			//  Get the OpenGL object.
			OpenGL gl = openGLControl.OpenGL;
			
			//  Clear the color and depth buffer.
			gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

			//  Load the identity matrix.
			gl.LoadIdentity();
			
			if (!TexturesInitialised)
			{
				InitialiseTexture(ref gl);
			}
			gl.PushMatrix();
			gl.Enable(OpenGL.GL_TEXTURE_2D);
			gl.BindTexture(OpenGL.GL_TEXTURE_2D, gtexture[0]);
			gl.Color(1.0f, 1.0f, 1.0f, 0.1f); //Must have, weirdness!
			gl.Begin(OpenGL.GL_QUADS);
			gl.TexCoord(1.0, 1.0, 0.0);
			gl.Vertex(this.Width, this.Height, 1.0f);

			gl.TexCoord(0.0, 1.0,0.0);
			gl.Vertex(0.0f, this.Height, 1.0f);
			gl.TexCoord(0.0, 0.0, 0.0);
			gl.Vertex(0.0f, 0.0f, 1.0f);
			gl.TexCoord(1.0, 0.0, 0.0);
			gl.Vertex(this.Width, 0.0f, 1.0f);
			
			gl.End();
			gl.Flush();
			gl.Disable(OpenGL.GL_TEXTURE_2D);
			
			gl.PopMatrix();
			TexturesInitialised=false;
			
			
			
		}

		private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
		{
			OpenGL gl = openGLControl.OpenGL;
			gl.ClearColor(0, 0, 0, 0);
		}

		private void openGLControl_Resized(object sender, EventArgs e)
		{
			OpenGL gl = openGLControl.OpenGL;
			gl.MatrixMode(OpenGL.GL_PROJECTION);
			gl.LoadIdentity();
			//gl.Perspective(60.0f, (double)Width / (double)Height, 1, 20000.0);
			calculateProjection(60.0f,(double)Width / (double)Height,20000,1.0);
			gl.MultMatrix(projection_matrix);
			//gl.Perspective(60.0f, aspectRatio, 1000000, 1.0);

			//gl.LookAt(0, 0, -500, 0, 0, 0, 0, 1, 0);
			//gl.LookAt(1 * (this.Width / 2), this.Height / 2, -350, 1 * (this.Width / 2), this.Height / 2, 0, 0, -1, 0);
			//gl.LookAt(this.Width / 2, this.Height / 2, -2.995*(Height / 2) / Math.Tan((Math.PI / 180) * 60), this.Width / 2, this.Height / 2, 0, 0, -1, 0);
			gl.LookAt(this.Width / 2, this.Height / 2, -1.48 * (Height / 2) / Math.Tan((Math.PI / 180) * 60), this.Width / 2, this.Height / 2, 0, 0, -1, 0);
			gl.MatrixMode(OpenGL.GL_MODELVIEW);
			
			
		}
		private void calculateProjection(float fovy, double aspectRatio, double zFar, double zNear)
		{
			float[,] projmatrix = new float[4,4];
			double f = 1/Math.Tan(((double)(Math.PI/180)*(double)fovy)/2);
			for(int i = 0; i<(projmatrix.Length/4);i++){
				for(int j = 0; j<(projmatrix.Length/4);j++){
						projmatrix[i,j]=0;
				}
			}
			projmatrix[0,0] = (float)f/(float)aspectRatio;
			projmatrix[1,1] = (float)f;
			projmatrix[2,2] = (float)(zFar+zNear)/(float)(zNear-zFar);
			projmatrix[3,2] = (float)-1;
			projmatrix[2,3] = (float)(2*zFar*zNear)/(float)(zNear-zFar);
			for (int i = 0; i < (projmatrix.Length / 4); i++)
			{
				for (int j = 0; j < (projmatrix.Length / 4); j++)
				{
					int pos = (i*4)+j;
					projection_matrix[pos]=projmatrix[i,j];
				}
			}


		}
		public void repaintGraphics()
		{
			if (toWrite != null)
			{
				gImage1 = toWrite;
				TexturesInitialised = false;
			}
			Color newCol = gImage1.GetPixel(0,0);
			
		}
		public void updateFrameBuffer(Image inp)
		{
			toWrite=new Bitmap(inp,Width,Height);

		}
		private void openGLControl_Load(object sender, EventArgs e)
		{

		}

		private void SharpGLForm_Load(object sender, EventArgs e)
		{

		}

		private void SharpGLForm_MouseDown(object sender, MouseEventArgs e)
		{
			int[] adjusted = getAdjustedPoint(e.X,e.Y);

			Program.glApp.fillRectangle(new Rectangle(e.X, e.Y, 10, 10), Color.Orange);
		}
		private int[] getAdjustedPoint(int x, int y)
		{
			OpenGL gl = openGLControl.OpenGL;;
			//int[] xy = new int[]{(int)(x*xratio),(int)(y*yratio)};
			//return xy;
			float x1 = (2.0f*x)/Width -1.0f;
			float y1 = 1.0f-(2.0f*y)/Height;
			float z1 = 1.0f;
			Vector3 vec3 = new Vector3(x1,y1,z1);
			Vector4 vec4 = new Vector4(vec3,-1.0f);
			float[] projmatrixdat = new float[16];
			projmatrixdat = projection_matrix;
			Matrix4x4 m2 = new Matrix4x4();
			
			//AForge.Math.
			Matrix4x4 m3 = new Matrix4x4(vec4.X,vec4.Y, vec4.Z,vec4.W,0,0,0,0,0,0,0,0,0,0,0,0);
			Matrix4x4 m1 = new Matrix4x4(projmatrixdat[0],projmatrixdat[1],projmatrixdat[2],projmatrixdat[3],projmatrixdat[4],projmatrixdat[5],projmatrixdat[6],projmatrixdat[7],projmatrixdat[8],projmatrixdat[9],projmatrixdat[10],projmatrixdat[11],projmatrixdat[12],projmatrixdat[13],projmatrixdat[14],projmatrixdat[15]);
			Matrix4x4.Invert(m1,out m2);
			m2 = Matrix4x4.Multiply(m2,m3);
			Vector4 ray_eye = new Vector4(m2.M11,m2.M12,-1.0f,0.0f);
			

			return null;
		}
	}
}

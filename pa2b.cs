using System;
using System.Drawing;
using static System.Console;
using static System.Math;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Bme121
{
	// Retina object holds a colour array
	// Change retina so it holds onto a smaller array of tiles
	// Must also hold Width and Height
	public class Retina
	{
		public class Tile
		{
			public const int Len = 8;
			public Color[,] Pixels {get; set;}
			
			public Tile()
			{
				Pixels = new Color[Len, Len];
			}
		}
		
		public Tile[,] Tiles {get; set;}
		public int Height {get; private set;}
		public int Width {get; private set;}
		
		
		public Retina(string path)
		{
			Image< Rgba32 > img6L = Image.Load< Rgba32 >(path);
		
			Height = img6L.Height;
			Width = img6L.Width;
			
			int tileRows = (int) Ceiling((double) Height / Tile.Len);
			int tileCols = (int) Ceiling((double) Width / Tile.Len);	
				
			Tiles = new Tile[ tileRows, tileCols ];
			
			for (int tileRow = 0; tileRow < tileRows; tileRow++)
			{
				for (int tileCol = 0; tileCol < tileCols; tileCol++)
				{
					Tiles [tileRow,tileCol] = new Tile ();
				
					for (int row = 0; row < Tile.Len; row++)
					{
						for (int col = 0; col < Tile.Len; col++) // in save to file, cut off extra pixels 
						{
							if (tileRow * Tile.Len + row >= Height || tileCol * Tile.Len + col >= Width) 
							{	
								Tiles [tileRow, tileCol].Pixels[row,col] = Color.FromArgb(255,0,0,0);
							}
							else 
							{
								Rgba32 p =  img6L[tileRow * Tile.Len + row, tileCol * Tile.Len + col];
								Color c = Color.FromArgb(p.A, p.R, p.G, p.B);
							
								Tiles [tileRow, tileCol].Pixels[row, col] = c;
							}
						}	
					}	
				}
			}
		}
		
		public void SaveToFile(string path) 
		{
			// doing the opposite; convert retina to img6L
			Image< Rgba32 > image = new Image< Rgba32> (Height, Width);
			
			//~ int tileRows = (int) Ceiling( (double) Height / Tile.Len);
			//~ int tileCols = (int) Ceiling( (double) Width / Tile.Len);	
			
			for (int tileRow = 0; tileRow < Tiles.GetLength(0); tileRow++)
			{
				for (int tileCol = 0; tileCol < Tiles.GetLength(1); tileCol++)
				{
					for (int row = 0; row < Tile.Len; row++)
					{
						for (int col = 0; col < Tile.Len; col++)
						{
							if (tileRow * Tile.Len + row < Height && tileCol * Tile.Len + col < Width) 
							{	
								Color c = Tiles[tileRow,tileCol].Pixels[row,col];
								Rgba32 p = new Rgba32 (c.R, c.G, c.B, c.A);
								image [tileRow * Tile.Len + row, tileCol * Tile.Len + col] = p;
							}
						}
					}	
				}
			}
			image.Save(path);	
		}
	}
	
    static class Program
    {
        static void Main( )
        {
			const string path = "retina.png";
			Retina retina = new Retina(path);
			retina.SaveToFile("./test2.png");
        }
    }
}

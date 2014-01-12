using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;

namespace Game_of_Generals {
	public class Piece {
		private int rank;
		private Image img;
		private bool onBoard;
		private int player;
		private BitmapImage blank;
		private BitmapImage face;
		private int x, y;
		public int X {
			get {
				return x;
			}
		}
		public int Y {
			get {
				return y;
			}
		}

		public bool dead = false;

		public Piece(int r, int p) {
			x = y = -1;
			rank = r;
			player = p;
			onBoard = false;
			img = new Image();
			blank = new BitmapImage(new Uri("pack://application:,,,/Game of Generals;component/pieces/empty" + (player == 0 ? "" : "r") + ".png", UriKind.Absolute));
			face = new BitmapImage(new Uri("pack://application:,,,/Game of Generals;component/pieces/" + rank.ToString() + (player == 0 ? "" : "r") + ".png", UriKind.Absolute));
			img.Source = face;
			img.Stretch = Stretch.Uniform;
			img.MouseUp += img_MouseUp;
		}

		void img_MouseUp(object sender, MouseButtonEventArgs e) {
			if (onBoard) {
				//Piece is on board, init moving
				if (MainWindow.moving) {
					if (player != MainWindow.movedPiece.getPlayer()) {
						MainWindow.movedPiece.Position = this.Position;

						Grid dead = new Grid();
						switch (Rules.stronger(this, MainWindow.movedPiece)) {
							case 0:
								this.dead = true;
								this.Parent = dead;
								MainWindow.movedPiece.dead = true;
								MainWindow.movedPiece.Parent = dead;
								break;
							case 1:
								MainWindow.movedPiece.dead = true;
								MainWindow.movedPiece.Parent = dead;
								break;
							case 2:
								this.dead = true;
								this.Parent = dead;
								break;
							default:
								break;
						}
					}
					MainWindow.moved = true;
					MainWindow.moving = false;
					MainWindow.movedPiece = null;
				} else if (!MainWindow.moved && player == MainWindow.CurrentPlayer) {
					//TODO:	Highlight destinations
					MainWindow.moving = true;
					MainWindow.movedPiece = this;
				}
			} else {
				if (MainWindow.placementColumn != -1 && MainWindow.placementRow != -1) {
					Position = new int[2] { MainWindow.placementColumn, MainWindow.placementRow };
					onBoard = true;
					Parent = (Grid)Application.Current.MainWindow.FindName("pnlBoardGrid");
					MainWindow.players[player].onBoardPieces += 1;
					MainWindow.placementColumn = -1;
					MainWindow.placementRow = -1;
				}

				//Set color to black on rect
				//piece is not on board, finish placement
			}
		}

		public int getRank() {
			return rank;
		}

		public int getPlayer() {
			return player;
		}

		public void flip(bool faceup) {
			if (!faceup) {
				img.Source = blank;
				faceup = false;
			} else {
				img.Source = face;
				faceup = true;
			}
		}

		public bool OnBoard {
			get {
				return onBoard;
			}
			set {
				onBoard = value;
			}
		}
		public int[] Position {
			get {
				return new int[2] { Grid.GetColumn(img), Grid.GetRow(img) };
			}
			set {
				x = value[0];
				y = value[1];
				Grid.SetColumn(img, value[0]);
				Grid.SetRow(img, value[1]);
			}
		}

		public Grid Parent {
			set {
				Grid grid = this.img.Parent as Grid;
				if (grid != null) {
					grid.Children.Remove(this.img);
				}
				value.Children.Add(img);
			}
		}

	}
}

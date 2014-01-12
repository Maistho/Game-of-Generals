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
using System.ComponentModel;

namespace Game_of_Generals {
	public class Piece : INotifyPropertyChanged{
		private int rank;
//		private Image img;
		private bool onBoard;
		private int player;
//		private BitmapImage blank;
//		private BitmapImage face;
		private int xpos, ypos;
        private string image;
        private string blankImage;
        private string faceImage;


        public int Player {
            get {
                return player;
            }
        }
        public string Image {
            get {
                return image;
            }
            set {
                if (this.image != value) {
                    this.image = value;
                    this.NotifyPropertyChanged("Image");
                }
            }
        }

		public int X {
			get {
				return xpos;
			}
            set {
                if (this.xpos != value) {
                    this.xpos = value;
                    this.NotifyPropertyChanged("X");
                }
            }
		}
		public int Y {
			get {
				return ypos;
			}
            set {
                if (this.ypos != value) {
                    this.ypos = value;
                    this.NotifyPropertyChanged("Y");
                }
            }
		}

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }

        }


		public Piece(int x, int y, int r, int p) {
            xpos = x;
            ypos = y;
			rank = r;
			player = p;
            blankImage = "pieces/empty" + (player == 0 ? "" : "r") + ".png";
            faceImage = "pieces/" + rank.ToString() + (player == 0 ? "" : "r") + ".png";
            this.Image = faceImage;
			/*img = new Image();
			blank = new BitmapImage(new Uri("pack://application:,,,/Game of Generals;component/pieces/empty" + (player == 0 ? "" : "r") + ".png", UriKind.Absolute));
			face = new BitmapImage(new Uri("pack://application:,,,/Game of Generals;component/pieces/" + rank.ToString() + (player == 0 ? "" : "r") + ".png", UriKind.Absolute));
			img.Source = face;
			img.Stretch = Stretch.Uniform;
			img.MouseUp += img_MouseUp;*/
		}

		void piece_MouseUp(object sender, MouseButtonEventArgs e) {
			//Piece is on board, init moving
            if (MainWindow.moving) {
                if (player != MainWindow.movedPiece.getPlayer()) {
                    MainWindow.movedPiece.X = this.X;
                    MainWindow.movedPiece.Y = this.Y;

                    switch (Rules.stronger(this, MainWindow.movedPiece)) {
                        case 0:
                            MainWindow.boardPieces.Remove(this);
                            MainWindow.boardPieces.Remove(MainWindow.movedPiece);
                            break;
                        case 1:
                            MainWindow.boardPieces.Remove(MainWindow.movedPiece);
                            break;
                        case 2:
                            MainWindow.boardPieces.Remove(this);
                            break;
                        default:
                            break;
                    }
                }
                MainWindow.moved = true;
                MainWindow.moving = false;
                MainWindow.movedPiece = null;
            } else if (!MainWindow.moved && player == MainWindow.CurrentPlayer) {
                //TODO:	Highlight destinations, or should we?
                MainWindow.moving = true;
                MainWindow.movedPiece = this;
            }
			/*} else {
				if (MainWindow.placementColumn != -1 && MainWindow.placementRow != -1) {
                    X = MainWindow.placementColumn;
                    Y = MainWindow.placementRow;
					Parent = (Grid)Application.Current.MainWindow.FindName("pnlBoardGrid");
					MainWindow.players[player].onBoardPieces += 1;
					MainWindow.placementColumn = -1;
					MainWindow.placementRow = -1;
				}

				//Set color to black on rect
				//piece is not on board, finish placement
			}*/
		}

		public int getRank() {
			return rank;
		}

		public int getPlayer() {
			return player;
		}

		public void flip(bool faceup) {
			if (faceup) {
				Image = faceImage;
			} else {
				Image = blankImage;
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
	}
}

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
		public int Id { get; set; }
		private int rank;
		//private Image img;
		//private bool onBoard;
		private int player;
		//private BitmapImage blank;
		//private BitmapImage face;
        //private BitmapImage image;
		private int xpos, ypos;
        private string image;
        private string blank;
        private string face;


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

		public Piece() { }
		public Piece(int x, int y, int r, int p) {
            xpos = x;
            ypos = y;
			rank = r;
			player = p;
            blank = "pieces/empty" + (player == 0 ? "" : "r") + ".png";
            face = "pieces/" + rank.ToString() + (player == 0 ? "" : "r") + ".png";
            this.Image = face;

			/*img = new Image();

			blank = new BitmapImage(new Uri("pack://application:,,,/Game of Generals;component/pieces/empty" + (player == 0 ? "" : "r") + ".png", UriKind.Absolute));
			face = new BitmapImage(new Uri("pack://application:,,,/Game of Generals;component/pieces/" + rank.ToString() + (player == 0 ? "" : "r") + ".png", UriKind.Absolute));
            Image = face;

			img.Source = face;
			img.Stretch = Stretch.Uniform;
			img.MouseUp += img_MouseUp;*/
		}



		public int getRank() {
			return rank;
		}

		public int getPlayer() {
			return player;
		}

		public void flip(bool faceup) {
			if (faceup) {
				Image = face;
			} else {
				Image = blank;
			}
		}

/*		public bool OnBoard {
			get {
				return onBoard;
			}
			set {
				onBoard = value;
			}
		}*/
	}
}

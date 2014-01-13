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
using System.Data.Entity.ModelConfiguration;

namespace Game_of_Generals {
	public class Piece : INotifyPropertyChanged{
		public int Id { get; set; }
		private int rank { get; set; }
		private int player { get; set; }
		private int xpos, ypos;
		private string image { get; set; }
		private string blank { get; set; }
		private string face { get; set; }
		public Piece() { }
		public Piece(int x, int y, int r, int p) {
			xpos = x;
			ypos = y;
			rank = r;
			player = p;
			blank = "pieces/empty" + (player == 0 ? "" : "r") + ".png";
			face = "pieces/" + rank.ToString() + (player == 0 ? "" : "r") + ".png";
			this.Image = face;
		}
		public class PieceConfiguration : EntityTypeConfiguration<Piece> {
			public PieceConfiguration() {
				Property(p => p.rank);
				Property(p => p.player);
				Property(p => p.blank);
				Property(p => p.face);
			}
		}
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
	}
}

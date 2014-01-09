using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game_of_Generals {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

		private Player[] players = {new Player(new Grid(),true), new Player(new Grid(),false)};
		private int currentPlayer;
		private int rows, columns;
		private Rectangle lastRect;
		public static int placementColumn, placementRow;
        public MainWindow() {
            InitializeComponent();
			DataContext = this;
			players[0].placementGrid = pnlP1PiecesGrid;
			players[1].placementGrid = pnlP2PiecesGrid;
			currentPlayer = 0;
			rows = 8;
			columns = 9;
            paintGrid();
			populatePlacement(0);
			populatePlacement(1);
        }

        private void paintGrid() {

			//Add grid definitions to the boardGrid
            for (int i = rows; i > 0; --i) {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
				pnlBoardGrid.RowDefinitions.Add(row);
			} for (int i = columns; i > 0; --i) {
				ColumnDefinition column = new ColumnDefinition();
				column.Width = new GridLength(1, GridUnitType.Star);
				pnlBoardGrid.ColumnDefinitions.Add(column);
			}

			//Add grid definitions to the player placementGrids
			for (int i = rows / 2; i > 0; --i) {
				RowDefinition row = new RowDefinition();
				row.Height = new GridLength(1, GridUnitType.Star);
				pnlP1PiecesGrid.RowDefinitions.Add(row);
			} for (int i = rows / 2; i > 0; --i) {
				RowDefinition row = new RowDefinition();
				row.Height = new GridLength(1, GridUnitType.Star);
				pnlP2PiecesGrid.RowDefinitions.Add(row);
			} for (int i = columns; i > 0; --i) {
				ColumnDefinition column = new ColumnDefinition();
				column.Width = new GridLength(1, GridUnitType.Star);
				pnlP1PiecesGrid.ColumnDefinitions.Add(column);
			} for (int i = columns; i > 0; --i) {
				ColumnDefinition column = new ColumnDefinition();
				column.Width = new GridLength(1, GridUnitType.Star);
				pnlP2PiecesGrid.ColumnDefinitions.Add(column);
			}

			//Fill the grid with rectangles
            for (int i = pnlBoardGrid.RowDefinitions.Count(); i >= 0; --i) {
                for (int j = pnlBoardGrid.ColumnDefinitions.Count(); j >= 0; --j) {
                    Rectangle rect = new Rectangle();
                    rect.Fill = Brushes.Black;
                    pnlBoardGrid.Children.Add(rect);
                    Grid.SetColumn(rect, j);
                    Grid.SetRow(rect, i);
					rect.Stroke = Brushes.Green;
					rect.MouseUp += rect_MouseUp;
                }
            }
			Image img = new Image();
			img.Source = new BitmapImage(new Uri("pack://application:,,,/Game of Generals;component/pieces/0r.png", UriKind.Absolute));
			img.Stretch = Stretch.Uniform;
			pnlBoardGrid.Children.Add(img);
			Grid.SetColumn(img, 1);
			Grid.SetRow(img, 2);
			img.MouseUp += img_MouseUp;

        }

		private void populatePlacement(int player) {
			int i = 0;
			int j = 0;
			foreach (Piece piece in players[player].pieces) {
				piece.Parent = players[player].placementGrid;
				piece.Position = new int[2] { i, j };
				i = (i + 1) % columns;
				if (i == 0) {
					++j;
				}
			}
		}

		void img_MouseUp(object sender, MouseButtonEventArgs e) {
			Image img = sender as Image;
			int column = Grid.GetColumn(img);
			int row = Grid.GetRow(img);
			//Do stuff like moving the image

		}

		void rect_MouseUp(object sender, MouseButtonEventArgs e) {
			if(lastRect != null) lastRect.Fill = Brushes.Black;
			Rectangle rect = sender as Rectangle;
			if (players[currentPlayer].pieces.Count() - players[currentPlayer].onBoardPieces != 0) {
				players[currentPlayer].placementGrid.Visibility = System.Windows.Visibility.Visible;
				rect.Fill = Brushes.LightGreen;
			} else {
				players[currentPlayer].placementGrid.Visibility = System.Windows.Visibility.Hidden;
			}
			lastRect = rect;
			placementColumn = Grid.GetColumn(rect);
			placementRow = Grid.GetRow(rect);
		}
    }

    public class Player {
        public ObservableCollection<Piece> pieces = new ObservableCollection<Piece>();
		public Grid placementGrid;
		public int onBoardPieces;

        public Player(Grid pGrid, bool colour) {
			placementGrid = pGrid;
			onBoardPieces = 0;
            //TODO: Ask rule engine how many different pieces
            for(int i = 0; i <= 14; ++i) {
				for(int j = 1; j > 0; --j) {
					//TODO: Ask rule engine how many pieces of current rank to add
					pieces.Add(new Piece(this, i, colour));
				}
            }
        }
    }

    public class Piece {
        private int rank;
        private bool colour; //True is green
        private Image img;
        private bool onBoard;
		private Player player;


        public Piece(Player p, int r, bool c) {
			player = p;
            rank = r;
            colour = c;
            onBoard = false;
            img = new Image();
			img.Source = new BitmapImage(new Uri("pack://application:,,,/Game of Generals;component/pieces/" + rank.ToString() + (colour ? "" : "r") + ".png", UriKind.Absolute));
            img.Stretch = Stretch.Uniform;
			img.MouseUp += img_MouseUp;
        }

		void img_MouseUp(object sender, MouseButtonEventArgs e) {
			if (onBoard) {
				//Piece is on board, init moving
			} else {
				Position = new int[2] { MainWindow.placementColumn, MainWindow.placementRow };
				onBoard = true;
				Parent = (Grid)Application.Current.MainWindow.FindName("pnlBoardGrid");
				player.onBoardPieces += 1;
				//Set color to black on rect
				//piece is not on board, finish placement
			}
        }

        public int getRank() {
            return rank;
		}

        public bool getColour() {
            return colour;
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

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

		public static Player[] players = { new Player(new Grid(), 0), new Player(new Grid(), 1) };
		private static int currentPlayer;
		private int rows, columns;
		private Rectangle lastRect;
        public static bool gameEnd;
        public static int winner;
		public static bool moving;
		public static Piece movedPiece;
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

		public static int CurrentPlayer {
			get { return currentPlayer; }
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

		void rect_MouseUp(object sender, MouseButtonEventArgs e) {
			Rectangle rect = sender as Rectangle;
			if (moving) {
				int[] newMove = new int[2] { Grid.GetColumn(rect), Grid.GetRow(rect) };
				if (Rules.legalMove(movedPiece, newMove)) {
					movedPiece.Position = newMove;
				}
				moving = false;
			} else {
				if (lastRect != null) lastRect.Fill = Brushes.Black;
				if (players[currentPlayer].pieces.Count() - players[currentPlayer].onBoardPieces != 0) {
					players[currentPlayer].placementGrid.Visibility = System.Windows.Visibility.Visible;

					if (Grid.GetColumn(rect) >= 0 && Grid.GetRow(rect) >= 0) {
						placementColumn = Grid.GetColumn(rect);
						placementRow = Grid.GetRow(rect);
						rect.Fill = Brushes.LightGreen;
					}
					lastRect = rect;
				} else {
					players[currentPlayer].placementGrid.Visibility = System.Windows.Visibility.Hidden;
				}
			}
		}

		private void finishButton_Click(object sender, RoutedEventArgs e) {
			Button btn = sender as Button;
			if (switchRectangle.Visibility == Visibility.Visible) {
				foreach (Piece piece in players[currentPlayer].pieces) {
					if (piece.OnBoard) piece.flip(true);
				}
				switchRectangle.Visibility = Visibility.Hidden;
				btn.Content = "Finish Turn";
			} else {
				switchRectangle.Visibility = Visibility.Visible;
				foreach (Piece piece in players[currentPlayer].pieces) {
					if (piece.OnBoard) piece.flip(false);
				}
				players[currentPlayer].placementGrid.Visibility = Visibility.Hidden;
				btn.Content = "Begin Turn";
				currentPlayer = (currentPlayer + 1) % 2;
			}

		}
	}

	public class Player {
		public ObservableCollection<Piece> pieces = new ObservableCollection<Piece>();
		public Grid placementGrid;
		public int onBoardPieces;

		public Player(Grid pGrid, int player) {
			placementGrid = pGrid;
			onBoardPieces = 0;
			int numRanks = Rules.numberOfRanks();
			for (int i = 0; i <= numRanks; ++i) {
				int numPieces = Rules.numberOfPieces(i);
				for (int j = 0; j < numPieces; ++j) {
					pieces.Add(new Piece(i, player));
				}
			}
		}
	}

	public class Piece {
		private int rank;
		private Image img;
		private bool onBoard;
		private int player;
		private BitmapImage blank;
		private BitmapImage face;


		public Piece(int r, int p) {
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
                                this.Parent = dead;
                                MainWindow.movedPiece.Parent = dead;
                                break;
                            case 1:
                                MainWindow.movedPiece.Parent = dead;
                                break;
                            case 2:
                                this.Parent = dead;
                                break;
                            case 3:
                                MainWindow.gameEnd = true;
                                MainWindow.winner = this.getPlayer();
                                break;
                            case 4:
                                MainWindow.gameEnd = true;
                                MainWindow.winner = this.getPlayer();
                                break;
                            case 5:
                                MainWindow.gameEnd = true;
                                MainWindow.winner = MainWindow.movedPiece.getPlayer();
                                break;
                            default:
                                break;
                        }
						//TODO: Ask rules engine which piece to remove
					}
					MainWindow.moving = false;
					MainWindow.movedPiece = null;
				} else if (player == MainWindow.CurrentPlayer) {
					//TODO: Ask rules engine about legal destinations
					//Highlight destinations
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

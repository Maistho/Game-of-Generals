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

		private Player player1, player2;
		private int currentPlayer;
		private int rows, columns;
		private int placementColumn, placementRow;
        public MainWindow() {
            InitializeComponent();
            DataContext = this;
			player1 = new Player();
			player2 = new Player();
			currentPlayer = 1;
			rows = 8;
			columns = 9;
            paintGrid();
			populatePlacement();
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
                    rect.MouseEnter += rect_MouseEnter;
                    rect.MouseLeave += rect_MouseLeave;
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

		private void populatePlacement() {
			int i = 0;
			int j = 0;
			foreach (Piece piece in player1.pieces) {
				piece.Parent = pnlP1PiecesGrid;
				piece.Position = new int[2] { i, j };
				i = (i + 1) % columns;
				if (i == 0) {
					++j;
				}
			}
			i = 0;
			j = 0;
			foreach (Piece piece in player2.pieces) {
				piece.Parent = pnlP2PiecesGrid;
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
			if (currentPlayer == 1 && player1.pieces.Count() != 0) {
				pnlP1PiecesGrid.Visibility = System.Windows.Visibility.Visible;
			} else if(currentPlayer == 2 && player2.pieces.Count() != 0) {
				pnlP2PiecesGrid.Visibility = System.Windows.Visibility.Visible;
			}

			Rectangle rect = sender as Rectangle;
			placementColumn = Grid.GetColumn(rect);
			placementRow = Grid.GetRow(rect);
		}

        void rect_MouseLeave(object sender, MouseEventArgs e) {
            Rectangle rect = sender as Rectangle;
            rect.Fill = Brushes.Black;
        }

        private void rect_MouseEnter(object sender, MouseEventArgs e) {
            Rectangle rect = sender as Rectangle;
            rect.Fill = Brushes.Blue;
        }
    }

    public class Player {
        public ObservableCollection<Piece> pieces = new ObservableCollection<Piece>();

        public Player() {
            //TODO: Ask rule engine how many different pieces
            for(int i = 0; i <= 14; ++i) {
				for(int j = 1; j > 0; --j) {
					//TODO: Ask rule engine how many pieces of current rank to add
					pieces.Add(new Piece(i, true));
				}
            }
        }
    }

    public class Piece {
        private int rank;
        private bool colour; //True is green
        private Image img;
        private bool onBoard;


        public Piece(int r, bool c) {
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
				//piece is not on board, init placement
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
                value.Children.Add(img);
            }
        }

    }
}

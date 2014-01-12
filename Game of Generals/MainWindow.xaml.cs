﻿using System;
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

		//public static Player[] players = { new Player(new Grid(), 0), new Player(new Grid(), 1) };
		private static int currentPlayer;
		private int rows, columns;
		private Rectangle lastRect;
        public static bool gameEnd;
        public static int winner;
		public static bool moving;
		public static Piece movedPiece;
		public static int placementColumn, placementRow;
		public static bool moved;
		public static ObservableCollection<Piece> deadPieces = new ObservableCollection<Piece>();
        public static ObservableCollection<Piece> boardPieces = new ObservableCollection<Piece>();
		public static Piece flag0, flag1;
		public MainWindow() {
			InitializeComponent();
			//flag0 = players[0].pieces.Single(x => x.getRank() == 0);
			//flag1 = players[1].pieces.Single(x => x.getRank() == 0);
			DataContext = this;
			//players[0].placementGrid = pnlP1PiecesGrid;
			//players[1].placementGrid = pnlP2PiecesGrid;
			currentPlayer = 0;
			rows = 8;
			columns = 9;
			paintGrid();
			//populatePlacement(0);
			//populatePlacement(1);
		}

		public static int CurrentPlayer {
			get { return currentPlayer; }
		}

		private void paintGrid() {

			/* (moved to XAML)
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
			*/

			/*  (moved to XAML)
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
			*/

			//Fill the grid with rectangles
			/*for (int i = pnlBoardGrid.RowDefinitions.Count(); i >= 0; --i) {
				for (int j = pnlBoardGrid.ColumnDefinitions.Count(); j >= 0; --j) {
					Rectangle rect = new Rectangle();
					rect.Fill = Brushes.Black;
					pnlBoardGrid.Children.Add(rect);
					Grid.SetColumn(rect, j);
					Grid.SetRow(rect, i);
					rect.Stroke = Brushes.Green;
					rect.MouseUp += rect_MouseUp;
				}
			}*/
		}

/*		private void populatePlacement(int player) {
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
		}*/

		void grid_MouseUp(object sender, MouseButtonEventArgs e) {
            Point position = e.GetPosition((Grid)sender);
            int[] clickedPos = new int[2] { (int)(position.X / ((Grid)sender).ColumnDefinitions[0].ActualWidth -1), (int)(position.X / ((Grid)sender).RowDefinitions[0].ActualHeight -1)};
            MessageBox.Show("You clicked at [" + clickedPos[0] + "," + clickedPos[1] + "].");

			if (moving) {
				if (Rules.legalMove(movedPiece, clickedPos)) {
                    movedPiece.X = clickedPos[0];
                    movedPiece.Y = clickedPos[1];
					moved = true;
				}
				moving = false;
			} else {
                /*if (Rules.canPlace()) {
                    if (players[currentPlayer].pieces.Count() - players[currentPlayer].onBoardPieces != 0) {
                        players[currentPlayer].placementGrid.Visibility = System.Windows.Visibility.Visible;

                        if (Rules.legalPlacement(currentPlayer, Grid.GetRow(rect))) {
                            placementColumn = Grid.GetColumn(rect);
                            placementRow = Grid.GetRow(rect);
                            rect.Fill = Brushes.LightGreen;
                        } else {
                            placementColumn = -1;
                            placementRow = -1;
                            rect.Fill = Brushes.PaleVioletRed;
                        }

                    } else {
                        players[currentPlayer].placementGrid.Visibility = System.Windows.Visibility.Hidden;
                    }
                }*/
			}
            //if (lastRect != null) lastRect.Fill = Brushes.Black;
            //lastRect = rect;
		}

		private void finishButton_Click(object sender, RoutedEventArgs e) {
			Button btn = sender as Button;
			
			if (switchRectangle.Visibility == Visibility.Visible) {
				int vic = Rules.victoryCheck(flag0, flag1);
				if (vic > 0) {
					MessageBox.Show("Player " + vic.ToString() + " has won!");
				}
				foreach (Piece piece in boardPieces) {
					if (piece.Player == currentPlayer) piece.flip(true);
				}
				moved = false;
				switchRectangle.Visibility = Visibility.Hidden;
				btn.Content = "Finish Turn";
                Rules.nextTurn();
            } else /*if (Rules.mayPass(MainWindow.players[MainWindow.CurrentPlayer]))*/ {
				switchRectangle.Visibility = Visibility.Visible;
				foreach (Piece piece in boardPieces) {
					if (piece.Player == currentPlayer) piece.flip(false);
				}
				//players[currentPlayer].placementGrid.Visibility = Visibility.Hidden;
				btn.Content = "Begin Turn";
				currentPlayer = (currentPlayer + 1) % 2;
			}

		}
	}

/*	public class Player {
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
					Piece piece = new Piece(i, player);
					if(i == 0 && player == 0){
						MainWindow.flag0 = piece;
					} else if(i == 0 && player == 1){
						MainWindow.flag1 = piece;
					}
					pieces.Add(piece);
				}
			}
		}
	}*/
}

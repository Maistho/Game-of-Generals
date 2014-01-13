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
using System.Data.Entity;

namespace Game_of_Generals {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public class GameContext : DbContext {
		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder
				.Configurations.Add(new Piece.PieceConfiguration());
			base.OnModelCreating(modelBuilder);
		}
		public DbSet<Game> games { get; set; }
	}
	public class Game {
		public Game() {	}
		public Game(ObservableCollection<Piece> p) {
			pieces = p;
		}
		public int gameId { get; set; }
		public virtual bool moved { get; set; }
		public virtual int currentPlayer { get; set; }
		public virtual ObservableCollection<Piece> pieces { get; set; }
	}
	public partial class MainWindow : Window {
        public static int winner;
		public static bool moving;
		public static Piece movedPiece;
		private static GameContext db;
		private static Game game;
		public MainWindow() {
			InitializeComponent();
			db = new GameContext();
			if (db.games.Count() == 0) {
				// If no saved games
				var boardPieces = new ObservableCollection<Piece>();
				game = new Game(boardPieces);
				db.games.Add(game);
				game.currentPlayer = 0;
			} else {
				// If saved games, then take the first one.
				game = db.games.First();
			}
			db.SaveChanges();
			DataContext = this;
			gameBoard.ItemsSource = game.pieces;
		}
        void piece_MouseUp(object sender, MouseButtonEventArgs e) {
            Piece clicked = ((Piece)(((Image)sender).DataContext));
            //Piece is on board, init moving
            if (moving) {
                if (clicked.Player != movedPiece.getPlayer()) {
                    movedPiece.X = clicked.X;
                    movedPiece.Y = clicked.Y;

                    switch (Rules.stronger(clicked, movedPiece)) {
                        case 0:
                            game.pieces.Remove(clicked);
                            game.pieces.Remove(movedPiece);
                            break;
                        case 1:
                            game.pieces.Remove(movedPiece);
                            break;
                        case 2:
                            game.pieces.Remove(clicked);
                            break;
                        default:
                            break;
                    }
                }
                game.moved = true;
                moving = false;
                movedPiece = null;
            } else if (!game.moved && clicked.Player == game.currentPlayer) {
                moving = true;
                movedPiece = clicked;
            }
            e.Handled = true;
        }
		void grid_MouseUp(object sender, MouseButtonEventArgs e) {
            Point position = e.GetPosition((Grid)sender);
            int[] clickedPos = new int[2] { (int)(position.X / ((Grid)sender).ColumnDefinitions[0].ActualWidth), (int)(position.Y / ((Grid)sender).RowDefinitions[0].ActualHeight)};
			if (moving) {
				if (Rules.legalMove(movedPiece, clickedPos)) {
                    movedPiece.X = clickedPos[0];
                    movedPiece.Y = clickedPos[1];
					game.moved = true;
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
		}
		private void finishButton_Click(object sender, RoutedEventArgs e) {
			Button btn = sender as Button;
            //TODO: Add function
		}
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			db.SaveChanges();
		}
	}
}

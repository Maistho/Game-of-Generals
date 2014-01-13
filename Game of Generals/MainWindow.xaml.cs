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
using System.ComponentModel;
using System.Collections;

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
		public DbSet<Piece> pieces { get; set; }
	}
	public class Game {
		public Game() {	}
		public Game(ObservableCollection<Piece> p) {
			pieces = p;
		}
		public int turn { get; set; }
		public int gameId { get; set; }
		public virtual int turn { get; set; }
		public virtual bool changingPlayers { get; set; }
		public virtual bool moved { get; set; }
		public virtual int currentPlayer { get; set; }
		public virtual ObservableCollection<Piece> pieces { get; set; }
	}
    public class piecePool : INotifyPropertyChanged {
        private int player;
        private Piece next;
        private int amount;
        private Stack<int[]> pieces;
        public piecePool(int p) {
            pieces = new Stack<int[]>();
            int numRanks = Rules.numberOfRanks();
            for (int i = 0; i <= numRanks; ++i) {
                int numPieces = Rules.numberOfPieces(i);
                pieces.Push(new int[2] { i, numPieces });
            }
            player = p;
            amount = 0;
            place();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public Piece NextPiece {
            get {
                return next;
            }
            set { }
        }
        public int Amount {
            get {
                return amount;
            }
            set { }
        }
        public void place() {
            amount--;
            if (amount <= 0) {
                if (pieces.Count > 0) {
                    int[] pieceData = pieces.Pop();
                    next = new Piece(-1, -1, pieceData[0], player);
                    amount = pieceData[1];
                } else {
                    amount = 0;
                    next = null;
                }
                this.NotifyPropertyChanged("NextPiece");
            }
            this.NotifyPropertyChanged("Amount");
        }
        public void NotifyPropertyChanged(string propName) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
	public partial class MainWindow : Window {
        public static int winner;
		public static bool moving = false;
		public static bool changingPlayers = false;
        public static bool placing = false;
        public static bool playing = false;
		public static Piece movedPiece;
        public static Piece nowPlacing;
		private static GameContext db;
		private static Game game;
        private piecePool[] piecePools;
		public MainWindow() {
			InitializeComponent();
			db = new GameContext();
			startGame();
			DataContext = this;
			gameBoard.ItemsSource = game.pieces;
		}
		private void startGame() {
			if (db.games.Count() == 0) {
				// If no saved games
				var boardPieces = new ObservableCollection<Piece>();
				game = new Game(boardPieces);
				db.games.Add(game);
				game.currentPlayer = 0;
				game.turn = 0;
                piecePools = new piecePool[2] { new piecePool(0), new piecePool(1) };
                pnlSideGrid.DataContext = piecePools[game.currentPlayer];
                gameBoard.ItemsSource = game.pieces;
                placing = true;
                playing = false;
				//TODO: Init placement
			} else {
				// If saved games, then take the first one.
				game = db.games.First();
                playing = true;
			}
			db.SaveChanges();
		}
		private void cleanUp() {
			db.games.Remove(game);
			db.pieces.RemoveRange(db.pieces);
			db.SaveChanges();
		}
		private void endGame(int winner) {
			if (winner > 0) {
				MessageBox.Show("Player " + winner.ToString() + " has won!", "Game over!");
				cleanUp();
				startGame();
				//TODO: Do cleanup stuff and show winner
			}
		}
        void piece_MouseUp(object sender, MouseButtonEventArgs e) {
            Piece clicked = ((Piece)(((Image)sender).DataContext));
            //Piece is on board, init moving
            if (moving && playing && Rules.legalMove(movedPiece, new int[] {clicked.X, clicked.Y}, game.turn)) {
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
				if (Rules.legalMove(movedPiece, clickedPos, game.turn)) {
                    movedPiece.X = clickedPos[0];
                    movedPiece.Y = clickedPos[1];
					game.moved = true;
				}
				moving = false;
			} else if (placing) {
                if (Rules.legalPlacement(game.currentPlayer, clickedPos[1])) {
                    nowPlacing = new Piece(clickedPos[0], clickedPos[1], piecePools[game.currentPlayer].NextPiece.getRank(), game.currentPlayer);
                    game.pieces.Add(nowPlacing);
                    piecePools[game.currentPlayer].place();
                    if (piecePools[game.currentPlayer].NextPiece == null) {
                        if (piecePools[(game.currentPlayer + 1) % 2].NextPiece == null) {
                            playing = true;
                        }
                        placing = false;
                    }
                } else {
                    MessageBox.Show("Illegal placement. You must place the piece within the three rows closest to your edge of the board.", "Oh no, not there!");
                }
			}
		}
		private void changeTurnButton_Click(object sender, RoutedEventArgs e) {
            if (placing) {
                MessageBox.Show("You must place all your pieces before passing the turn to the next player.", "Pieces left");
                return;
            }/* else if (!game.moved && changingPlayers) {
                MessageBox.Show("You must make a move before passing the turn to the next player.");
                return;
            }*/
			Button btn = sender as Button;
			bool flip = false;
			string content = "Begin turn";
			int player = game.currentPlayer;
			if (!game.changingPlayers) {
				game.changingPlayers = true;
				game.moved = false;
				game.turn++;
				game.currentPlayer = (game.currentPlayer + 1) % 2;
			} else {
				game.changingPlayers = false;
				flip = true;
				content = "End turn";
				var q = from g in game.pieces
						where g.getRank() == 0
						select g;
                if (playing) {
                    endGame(Rules.victoryCheck(q));
                } else {
                    placing = true;
                    pnlSideGrid.DataContext = piecePools[game.currentPlayer];
                }
			}
			foreach (var p in game.pieces) {
				if (p.getPlayer() == player) {
					p.flip(flip);
				}
			}
			btn.Content = content;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (!playing) {
                cleanUp();
            }
			db.SaveChanges();
		}
        private void surrenderButton_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to surrender?", "Coward.", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) {
                endGame(((game.currentPlayer + 1) % 2) + 1);
            }
        }
	}
}

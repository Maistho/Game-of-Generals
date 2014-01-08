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

        public MainWindow() {
            InitializeComponent();
            paintGrid();
        }

        private void paintGrid() {
            for (int i = 9; i > 0; --i) {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                pnlBoardGrid.RowDefinitions.Add(row);
            }
            for (int i = 8; i > 0; --i) {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(1, GridUnitType.Star);
                pnlBoardGrid.ColumnDefinitions.Add(column);
            }
            for (int i = pnlBoardGrid.RowDefinitions.Count(); i >= 0; --i) {
                for (int j = pnlBoardGrid.ColumnDefinitions.Count(); j >= 0; --j) {
                    Rectangle rect = new Rectangle();
                    rect.Fill = Brushes.Black;
                    pnlBoardGrid.Children.Add(rect);
                    Grid.SetColumn(rect, j);
                    Grid.SetRow(rect, i);
                    rect.MouseEnter += rect_MouseEnter;
                    rect.MouseLeave += rect_MouseLeave;
                }
            }

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
            for (int i = 0; i > 15; ++i) {
                //TODO: Ask rule engine how many pieces of current rank to add
                pieces.Add(new Piece(i));
            }
        }
    }

    public class Piece {
        private int rank;

        public Piece(int r) {
            rank = r;
        }

        public int getRank() {
            return rank;
        }
    }
}

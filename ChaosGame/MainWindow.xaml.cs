using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ChaosGame {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public MainWindow() {
            InitializeComponent();
            this.initialVertices = new List<Vector>();
            this.NumberOfVertices = 4;
            this.NumberOfIterations = 10;
            this.FractionalStep = .6;
        }

        private int _NumberOfIterations;
        public int NumberOfIterations {
            get { return _NumberOfIterations; }
            set {
                _NumberOfIterations = value;
                NotifyPropertyChanged();
            }
        }

        private double _FractionalStep;
        public double FractionalStep {
            get { return _FractionalStep; }
            set {
                _FractionalStep = value;
                NotifyPropertyChanged();
            }
        }

        private int _NumberOfVertices;
        public int NumberOfVertices {
            get { return _NumberOfVertices; }
            set {
                _NumberOfVertices = value;
                NotifyPropertyChanged();
            }
        }

        private List<Vector> initialVertices;
        private Vector currentPosition;

        private void reset() {
            this.canvas.Children.Clear();
        }

        private const double RADIUS = 200;

        private List<Vector> getInitialVertices(int numberOfVertices) {
            List<Vector> outputVectors = new List<Vector>();
            double theta = (Math.PI * 2) / numberOfVertices;
            for (int i = 0; i < numberOfVertices; i++) {
                double x = RADIUS * Math.Cos(theta * i);
                double y = RADIUS * Math.Sin(theta * i);
                Vector v = new Vector(x, y);
                outputVectors.Add(v);
            }
            return outputVectors;
        }

        private void draw(Vector v) {
            Ellipse e = new Ellipse() {
                Width = 1,
                Height = 1,
                Fill = Brushes.Black,  
            };

            this.canvas.Children.Add(e);
            var x= v.X + RADIUS;
            var y = v.Y + RADIUS;
            Canvas.SetLeft(e, x);
            Canvas.SetTop(e, y);
        }

        private static Random rand = new Random();

        private Vector getRandomPosition(double maxVal) {
            double x = rand.NextDouble() * maxVal;
            double y = rand.NextDouble() * maxVal;
            return new Vector(x, y);
        }

        private Vector pickRandomVertex() {
            int idx = rand.Next(this.NumberOfVertices);
            return this.initialVertices[idx];
        }

        

        private void iterate() {
            var targetVertex = pickRandomVertex();
            var dx = (targetVertex.X - currentPosition.X) * this.FractionalStep;
            var dy = (targetVertex.Y - currentPosition.Y) * this.FractionalStep;
            double newX = currentPosition.X + dx;
            double newY = currentPosition.Y + dy;
            this.currentPosition = new Vector(newX, newY);
            this.draw(this.currentPosition);
        }

        private void Run_Click(object sender, RoutedEventArgs e) {
            this.reset();
            this.initialVertices = this.getInitialVertices(this.NumberOfVertices);
            this.currentPosition = getRandomPosition(RADIUS);
            for (int i = 0; i < this.NumberOfIterations; i++) {
                iterate();
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged Implementation
    }
}

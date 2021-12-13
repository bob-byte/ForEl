using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FOREL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const Int32 COUNT_ELEMS = 30;

        private const Int32 COUNT_LIMITS = 3;

        private const Double UPPER_LIMIT_MASS = 4;
        private const Double LOWER_LIMIT_MASS = 2;

        private const Int32 STROKE_THICKNESS = 3;

        private readonly Double[] m_upperLimitX = { 30, 105, 200 };
        private readonly Double[] m_lowerLimitX = { 0, 75, 170};

        private readonly Double[] m_upperLimitY = { 30, 105, 200 };
        private readonly Double[] m_lowerLimitY = { 0, 75, 170 };

        private readonly SolidColorBrush[] m_colors =
        {
            Brushes.Black,
            Brushes.Red,
            Brushes.Yellow,
            Brushes.LawnGreen,
            Brushes.DarkBlue,
            Brushes.DarkGreen,
            Brushes.Chocolate
        };

        private readonly List<Element> m_sample = new(COUNT_ELEMS);

        private List<Cluster> m_clusterList = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Random_Click(Object sender, RoutedEventArgs e)
        {
            Random random = new();
            for (Int32 numElem = 0; numElem < COUNT_ELEMS; numElem++)
            {
                Element elem = new Element
                {
                    Mass = random.NextDouble(LOWER_LIMIT_MASS, UPPER_LIMIT_MASS),
                    X = random.NextDouble(m_lowerLimitX[numElem / (COUNT_ELEMS / COUNT_LIMITS)], m_upperLimitX[numElem / (COUNT_ELEMS / COUNT_LIMITS)]),
                    Y = random.NextDouble(m_lowerLimitY[numElem / (COUNT_ELEMS / COUNT_LIMITS)], m_upperLimitY[numElem / (COUNT_ELEMS / COUNT_LIMITS)]),
                    Color = m_colors[random.Next(0, m_colors.Length)]
                };

                m_sample.Add(elem);
                plot.Children.Add(Representation.GetCircle(elem.X, elem.Y, 
                    elem.Mass, elem.Color, STROKE_THICKNESS));
            }

            plot.ShowInLimitX(m_lowerLimitX.Min(), m_upperLimitX.Max());
            plot.ShowInLimitY(m_lowerLimitY.Min(), m_upperLimitY.Max());
        }

        private void RunForelAlgorithm_Click(Object sender, RoutedEventArgs e)
        {
            Forel forel = new();
            Double radiusSphere = Convert.ToDouble(T_B_RadiusSphere.Text);
            m_clusterList = forel.Clustering(radiusSphere, m_sample);

            plot.Children.Clear();

            foreach (Element elem in m_sample)
            {
                plot.Children.Add(Representation.GetCircle(elem.X, elem.Y,
                    elem.Mass, elem.Color, STROKE_THICKNESS));
                elem.Mass *= elem.X + elem.Y;
            }

            foreach (Cluster cluster in m_clusterList)
            {
                Ellipse circle = Representation.GetCircle(cluster.X, cluster.Y,
                    radiusSphere + UPPER_LIMIT_MASS, Brushes.White, STROKE_THICKNESS);
                circle.SetValue(Panel.ZIndexProperty, value: -1);
                circle.Stroke = cluster.Color;

                plot.Children.Add(circle);
            }
        }

        private void ClearCoordinate_Click(Object sender, RoutedEventArgs e)
        {
            plot.Children.Clear();
        }

        private void ExecuteStatisticsAnalysis_Click(Object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultDialog = MessageBox.Show(messageBoxText: "Do you want to analyse cluster? \nIf you select \"No\" you will test data with random integer values", caption: "Statistics analysis", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);

            if(resultDialog != MessageBoxResult.Cancel)
            {
                StatisticsAnalysis statisticsAnalysis;
                if(resultDialog == MessageBoxResult.Yes)
                {
                    statisticsAnalysis = new(m_clusterList);
                }
                else
                {
                    statisticsAnalysis = new();
                }

                statisticsAnalysis.Show();
            }
        }
    }
}

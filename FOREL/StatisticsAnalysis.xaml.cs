using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

using Extreme.Statistics;
using Extreme.Statistics.Distributions;

using FOREL.Extensions;

using TAlex.WPF.Controls;

namespace FOREL
{
    /// <summary>
    /// Логика взаимодействия для StatisticsAnalysis.xaml
    /// </summary>
    public partial class StatisticsAnalysis : Window
    {
        private Double[] m_data;
        private List<Cluster> m_clusters;
        private AnalysisType m_analysisType;

        internal StatisticsAnalysis(List<Cluster> clusters)
        {
            InitializeComponent();

            m_analysisType = AnalysisType.Cluster;

            m_clusters = clusters.ToList();

            I_U_D_NumCluster.Maximum = clusters.Count;
            I_U_D_NumCluster.Minimum = 1;
            I_U_D_NumCluster.Value = 1;

            I_U_D_NumCluster.Visibility = Visibility.Visible;
            L_NumCluster.Visibility = Visibility.Visible;

            T_B_ElementCount.Visibility = Visibility.Collapsed;
            L_ElemCount.Visibility = Visibility.Collapsed;
        }

        public StatisticsAnalysis()
        {
            InitializeComponent();

            m_analysisType = AnalysisType.RandomData;
            T_B_ElementCount.Visibility = Visibility.Visible;
            L_ElemCount.Visibility = Visibility.Visible;

            I_U_D_NumCluster.Visibility = Visibility.Collapsed;
            L_NumCluster.Visibility = Visibility.Collapsed;
        }

        private void CalculateWithRandomValues_Click(Object sender, RoutedEventArgs e)
        {
            if(m_analysisType == AnalysisType.RandomData)
            {
                Int32 elementCount = Convert.ToInt32(T_B_ElementCount.Text);
                m_data = new Double[elementCount];

                //max value is excluded
                m_data.FillRandomly(minValue: 0, maxValue: 101);
            }
            else if(m_analysisType == AnalysisType.Cluster)
            {
                Int32 numCluster = (Int32)I_U_D_NumCluster.Value - 1;
                m_data = m_clusters[numCluster].Elems().Select(c => c.Value).ToArray();
            }

            Int32 digitsCountAfterComa = 3;

            Double mean = Stats.Mean(m_data);
            T_B_Mean.Text = Math.Round(mean, digitsCountAfterComa).ToString();

            Double median = Stats.Median(m_data);
            T_B_Median.Text = Math.Round(median, digitsCountAfterComa).ToString();

            Double kurtosis = Stats.Kurtosis(m_data);
            T_B_Kurtosis.Text = Math.Round(kurtosis, digitsCountAfterComa).ToString();

            Double stdev = Stats.StandardDeviation(m_data);
            T_B_Stdev.Text = Math.Round(stdev, digitsCountAfterComa).ToString();
        }
    }
}

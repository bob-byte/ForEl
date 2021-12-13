using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FOREL
{
    class Cluster
    {
        private List<Element> m_elemsOfCluster;

        public Cluster()
        {
            m_elemsOfCluster = new List<Element>();
        }

        public SolidColorBrush Color { get; set; }

        public Double X { get; set; }

        public Double Y { get; set; }

        public void AddElemsOfCluster(params Element[] elements) =>
            m_elemsOfCluster.AddRange(elements);

        //Copy of elements to don't allow change them
        public List<Element> Elems() =>
            m_elemsOfCluster.ToList();
    }
}

using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOREL
{
    class Cluster : Element
    {
        private List<Element> m_elemsOfCluster;

        public Cluster()
        {
            m_elemsOfCluster = new List<Element>();
        }

        public void AddElemsOfCluster(params Element[] elements) =>
            m_elemsOfCluster.AddRange(elements);

        public List<Element> Elems() =>
            m_elemsOfCluster.ToList();
    }
}

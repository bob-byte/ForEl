using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace FOREL
{
    class Forel
    {
        private const Double EPS_DOUBLE = 0.0001;

        private static IMapper s_mapper;

        static Forel()
        {
            MapperConfiguration mapperConfiguration = new(configure => configure.CreateMap<Element, Cluster>());
            s_mapper = mapperConfiguration.CreateMapper();
        }


        private Double SegmentLength(Double x1, Double x2, Double y1, Double y2) => 
            Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

        private List<Element> NeighbourElems(Double radiusSphere, List<Element> sample, Element centerElem)
        {
            List<Element> neighbourElems = new List<Element>();
            foreach (Element elem in sample)
            {
                Double segmentLength = SegmentLength(centerElem.X, elem.X, centerElem.Y, elem.Y);
                if (segmentLength <= radiusSphere)
                {
                    neighbourElems.Add(elem);
                }
            }

            return neighbourElems;
        }

        private Cluster CenterOfElems(List<Element> neighbourElems)
        {
            List<Element> copyOfNeighbourElems = neighbourElems.ToList();

            Double totalMass = copyOfNeighbourElems[0].Mass;

            Double sumXByMass = copyOfNeighbourElems[0].Mass * copyOfNeighbourElems[0].X;
            Double sumYByMass = copyOfNeighbourElems[0].Mass * copyOfNeighbourElems[0].Y;

            for (Int32 elem = 1; elem < copyOfNeighbourElems.Count; elem++)
            {
                totalMass += copyOfNeighbourElems[elem].Mass;
                sumXByMass += copyOfNeighbourElems[elem].Mass * copyOfNeighbourElems[elem].X;
                sumYByMass += copyOfNeighbourElems[elem].Mass * copyOfNeighbourElems[elem].Y;
            }

            Double idealX = sumXByMass / totalMass;
            Double idealY = sumYByMass / totalMass;

            Cluster centerElem = s_mapper.Map<Cluster>(copyOfNeighbourElems[0]);

            for (Int32 elem = 1; elem < copyOfNeighbourElems.Count; elem++)
            {
                Double epsOfNeightbour = Math.Abs(copyOfNeighbourElems[elem].X - idealX) + Math.Abs(copyOfNeighbourElems[elem].Y - idealY);
                Double epsOfCenter = Math.Abs(centerElem.X - idealX) + Math.Abs(centerElem.Y - idealY);
                if (epsOfNeightbour < epsOfCenter)
                {
                    centerElem = s_mapper.Map<Cluster>(copyOfNeighbourElems[elem]);
                }
            }

            centerElem.AddElemsOfCluster(copyOfNeighbourElems.ToArray());

            return centerElem;
        }

        /// <returns>
        /// Centers of clusters
        /// </returns>
        public List<Cluster> Clustering(Double radiusSphere, List<Element> sample)
        {
            List<Element> copySample = sample.ToList();
            Random random = new Random();
            List<Cluster> spheres = new();

            //wheather all elements belong to any cluster
            while (copySample.Count > 0)
            {
                //get random elements
                Element currentElem = copySample[random.Next(0, copySample.Count)];
                List<Element> neighbourElems = NeighbourElems(radiusSphere, copySample, currentElem);
                Cluster centerElem = CenterOfElems(neighbourElems);

                //until the new current object matches the old one
                while (!IsNear(currentElem, centerElem, EPS_DOUBLE))
                {
                    currentElem = centerElem.Clone() as Element;
                    neighbourElems = NeighbourElems(radiusSphere, copySample, currentElem);
                    centerElem = CenterOfElems(neighbourElems);
                }

                //remove clusterizing objects and change their color in input collection
                for (Int32 i = 0; i < neighbourElems.Count; i++)
                {
                    copySample.Remove(neighbourElems[i]);
                    Int32 j = sample.IndexOf(sample.Single(c => c.Equals(neighbourElems[i])));
                    sample[j].Color = centerElem.Color;
                }
                spheres.Add(centerElem);
            }

            return spheres;
        }

        private Boolean IsNear(Element elem1, Element elem2, Double howNear) => 
            (Math.Abs(elem1.X - elem2.X) < howNear) && (Math.Abs(elem1.Y - elem2.Y) < howNear);
    }
}

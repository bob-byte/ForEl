using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FOREL.Extensions
{
    public static class ArrayExtension
    {
        public static T Max<T>(this T[,] array) where T : IComparable
        {
            FindLastElem( array, ( current, intermediateMax ) => intermediateMax.CompareTo( current ) < 0, out T max );
            return max;
        }

        public static T Min<T>(this T[,] array) where T : IComparable
        {
            FindLastElem( array, ( current, intermediateMin ) => intermediateMin.CompareTo( current ) > 0, out T min );
            return min;
        }

        private static void FindLastElem<T>(T[,] array, Func<T, T, Boolean> search, out T foundElem)
            where T : IComparable
        {
            Int32 countRows = array.GetUpperBound(0) + 1;
            Int32 countCols = array.GetUpperBound(1) + 1;

            foundElem = array[0, 0];

            for (Int32 numRow = 0; numRow < countRows; numRow++)
            {
                for (Int32 numColumn = 0; numColumn < countCols; numColumn++)
                {
                    if (search(array[numRow, numColumn], foundElem))
                    {
                        foundElem = array[numRow, numColumn];
                    }
                }
            }
        }

        public static void FillRandomly(this Double[,] array, Int32 digitsAfterComa )
        {
            Int32 countRows = RowCount( array );
            Int32 countCols = ColumnCount( array );
            Random random = new Random();

            for ( Int32 row = 0; row < countRows; row++ )
            {
                for ( Int32 col = 0; col < countCols; col++ )
                {
                    array[ row, col ] = Math.Round(random.NextDouble(), digitsAfterComa);
                }
            }
        }

        public static void FillRandomly(this Double[] array, Int32 minValue, Int32 maxValue)
        {
            Random random = new Random();

            for (Int32 row = 0; row < array.Length; row++)
            {
                array[row] = random.Next(minValue, maxValue);
            }
        }

        public static IEnumerable<Double> Rounded( this IEnumerable<Double> enumerable, Int32 digitsAfterComa )
        {
            BlockingCollection<Double> roundedEnumerable = new BlockingCollection<Double>();

            Task.Factory.StartNew( () =>
             {
                 Parallel.ForEach( enumerable, ( value ) =>
                 {
                     Double roundedValue = Math.Round( value, digitsAfterComa );
                     roundedEnumerable.Add( roundedValue );
                 } );

                 roundedEnumerable.CompleteAdding();
             } );
            
            return roundedEnumerable.GetConsumingEnumerable();
        }

        public static Double[,] RoundedMatrix(this Double[,] matrix, Int32 digitsAfterComa )
        {
            Int32 rowCount = matrix.RowCount();
            Int32 colCount = matrix.ColumnCount();
            Double[,] roundedMatrix = new Double[ rowCount, colCount ];

            Parallel.For( fromInclusive: 0, rowCount, ( numRow ) =>
              {
                  Parallel.For( 0, colCount, ( numCol ) =>
                   {
                       roundedMatrix[ numRow, numCol ] = Math.Round( matrix[ numRow, numCol ], digitsAfterComa );
                   } );
              } );

            return roundedMatrix;
        }

        public static Int32 RowCount<T>(this T[,] array ) =>
            array.GetUpperBound( 0 ) + 1;

        public static Int32 ColumnCount<T>(this T[,] array) =>
            array.GetUpperBound( 1 ) + 1;

        public static T[] Row<T>(this T[,] array, Int32 numRow)
        {
            Int32 countColumns = array.GetUpperBound(1) + 1;

            T[] rowArray = new T[countColumns];
            for (Int32 numColumn = 0; numColumn < countColumns; numColumn++)
            {
                rowArray[numColumn] = array[numRow, numColumn];
            }

            return rowArray;
        }

        public static T[] Column<T>(this T[,] array, Int32 numColumn)
        {
            Int32 countRows = array.GetUpperBound(0) + 1;

            T[] columnArray = new T[countRows];
            for (Int32 numRow = 0; numRow < countRows; numRow++)
            {
                columnArray[numRow] = array[numRow, numColumn];
            }

            return columnArray;
        }

        public static void Swap<T>(this T[] array, Int32 index1, Int32 index2)
            where T: struct
        {
            T temp = array[ index1 ];
            array[ index1 ] = array[ index2 ];
            array[ index2 ] = temp;
        }

        public static DataTable ToDataTable<T>( this T[,] array )
        {
            Int32 rowCount = RowCount( array );
            Int32 colCount = ColumnCount( array );
            var dataTable = new DataTable();

            //Add columns with name "0", "1", "2", ...
            for ( Int32 numCol = 0; numCol < colCount; numCol++ )
            {
                dataTable.Columns.Add( new DataColumn( columnName: numCol.ToString() ) );
            }

            //Add data to dataTable
            for ( Int32 numRow = 0; numRow < rowCount; numRow++ )
            {
                DataRow newRow = dataTable.NewRow();

                for ( Int32 numCol = 0; numCol < colCount; numCol++ )
                {
                    newRow[ numCol ] = array[ numRow, numCol ];
                }

                dataTable.Rows.Add( newRow );
            }

            return dataTable;
        }
    }
}
